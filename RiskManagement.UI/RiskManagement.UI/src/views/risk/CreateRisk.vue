<template>
  <modal :show="isCreateModalVisibleRef" @close="closeCreateModal">
    <template v-slot:header> <div class="font-bold text-base">افزودن ریسک جدید</div></template>
    <template v-slot:body>
      <v-locale-provider rtl>
        <v-row class="flex gap-3 items-center text-center justify-center mt-2 px-3">
          <v-text-field
            v-model="createRiskModel.title"
            label="عنوان ریسک"
            variant="outlined"
            rounded="lg"
            required
          ></v-text-field>
        </v-row>
        <v-row class="flex gap-3 items-center text-center justify-center mt-6 px-3">
          <div class="w-full flex">
            <v-select
              v-model="mainCategoryId"
              class="w-1/2 ml-3"
              label="نوع ریسک"
              variant="outlined"
              rounded="lg"
              :items="mainCategories"
              item-title="title"
              item-value="id"
            ></v-select>
            <v-select
              v-model="createRiskModel.secondaryRiskCategory"
              class="w-1/2"
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
          class="flex w-2.4/5 py-6 text-center items-center text-white"
          color="#4da35a"
          rounded="lg"
          @click="createProjectBtn"
          >ثبت</v-btn
        >
        <v-btn
          class="flex w-2.4/5 py-6 text-center items-center"
          color="red"
          variant= "outlined"
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
import { ref, onMounted, reactive, watch } from 'vue'
import { useUserStore } from '@/stores/store.js'
import { toast } from 'vue3-toastify'
import { useRoute } from 'vue-router'


const user = useUserStore()
const route = useRoute()

const emit = defineEmits('click', 'closeCreateModal')
const props = defineProps(['isCreateModalVisible'])

let token = JSON.parse(localStorage.getItem('token'))
let projectId = route.params.id;

const isCreateModalVisibleRef = ref(props.isCreateModalVisible)
const mainCategories = ref([])
const secondaryRiskCategories = ref([])
const mainCategoryId = ref(null)



const createRiskModel = reactive({
  projectId: projectId,
  title: '',
  secondaryRiskCategory: null,
})

function closeCreateModal() {
  isCreateModalVisibleRef.value = false
  user.createRiskModal = false
}

function createProjectBtn() {
  createRisk()
  closeCreateModal()
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
    if(res.status == 401){
      user.Logout();
    }
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
    if(res.status == 401){
      user.Logout();
    }
    await res.json().then((response) => {
      secondaryRiskCategories.value = response.data
    })
  } catch (error) {
    secondaryRiskCategories.value = []
    console.log('response-getMainCategories', error)
  }
}

async function createRisk() {
  let url = `${user.url}risk/addRisk`
  try {
    let res = await fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: 'Bearer ' + token
      },
      body: JSON.stringify(createRiskModel)
    })
    if(res.status == 401){
      user.Logout();
    }
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

watch(
  () => mainCategoryId.value,
  async (newval) => {
    console.log('newval', newval)
    if(newval > 0){
      getSecondaryRiskCategories(newval)
    }
  }
)
</script>

<style></style>
