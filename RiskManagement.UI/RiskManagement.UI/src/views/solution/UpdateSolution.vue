<template>
  <modal :show="isUpdateModalVisibleRef" @close="closeUpdateModal">
    <template v-slot:header> <div class="font-bold text-base">ویرایش پروژه</div></template>
    <template v-slot:body>
      <v-locale-provider rtl>
        <v-textarea
          v-model="updateItems.description"
          class="mt-2"
          label="شرح راه‌حل"
          variant="outlined"
          rounded="lg"
        ></v-textarea>
        <v-row class="flex gap-3 items-center text-center justify-center mt-2 px-3">
          <v-text-field
            v-model="formattedAmount"
            @input="updateSolutionAmount"
            label="هزینه برآورد شده پس از آن(ریال) "
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
          @click="updateSolution"
          >ثبت</v-btn
        >
        <v-btn class="flex w-2.4/5 py-6 text-center items-center" color="red" rounded="lg" @click="closeUpdateModal"
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

const emit = defineEmits('click', 'closeUpdateModal')

const props = defineProps(['isUpdateModalVisible', 'items'])

const isUpdateModalVisibleRef = ref(props.isUpdateModalVisible)
const updateItems = ref(props.items)

let token = JSON.parse(localStorage.getItem('token'))

async function updateSolution() {
  const updateModel = reactive({
    id: updateItems.value.id,
    amount: updateItems.value.amount,
    description: updateItems.value.description,
    riskId: route.params.riskId
  })

  let url = `${user.url}risk/solution/updateSolution`
  try {
    let res = await fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: 'Bearer ' + token
      },
      body: JSON.stringify(updateModel)
    })
    await res.json().then((response) => {
      closeUpdateModal()
      toast.success(response.data)
    })
  } catch (error) {
    console.log('response-updateSolution', error)
  }
}


const formattedAmount = computed({
  get() {
    return formatNumber(updateItems.value.amount);
  },
  set(newValue) {
    updateItems.value.amount = parseNumber(newValue);
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
  updateItems.value.amount = parseNumber(value);
}

function closeUpdateModal() {
  isUpdateModalVisibleRef.value = false
  user.updateSolutionModal = false
  user.solutionUpdateItems = null
}

watch(
  () => props.isUpdateModalVisible,
  async (newval) => {
    isUpdateModalVisibleRef.value = newval
  }
)

watch(
  () => props.items,
  async (newval) => {
    updateItems.value = newval
  }
)
</script>

<style></style>
