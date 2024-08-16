<template>
  <modal :show="isCreateModalVisibleRef" @close="closeCreateModal">
    <template v-slot:header> <div class="font-bold text-base">افزودن راه‌حل جدید</div></template>
    <template v-slot:body>
      <v-locale-provider rtl>
        <v-textarea
          v-model="createModel.description"
          class="mt-2"
          label="شرح راه‌حل"
          variant="outlined"
          rounded="lg"
        ></v-textarea>
        <v-row class="flex gap-3 items-center text-center justify-center mt-2 px-3">
          <v-text-field
            v-model="formattedAmount"
            @input="updateSolutionAmount"
            label="هزینه برآورد شده پس از آن "
            variant="outlined"
            rounded="lg"
            type="text"
            required
          ></v-text-field>
        </v-row>
      </v-locale-provider>
    </template>
    <template v-slot:footer>
      <v-row class="flex text-center items-center justify-center mt-8 px-3 gap-3 mb-1">
        <v-btn
          class="flex w-2.4/5 py-6 text-center items-center"
          color="primary"
          rounded="lg"
          @click="createSolutionBtn"
          >ثبت</v-btn
        >
        <v-btn
          class="flex w-2.4/5 py-6 text-center items-center"
          color="red"
          rounded="lg"
          @click="closeCreateModal"
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

const user = useUserStore()
const route = useRoute()

const emit = defineEmits('click', 'closeCreateModal')
const props = defineProps(['isCreateModalVisible'])

let token = JSON.parse(localStorage.getItem('token'))

const isCreateModalVisibleRef = ref(props.isCreateModalVisible)

const createModel = reactive({
  amount: 0,
  description: '',
  riskId: route.params.riskId
})


const formattedAmount = computed({
  get() {
    return formatNumber(createModel.amount);
  },
  set(newValue) {
    createModel.amount = parseNumber(newValue);
  }
});

function formatNumber(value) {
  if (value === null || value === undefined) return '';
  return value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',');
}

function parseNumber(value) {
  return parseInt(value.toString().replace(/,/g, ''), 10);
}

function updateSolutionAmount(event) {
  const value = event.target.value;
  createModel.amount = parseNumber(value);
}


function closeCreateModal() {
  isCreateModalVisibleRef.value = false
  user.createSolutionModal = false
  createModel.description = ''
  createModel.amount = 0
}

function createSolutionBtn() {
  createSolution()
  closeCreateModal()
}

async function createSolution() {
  let url = `${user.url}risk/solution/addSolution`
  try {
    let res = await fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: 'Bearer ' + token
      },
      body: JSON.stringify(createModel)
    })
    await res.json().then((response) => {
      toast.success(response.data)
    })
  } catch (error) {
    toast.error('عملیات با خطا مواجه شد')
  }
}

watch(
  () => props.isCreateModalVisible,
  async (newval) => {
    isCreateModalVisibleRef.value = newval
  }
)
</script>

<style></style>
