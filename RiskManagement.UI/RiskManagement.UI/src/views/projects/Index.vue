<template>
  <div>
    <div class="project-title-box pt-8 justify-center items-center">
      <div class="project-title flex mb-8 mr-8 pb-4 ml-8 font-semibold text-xl">
        <div class="w-1/4 mr-4 font-bold text-1.5xl mt-2">مشاهده پروژه‌های موجود</div>
        <v-btn
          v-if="isAdminRef"
          class="mt-2 mr-4 px-6 ml-4 h-12"
          color="#4da35a"
          @click="showCreateModal"
          >ایجاد</v-btn
        >
      </div>
    </div>
    <div class="">
      <div class="v-card-container flex flex-col w-full px-32">
        <div v-for="(item, index) in projects" :key="index" class="v-card py-10">
          <div class="w-full flex justify-between items-center px-2">
            <div class="flex flex-col justify-between w-9/12" @click="gotoRisks(item.id)">
              <div class="flex">
                <h2 class="text-3xl font-bold">{{ item.projectName }}</h2>
              </div>
              <div class="flex pt-4 w-full">
                <div class="flex text-lg w-1/5">
                  متدولوژی :‌
                  <p class="font-bold pr-4">{{ item.methodology }}</p>
                </div>
                <div class="flex text-lg w-1/2">
                  توضیحات :
                  <p class="font-bold pr-4">{{ item.description }}</p>
                </div>
              </div>
            </div>
            <div>
              <v-btn class="h-10 w-24 mx-2" @click="showUpdateModal(item)" color="#EC622E">
                <v-icon :icon="updateIcon"></v-icon> ویرایش
              </v-btn>
              <v-btn v-if="isAdminRef" variant="outlined" class="h-10 w-24" @click="deleteItem(item.id)" color="red">
                <v-icon :icon="deleteIcon"></v-icon> حذف
              </v-btn>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>

  <create-project :isCreateModalVisible="isCreateModalVisibleRef"> </create-project>
  <update-project
    :isUpdateModalVisible="isUpdateModalVisibleRef"
    :items="updateItems"
  ></update-project>
</template>

<script setup>
import { ref, onMounted, reactive, watch } from 'vue'
import { useUserStore } from '@/stores/store.js'
import CreateProject from './CreateProject.vue'
import UpdateProject from './UpdateProject.vue'
import { useRouter, useRoute } from 'vue-router'
import { toast } from 'vue3-toastify'

onMounted(() => {
  getAllProjects()
})

const user = useUserStore()
const projects = ref([])
const router = useRouter()
const route = useRoute()

user.routeName = route.name


const deleteIcon = 'mdi-delete'
const updateIcon = 'mdi-pencil'

let token = JSON.parse(localStorage.getItem('token'))

const isCreateModalVisibleRef = ref(false)
const isUpdateModalVisibleRef = ref(false)
const isAdminRef = ref(user.isAdmin)

const updateItems = ref(null)

function showCreateModal() {
  user.createProjectModal = true
  isCreateModalVisibleRef.value = true
}

function gotoRisks(id) {
  console.log('id', id)
  router.push({ name: 'risks', params: { id: id } })
}

function showUpdateModal(item) {
  user.updateProjectModal = true
  isUpdateModalVisibleRef.value = true
  user.updateItems = item
  updateItems.value = item
}

async function deleteItem(id) {
  let url = `${user.url}project/deleteProject?id=${id}`
  try {
    let res = await fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: 'Bearer ' + token
      }
    })
    await res.json().then((response) => {
      console.log('response-getProjects', response)
      toast.success(response.message)
      getAllProjects()
    })
  } catch (error) {
    console.log('response-getProjects', error)
  }
}

async function getAllProjects() {
  let url = `${user.url}project/getAllProjects`
  try {
    let res = await fetch(url, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: 'Bearer ' + token
      }
    })
    await res.json().then((response) => {
      console.log('response-getProjects', response)
      projects.value = response.data
    })
  } catch (error) {
    console.log('response-getProjects', error)
  }
}

function loader() {
  new Promise((resolve) => {
    setTimeout(() => {
      resolve(getAllProjects())
    }, 2000)
  })
}

watch(
  () => user.createProjectModal,
  async (newval) => {
    isCreateModalVisibleRef.value = newval
    if(newval == false){
      loader()
    }
  }
)

watch(
  () => user.updateProjectModal,
  async (newval) => {
    isUpdateModalVisibleRef.value = newval
    if(newval == false){
      loader()
    }
  }
)

watch(
  () => user.updateItems,
  async (newval) => {
    updateItems.value = newval
  }
)

watch(
  () => user.isAdmin,
  async (newval) => {
    isAdminRef.value = newval
  }
)
</script>

<style scoped>
.project-title {
  border-bottom: 1.2px black solid;
  justify-content: space-between;
}

.v-card-container :hover {
  transform: scale(1.006);
  cursor: pointer;
}

.v-card {
  border: 1px solid #ccc;
  border-radius: 8px;
  padding: 16px;
  margin: 8px;
  flex: 1 1 calc(50% - 16px);
  box-sizing: border-box;

  background: #f5e4dede;
  height: 25vh;
}
</style>
