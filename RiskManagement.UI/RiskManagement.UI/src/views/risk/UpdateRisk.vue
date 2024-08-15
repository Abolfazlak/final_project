<template>
  <modal :show="isUpdateModalVisibleRef" @close="closeUpdateModal">
    <template v-slot:header> <div class="font-bold text-base">افزودن ریسک جدید</div></template>
    <template v-slot:body>
      <v-locale-provider rtl>
        <v-row class="flex gap-3 items-center text-center justify-center mt-2 px-3">
          <v-text-field
            v-model="updateItems.title"
            label="عنوان ریسک"
            variant="outlined"
            rounded="lg"
            required
          ></v-text-field>
        </v-row>
        <v-row class="flex gap-3 items-center text-center justify-center mt-6 px-3">
          <div class="w-full flex">
            <v-select
              v-model="mainCategory"
              class="w-1/2 ml-2"
              label="نوع ریسک"
              variant="outlined"
              rounded="lg"
              :items="mainCategories"
              item-title="title"
              item-id="id"
              return-object
            ></v-select>
            <v-select
              v-model="updateItems.secondaryRiskCategory"
              class="w-1/2 ml-2"
              label="دسته‌بندی ریسک"
              variant="outlined"
              rounded="lg"
              :items="secondaryRiskCategories"
              item-title="title"
              item-id="id"
              return-object
            ></v-select>
          </div>
        </v-row>
      </v-locale-provider>
    </template>
    <template v-slot:footer>
      <v-row class="flex text-center items-center justify-center mt-8 px-3 gap-3 mb-1">
        <v-btn
          class="flex w-2.4/5 py-6 text-center items-center"
          color="primary"
          rounded="lg"
          @click="updateRisktBtn"
          >ثبت</v-btn
        >
        <v-btn
          class="flex w-2.4/5 py-6 text-center items-center"
          color="red"
          rounded="lg"
          @click="closeUpdateModal"
          >انصراف</v-btn
        >
      </v-row>
    </template>
  </modal>
</template>

<script setup>
import modal from '@/components/CreateUpdateModal.vue'
import { ref, onMounted, reactive, watch } from 'vue'
import { useUserStore } from '@/stores/store.js'
import { toast } from 'vue3-toastify'
import { useRoute } from 'vue-router'


const user = useUserStore()
const route = useRoute()

const emit = defineEmits('click', 'closeUpdateModal')
const props = defineProps(['isUpdateModalVisible', 'items'])

const updateItems = ref(props.items)

let token = JSON.parse(localStorage.getItem('token'))

const isUpdateModalVisibleRef = ref(props.isUpdateModalVisible)
const mainCategories = ref([])
const secondaryRiskCategories = ref([])

const mainCategory = ref(null)

const isFirst = ref(false);
let projectId = route.params.id;

function closeUpdateModal() {
  isUpdateModalVisibleRef.value = false
  user.updateRiskModal = false
}

function updateRisktBtn() {
  updateRisk()
  closeUpdateModal()
}
onMounted(() => {
  getMainCategories()
})

async function getMainCategories() {
  let url = `${user.url}risk/getMainRiskCategories`
  try {
    let res = await fetch(url, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: 'Bearer ' + token
      }
    })
    await res.json().then((response) => {
      mainCategories.value = response.data
    })
  } catch (error) {
    console.log('response-getMainCategories', error)
  }
}

async function getSecondaryRiskCategories(id) {
  let url = `${user.url}risk/getSecondaryRiskCategories?id=${id}`
  try {
    let res = await fetch(url, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: 'Bearer ' + token
      }
    })
    await res.json().then((response) => {
      secondaryRiskCategories.value = response.data
    })
  } catch (error) {
    secondaryRiskCategories.value = []
    console.log('response-getMainCategories', error)
  }
}

async function updateRisk() {
  let url = `${user.url}risk/updateRisk`
  try {
    updateItems.value.projectId = Number(projectId)

    let res = await fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: 'Bearer ' + token
      },
      body: JSON.stringify(updateItems.value)
    })
    await res.json().then((response) => {
      toast.success(response.data)
    })
  } catch (error) {
    toast.error('عملیات با خطا مواجه شد')
  }
}

watch(
  () => props.isUpdateModalVisible,
  async (newval) => {
    isUpdateModalVisibleRef.value = newval
  }
)

watch(
  () => mainCategory.value,
  async (newval) => {
    console.log('newval', newval)
    if(newval.id > 0){
      if(user.isFirstGetCategory){
        updateItems.value.secondaryRiskCategory = null
      }
      getSecondaryRiskCategories(newval.id)
      user.isFirstGetCategory = true;
    }
  }
)

watch(
  () => props.items,
  async (newval) => {
    updateItems.value = newval
    mainCategory.value = newval.mainRiskCategory
  }
)
</script>

<style></style>
