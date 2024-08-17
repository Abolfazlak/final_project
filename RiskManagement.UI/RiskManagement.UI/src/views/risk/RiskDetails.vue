<template>
  <modal :show="isDetailsModalVisibleRef" @close="closeDetailsModal">
    <template v-slot:header> <div class="font-bold text-base">جزئیات ریسک</div></template>
    <template v-slot:body>
      <v-locale-provider rtl>
        <v-container class="h-96 overflow-auto">
          <v-row class="flex gap-3 items-center text-center justify-center mt-2 px-3">
            <v-textarea
              v-model="riskDetailModel.description"
              label="جزئیات ریسک"
              variant="outlined"
              rounded="lg"
              required
            ></v-textarea>
          </v-row>
          <v-row class="flex gap-3 items-center text-center justify-center mt-8 px-3">
            <v-radio-group inline label="تهدید یا فرصت؟" v-model="riskDetailModel.isOpportunity">
              <v-radio class="ml-16" label="تهدید" :value="false"></v-radio>
              <v-radio label="فرصت" :value="true"></v-radio>
            </v-radio-group>

            <v-select
              v-if="methodologyRef == 'rup'"
              v-model="riskDetailModel.rupPhase"
              class="w-1/4"
              label="فاز"
              variant="outlined"
              rounded="lg"
              :items="rupPhases"
            >
            </v-select>
          </v-row>

          <v-row class="flex w-1/2 gap-3 items-center text-center justify-center mt-8 px-3">
            <v-text-field
              v-if="!riskDetailModel.isOpportunity"
              v-model="formattedRiskAmount"
              @input="updateRiskAmount"
              label="هزینه برآورد شده ناشی از ریسک(ریال)"
              variant="outlined"
              rounded="lg"
              required
              type="text"
            ></v-text-field>
          </v-row>

          <v-row class="flex w-1/2 gap-3 items-center text-center justify-center mt-8 px-3">
            <v-text-field
              v-if="riskDetailModel.isOpportunity"
              v-model="riskDetailModel.estimatedOpportunityAmount"
              label="هزینه برآورد شده ناشی از فرصت(ریال)"
              variant="outlined"
              rounded="lg"
              required
              type="number"
            ></v-text-field>
          </v-row>

          <v-row
            v-if="!riskDetailModel.isOpportunity"
            class="flex gap-3 items-center text-center justify-center mt-8 px-3"
          >
            <v-radio-group
              inline
              label="احتمال وقوع ریسک"
              v-model="riskDetailModel.riskProbability"
            >
              <v-radio class="ml-8" label="بسیار محتمل" :value="5"></v-radio>
              <v-radio class="ml-8" label="محتمل" :value="4"></v-radio>
              <v-radio class="ml-8" label="متوسط" :value="3"></v-radio>
              <v-radio class="ml-8" label="غیر محتمل" :value="2"></v-radio>
              <v-radio class="ml-8" label="بسیار غیر محتمل" :value="1"></v-radio>
            </v-radio-group>
          </v-row>
          <v-row
            v-if="!riskDetailModel.isOpportunity"
            class="flex gap-3 items-center text-center justify-center mt-8 px-3"
          >
            <v-radio-group
              inline
              label="عواقب ناشی از وقوع ریسک"
              v-model="riskDetailModel.riskImpact"
            >
              <v-radio class="ml-8" label="فاجعه آمیز" :value="5"></v-radio>
              <v-radio class="ml-8" label="اساسی" :value="4"></v-radio>
              <v-radio class="ml-8" label="متوسط" :value="3"></v-radio>
              <v-radio class="ml-8" label="جزئی" :value="2"></v-radio>
              <v-radio class="ml-8" label="بدون عواقب" :value="1"></v-radio>
            </v-radio-group>
          </v-row>
          <v-row
            v-if="riskDetailModel.isOpportunity"
            class="flex gap-3 items-center text-center justify-center mt-8 px-3"
          >
            <v-radio-group
              inline
              label="احتمال وقوع فرصت"
              v-model="riskDetailModel.opportunityProbability"
            >
              <v-radio class="ml-16" label="محتمل" :value="3"></v-radio>
              <v-radio class="ml-16" label="متوسط" :value="2"></v-radio>
              <v-radio class="ml-16" label="غیر محتمل" :value="1"></v-radio>
            </v-radio-group>
          </v-row>
          <v-row
            v-if="riskDetailModel.isOpportunity"
            class="flex gap-3 items-center text-center justify-center mt-8 px-3"
          >
            <v-radio-group
              inline
              label="عواقب ناشی از وقوع فرصت"
              v-model="riskDetailModel.opportunityImpact"
            >
              <v-radio class="ml-16" label="اساسی" :value="3"></v-radio>
              <v-radio class="ml-16" label="متوسط" :value="2"></v-radio>
              <v-radio class="ml-16" label="جزئی" :value="1"></v-radio>
            </v-radio-group>
          </v-row>
          <v-row class="flex gap-3 mt-12 px-3">
            <div class="flex">
              <div class="w-1/4 text-gray mr-6 ml-16">تاریخ بررسی:</div>
              <custom-date-picker
                class="mr-20"
                v-model="riskDetailModel.estimatedDateTime"
                format="jYYYY/jMM/jDD HH:mm"
              />
            </div>
          </v-row>
          <v-col class="gap-3 mt-12 px-3">
            <div class="flex">
              <div class="w-1/3 text-gray mr-2">انتخاب پیوست:</div>
              <input
                type="file"
                @change="handleFileUpload"
                accept=".zip,.pdf"
                class="block w-full text-sm text-gray-500 file:mr-16 file:py-2 file:px-4 file:rounded-full file:border-0 file:text-sm file:font-semibold file:bg-blue-50 file:text-blue-700 hover:file:bg-blue-100"
              />
            </div>

            <div class="flex mt-12">
              <div v-if="riskDetailModel.document" class="w-1/3 text-gray mr-2">
                پیوست‌های قبلی:
              </div>
              <div
                v-if="riskDetailModel.document"
                class="flex items-start gap-2 cursor-pointer"
                @click="viewExistingFile"
              >
                <v-icon>mdi-zip-box-outline</v-icon>
                <span>{{ getFileName(documentRef) }}</span>
              </div>
            </div>
          </v-col>
        </v-container>
      </v-locale-provider>
    </template>
    <template v-slot:footer>
      <v-row class="flex text-center items-center justify-center mt-8 px-3 gap-3 mb-1">
        <v-btn
          class="flex w-2.4/5 py-6 text-center items-center"
          color="#4da35a"
          rounded="lg"
          @click="addRiskDetailsBtn"
          >ثبت</v-btn
        >
        <v-btn
          class="flex w-2.4/5 py-6 text-center items-center"
          color="red"
          rounded="lg"
          variant="outlined"
          @click="closeDetailsModal"
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

