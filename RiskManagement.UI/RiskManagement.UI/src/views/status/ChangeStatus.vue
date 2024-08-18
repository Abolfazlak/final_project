<template>
  <modal :show="isModalVisibleRef" @close="closeModal">
    <template v-slot:header> <div class="font-bold text-base">جزئیات ریسک</div></template>
    <template v-slot:body>
      <v-locale-provider rtl>
        <v-container class="h-96 overflow-auto">
          <v-row class="flex gap-3 items-center text-center justify-center mt-2 px-3">
            <v-select
              v-model="model.status"
              class="w-1/2"
              label="وضعیت"
              variant="outlined"
              rounded="lg"
              :items="statusList"
              item-title="title"
              item-id="id"
            ></v-select>
            <v-row class="flex w-1/2 gap-3 items-center text-center justify-center px-3">
              <v-text-field
                v-model="formattedRiskAmount"
                @input="updateRiskAmount"
                label="هزینه نهایی ناشی از ریسک"
                variant="outlined"
                rounded="lg"
                required
                type="text"
              ></v-text-field>
            </v-row>
          </v-row>

          <v-row class="flex gap-3 mt-16 px-3">
            <div class="flex">
              <div class="w-1/3 text-gray ml-12 justify-center mt-1">تاریخ تغییر وضعیت:</div>
              <custom-date-picker
                class=""
                v-model="model.finishedDate"
                format="jYYYY/jMM/jDD HH:mm"
                label=""
                size="xl"
              />
            </div>
          </v-row>

          <v-row
            class="flex items-center text-center justify-center mt-16"
            v-if="statusRef == 'اتفاق نیفتاد'"
          >
            <v-radio-group inline label="آیا راه‌حل جدید است؟" v-model="model.isNewSolution">
              <v-radio class="ml-16" label="بله" :value="true"></v-radio>
              <v-radio label="خیر" :value="false"></v-radio>
            </v-radio-group>
          </v-row>

          <v-row class="flex gap-3 items-center text-center justify-center mt-16 px-3">
            <v-textarea
              v-if="isNewSolutionRef"
              v-model="model.solutionTitle"
              class="mt-2"
              label="شرح راه‌حل"
              variant="outlined"
              rounded="lg"
            ></v-textarea>
            <v-select
              v-else
              v-model="chosenSolution"
              class="w-1/2"
              label="راه‌حل"
              variant="outlined"
              rounded="lg"
              :items="solutionsRef"
              item-title="description"
              item-id="id"
              return-object
            ></v-select>
          </v-row>
        </v-container>
      </v-locale-provider>
    </template>
    <template v-slot:footer>
      <v-row class="flex text-center items-center justify-center mt-8 px-3 gap-3 mb-1">
        <v-btn
          class="flex w-2.4/5 py-6 text-center items-center"
          color="#4da35a"
          rounded="lg"
          @click="addNewStatusBtn"
          >ثبت</v-btn
        >
        <v-btn
          class="flex w-2.4/5 py-6 text-center items-center"
          color="red"
          rounded="lg"
          variant="outlined"
          @click="closeModal"
          >انصراف</v-btn
        >
      </v-row>
    </template>
  </modal>
</template>

<script setup>
import modal from '@/components/CreateUpdateModal.vue'
import { ref, onMounted, reactive, watch, computed } from 'vue'
import { useUserStore } from '@/stores/store.js'
import { toast } from 'vue3-toastify'
import { useRoute } from 'vue-router'
import moment from 'jalali-moment'

const user = useUserStore()
const route = useRoute()

const emit = defineEmits('click', 'closeModal')
const props = defineProps(['isModalVisible', 'riskId'])

const statusList = [
  { id: 1, title: 'اتفاق افتاد' },
  { id: 2, title: 'اتفاق نیفتاد' }
]
let token = JSON.parse(localStorage.getItem('token'))
const model = reactive({
  status: null,
  finalAmount: 0,
  finishedDate: null,
  isNewSolution: false,
  solutionTitle: null
})

const riskIdRef = ref(0)
const solutionsRef = ref([])
const chosenSolution = ref(null)
const statusRef = ref(0)
const isModalVisibleRef = ref(props.isModalVisible)
const isNewSolutionRef = ref(false)

const formattedRiskAmount = computed({
  get() {
    return formatNumber(model.finalAmount)
  },
  set(newValue) {
    model.finalAmount = parseNumber(newValue)
  }
})

function formatNumber(value) {
  if (value === null || value === undefined) return ''
  return value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',')
}

function parseNumber(value) {
  return parseInt(value.toString().replace(/,/g, ''), 10)
}

function updateRiskAmount(event) {
  const value = event.target.value
  model.estimatedRiskAmount = parseNumber(value)
}

function englishTime(date) {
  return moment(date, 'jYYYY/jMM/jDD').format('YYYY/MM/DD HH:mm')
}

function closeModal() {
  Object.assign(model, {})
  isModalVisibleRef.value = false
  user.changeStatusModal = false
}

function addNewStatusBtn() {
  let x = new Date(englishTime(model.finishedDate))
  model.finishedDate = x.toISOString()
  model.id = riskIdRef.value

  if (model.status == 'اتفاق افتاد') {
    model.status = 1
  }
  if (model.status == 'اتفاق نیفتاد') {
    model.status = 2
  }

  if (!model.isNewSolution) {
    model.solutionId = chosenSolution.value.id
  }
  console.log('model', model)

  changeStatus()
}

async function changeStatus() {
  let url = `${user.url}risk/status/changeRiskStatus`
  try {
    let res = await fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: 'Bearer ' + token
      },
      body: JSON.stringify(model)
    })
    await res.json().then((response) => {
      toast.success(response.data)
      closeModal()
    })
  } catch (error) {
    toast.error('عملیات با خطا مواجه شد')
  }
}

const getAllSolutions = async (id) => {
  const url = `${user.url}risk/solution/getAllSolutionByRiskId?id=${id}`
  try {
    const res = await fetch(url, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: 'Bearer ' + token
      }
    })
    const response = await res.json()
    solutionsRef.value = Array.isArray(response.data) ? response.data : [];
  } catch (error) {
    console.log('response-getAllSolutions', error)
  }
}

watch(
  () => props.isModalVisible,
  async (newval) => {
    isModalVisibleRef.value = newval
  }
)

watch(
  () => props.riskId,
  async (newval) => {
    riskIdRef.value = Number(newval)
    if (newval) {
      getAllSolutions(newval)
    }
  }
)

watch(
  () => model.status,
  async (newval) => {
    console.log('newval', newval)
    statusRef.value = newval
  }
)

watch(
  () => model.isNewSolution,
  async (newval) => {
    isNewSolutionRef.value = newval
  }
)
watch(
  () => solutionsRef.value,
  (newVal) => {
    console.log('solutionsRef updated:', newVal);
  }
);
</script>

<style></style>
