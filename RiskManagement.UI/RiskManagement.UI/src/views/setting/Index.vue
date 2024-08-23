<template>
  <div class="w-full px-4 h-full">
    <v-card class="mx-4 mt-8">
      <v-card-title class="d-flex align-center mb-8">
        <div class="flex justify-between w-full pt-4">
          <div class="w-1/4 font-bold text-xl mt-2">مدیریت کاربران</div>
          <div class="flex w-1/3">
            <v-locale-provider rtl>
              <v-text-field
                class="mt-2 h-12"
                items-per-page="5"
                v-model="search"
                density="small"
                label="جستجو"
                prepend-inner-icon="mdi-magnify"
                variant="solo-filled"
                flat
                hide-details
                single-line
              ></v-text-field>
            </v-locale-provider>
            <v-btn
              class="btn-txt mt-2 mr-4 px-6 ml-4 h-9 text-center items-center flex text-xs text-white"
              color="#4da35a"
              @click="showCreateModal"
              >ایجاد</v-btn
            >
          </div>
        </div>
      </v-card-title>
      <v-data-table
        v-if="user.hasRiskStatusData"
        class="table-content px-8"
        v-model:search="search"
        :headers="headers"
        :items="serverItems"
        :loading="loading"
        item-value="id"
        @update:options="loadItems"
      >
      <template v-slot:item.isActive="{ item }">
          <div v-if="item.isActive" class="text-green">فعال</div>
          <div v-if="!item.isActive" class="text-red">غیرفعال</div>
        </template>

        <template v-slot:item.rowNumber="{ index }">
          {{ index + 1 + itemsPerPage * (currentPage - 1) }}
        </template>
        <template v-slot:item.actions="{ item }">
          <v-btn
            @click="item.showBtn = !item.showBtn"
            class="my-4 text-black"
            size="small"
            density="default"
            icon="mdi-format-list-bulleted"
            color="white"
          ></v-btn>
          <v-list class="absolute z-50 mr-16 -mt-16 shadow-lg rounded-lg" v-if="item.showBtn">
            <v-list-item>
              <v-btn class="w-24" color="#EC622E" @click="changePassword(item.id)">
                تغییر رمز عبور</v-btn
              ></v-list-item
            >
            <v-list-item>
              <v-btn class="w-24" variant="outlined" color="red" @click="changeIsActive(item.id)" v-if="item.isActive">
                غیرفعال کردن</v-btn>

                <v-btn class="w-24" variant="outlined" color="green" @click="changeIsActive(item.id)" v-else>
                  فعال کردن</v-btn
              ></v-list-item
            >
          </v-list>
        </template>
      </v-data-table>
      <div v-else class="flex justify-center text-center mt-24 pb-128 text-xl">
        داده‌ای جهت نمایش وجود ندارد
      </div>
    </v-card>
  </div>
  <create-user :isCreateModalVisible="isCreateModalVisibleRef"></create-user>
</template>

<script setup>
import { ref, onMounted, watch } from 'vue'
import { useRoute } from 'vue-router'
import { useUserStore } from '@/stores/store.js'
import { toast } from 'vue3-toastify'
import CreateUser from './CreateUser.vue'

const user = useUserStore()
const route = useRoute()

user.routeName = route.name
user.hasSettingData = true

const isCreateModalVisibleRef = ref(false)
const isPasswordModalVisibleRef = ref(false)

const userId = ref(0)

const token = JSON.parse(localStorage.getItem('token'))
const search = ref('')
const itemsPerPage = ref(5)
const currentPage = ref(1)

const headers = [
  { title: 'ردیف', align: 'center', sortable: false, key: 'rowNumber' },
  { title: 'نام و نام خانوادگی', key: 'fullName', align: 'start' },
  { title: 'نام کاربری', key: 'userName', align: 'start' },
  { title: 'ایمیل', key: 'email', align: 'start' },
  { title: 'وضعیت', key: 'isActive', align: 'start' },
  { title: '', key: 'actions', sortable: false }
]

const serverItems = ref([])
const loading = ref(true)
const totalItems = ref(0)

const loadItems = ({ page, itemsPerPage, sortBy }) => {
  currentPage.value = page // Track the current page
  loading.value = true
  getAllUsers()
}

function changePassword(id) {
  user.changePasswordModal = true
  isPasswordModalVisibleRef.value = true
  userId.value = id
}

function showCreateModal(){
  user.createUserModal = true
  isCreateModalVisibleRef.value = true
}

async function changeIsActive(id){
  const url = `${user.url}users/changeUserActivity?id=${id}`
  try {
    const res = await fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: 'Bearer ' + token
      }
    })
    if (res.status == 404) {
      return;
    }
    const response = await res.json()
    toast.success(response.data)
    loader()
  } catch (error) {
    toast.error(error)
  }
}

function loader() {
  new Promise((resolve) => {
    setTimeout(() => {
      resolve(loadItems({ page: 1, itemsPerPage: itemsPerPage.value }))
    }, 1200)
  })
}

const getAllUsers = async () => {
  const url = `${user.url}users/getAllUsers`
  try {
    const res = await fetch(url, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: 'Bearer ' + token
      }
    })
    if (res.status == 404) {
      user.hasSettingData = false
    }
    const response = await res.json()
    serverItems.value = response.data
    totalItems.value = response.data.length
    loading.value = false
  } catch (error) {
    console.log('response-getAllUsers', error)
  }
}

onMounted(() => {
  loadItems({ page: 1, itemsPerPage: itemsPerPage.value })
})

watch(
  () => user.changePasswordModal,
  async (newval) => {
    isPasswordModalVisibleRef.value = newval
    if (newval == false) {
      loader()
    }
  }
)

watch(
  () => user.createUserModal,
  async (newval) => {
    isCreateModalVisibleRef.value = newval
    if (newval == false) {
      loader()
    }
  }
)
</script>

<style>
.disable-btn:hover {
  cursor: default;
}
.v-data-table-footer__items-per-page span {
  display: none;
}
.v-select__selection-text {
  display: block !important ;
}
.v-data-table-footer {
  margin-top: 30px;
  direction: ltr;
}
.v-data-table-header__content span {
  font-weight: bold;
  font-size: medium;
}

.v-data-table__td {
  font-weight: normal;
  font-size: medium;
}

.table-content {
  height: 66vh;
  overflow-y: auto;
  overflow-x: auto;
}

.btn-txt {
  display: flex;
  font-family: 'Yekan-Bakh-Heavy';
  font-size: medium;
  justify-content: center;
  align-items: center;
}

.v-data-table-header__content {
  width: 150px !important;
}

.v-data-table-footer__info {
  display: none !important;
}
</style>