const projectId = route.params.id

const emit = defineEmits('click', 'closeDetailsModal')
const props = defineProps(['isDetailsModalVisible', 'riskId'])

let token = JSON.parse(localStorage.getItem('token'))
const riskDetailModel = reactive({})
const riskIdRef = ref(null)
const methodologyRef = ref(null)
const hasRiskDetail = ref(false)
const documentRef = ref(null)

const isDetailsModalVisibleRef = ref(props.isDetailsModalVisible)

const rupPhases = ['Inception', 'Elaboration', 'Construction', 'Transition']


const formattedRiskAmount = computed({
  get() {
    return formatNumber(riskDetailModel.estimatedRiskAmount);
  },
  set(newValue) {
    riskDetailModel.estimatedRiskAmount = parseNumber(newValue);
  }
});

function formatNumber(value) {
  if (value === null || value === undefined) return '';
  return value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',');
}

function parseNumber(value) {
  return parseInt(value.toString().replace(/,/g, ''), 10);
}

function updateRiskAmount(event) {
  const value = event.target.value;
  riskDetailModel.estimatedRiskAmount = parseNumber(value);
}


function persianTimeInput(date) {
  return moment(new Date(date)).locale('fa').format('YYYY/MM/DD HH:mm')
}
function englishTime(date) {
  return moment(date, 'jYYYY/jMM/jDD').format('YYYY/MM/DD HH:mm')
}

function closeDetailsModal() {
  Object.assign(riskDetailModel, {})
  isDetailsModalVisibleRef.value = false
  user.riskDetailsModal = false
}

function addRiskDetailsBtn() {
  if (riskDetailModel.isOpportunity) {
    riskDetailModel.riskProbability = null
    riskDetailModel.riskImpact = null
    riskDetailModel.estimatedRiskAmount = 0
  }
  if (!riskDetailModel.isOpportunity) {
    riskDetailModel.opportunityProbability = null
    riskDetailModel.opportunityImpact = null
    riskDetailModel.estimatedOpportunityAmount = 0

  }
  let x = new Date(englishTime(riskDetailModel.estimatedDateTime))
  console.log(x)
  riskDetailModel.estimatedDateTime = x.toISOString()
  riskDetailModel.riskId = riskIdRef.value
  console.log('riskDetailModel', riskDetailModel)

  if (hasRiskDetail.value) {
    updateRiskDetails()
  } else {
    addRiskDetails()
  }
  closeDetailsModal()
}

async function addRiskDetails() {
  let url = `${user.url}risk/detail/addRiskDetail`
  try {
    let res = await fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: 'Bearer ' + token
      },
      body: JSON.stringify(riskDetailModel)
    })
    await res.json().then((response) => {
      toast.success(response.data)
    })
  } catch (error) {
    toast.error('عملیات با خطا مواجه شد')
  }
}

function viewExistingFile() {
  if (riskDetailModel.document.endsWith('.pdf')) {
    window.open(riskDetailModel.document, '_blank') // Open PDF in new tab
  } else if (riskDetailModel.document.endsWith('.zip')) {
    const a = document.createElement('a')
    a.href = riskDetailModel.document
    a.download = getFileName(riskDetailModel.document)
    document.body.appendChild(a)
    a.click()
    document.body.removeChild(a)
  }
}

function getFileName(filePath) {
  return filePath.split('/').pop()
}

function handleFileUpload(event) {
  const file = event.target.files[0]
  const reader = new FileReader()

  reader.onloadend = () => {
    riskDetailModel.document = reader.result.split(',')[1] // Store the base64 string without the prefix
  }

  if (file) {
    reader.readAsDataURL(file)
  }
}

async function updateRiskDetails() {
  let url = `${user.url}risk/detail/updateRiskDetail`
  try {
    let res = await fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: 'Bearer ' + token
      },
      body: JSON.stringify(riskDetailModel)
    })
    await res.json().then((response) => {
      toast.success(response.data)
    })
  } catch (error) {
    toast.error('عملیات با خطا مواجه شد')
  }
}

async function getRiskDetail(riskId) {
  let url = `${user.url}risk/detail/getRiskDetail?id=${riskId}`
  try {
    let res = await fetch(url, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: 'Bearer ' + token
      }
    })
    if (res.status == 404) {
      riskDetailModel.isOpportunity = false
    }
    await res.json().then((response) => {
      hasRiskDetail.value = true
      Object.assign(riskDetailModel, response.data)
      documentRef.value = response.data.document
      riskDetailModel.estimatedDateTime = persianTimeInput(response.data.estimatedDateTime)
    })
  } catch (error) {
    console.log('response.content', error)
  }
}

async function getMethodology(id) {
  let url = `${user.url}project/getProjectMethodologyById?id=${id}`
  try {
    let res = await fetch(url, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: 'Bearer ' + token
      }
    })
    await res.json().then((response) => {
      methodologyRef.value = response.data
    })
  } catch (error) {
    console.log('response.content', error)
  }
}

watch(
  () => props.isDetailsModalVisible,
  async (newval) => {
    isDetailsModalVisibleRef.value = newval
    if (newval == true) {
      getRiskDetail(riskIdRef.value)
      getMethodology(projectId)
    }
  }
)

watch(
  () => props.riskId,
  async (newval) => {
    riskIdRef.value = newval
  }
)
</script>

<style>

</style>
