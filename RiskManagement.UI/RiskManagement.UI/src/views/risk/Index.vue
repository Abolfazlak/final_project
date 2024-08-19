<template>
  <div class="w-full h-full">
    <v-card class="mx-6 mt-8">
      <v-card-title class="d-flex align-center mb-8">
        <div class="flex justify-between w-full pt-4">
          <div class="w-1/4 font-bold text-2xl mt-2">مدیریت ریسک های پروژه</div>
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
        v-if="user.hasRiskData"
        class="table-content px-8 pb-2"
        v-model:search="search"
        :headers="headers"
        :items="serverItems"
        :loading="loading"
        item-value="id"
        @update:options="loadItems"
      >
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
            <v-list-item
              ><v-btn class="w-20" color="#77817E" @click="showDetailsModal(item.id)"
                >جزيیات</v-btn
              ></v-list-item
            >
            <v-list-item>
              <v-btn class="w-20 text-white" color="#4da35a" @click="gotoSolutions(item.id)"
                >راه‌حل‌ها</v-btn
              ></v-list-item
            >
            <v-list-item>
              <v-btn class="w-20" color="#EC622E" @click="showUpdateModal(item)">
                ویرایش</v-btn
              ></v-list-item
            >
            <v-list-item>
              <v-btn class="w-20" variant="outlined" color="red" @click="deleteRisk(item.id)">
                حذف</v-btn
              ></v-list-item
            >
          </v-list>
        </template>
      </v-data-table>
      <div v-else class="flex justify-center text-center mt-24 pb-128 text-xl"> داده‌ای جهت نمایش وجود ندارد</div>
    </v-card>
  </div>

  <create-risk :isCreateModalVisible="isCreateModalVisibleRef"></create-risk>
  <update-risk :isUpdateModalVisible="isUpdateModalVisibleRef" :items="updateItems"></update-risk>
  <risk-details
    :riskId="riskIdRef"
    :isDetailsModalVisible="isDetailsModalVisibleRef"
  ></risk-details>
</template>

<script setup>
import { ref, onMounted, watch } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useUserStore } from '@/stores/store.js'
import CreateRisk from './CreateRisk.vue'
import UpdateRisk from './UpdateRisk.vue'
import RiskDetails from './RiskDetails.vue'

import { toast } from 'vue3-toastify'

const user = useUserStore()
const route = useRoute()
const router = useRouter()

user.routeName = route.name

const isCreateModalVisibleRef = ref(false)
const isUpdateModalVisibleRef = ref(false)
const isDetailsModalVisibleRef = ref(false)

const token = JSON.parse(localStorage.getItem('token'))
const search = ref('')
const itemsPerPage = ref(5)
const currentPage = ref(1)

const headers = [
  { title: 'ردیف', align: 'start', sortable: false, key: 'rowNumber' },
  { title: 'عنوان ریسک', key: 'title', align: 'start' },
  { title: 'گروه ریسک', key: 'mainRiskCategory.title', align: 'start' },
  { title: 'دسته‌بندی ریسک', key: 'secondaryRiskCategory.title', align: 'start' },
  { title: '', key: 'actions', sortable: false, align: 'start' }
]
const serverItems = ref([])
const loading = ref(true)
const totalItems = ref(0)
const updateItems = ref(null)
const riskIdRef = ref(0)
user.hasRiskData = true

const loadItems = ({ page, itemsPerPage, sortBy }) => {
  currentPage.value = page // Track the current page
  loading.value = true
  getAllRisks(route.params.id)
}

function showCreateModal() {
  user.createRiskModal = true
  isCreateModalVisibleRef.value = true
}

function showUpdateModal(item) {
  user.isFirstGetCategory = false
  user.updateRiskModal = true
  isUpdateModalVisibleRef.value = true
  user.riskUpdateItems = item
  updateItems.value = item
}

function showDetailsModal(id) {
  user.riskDetailsModal = true
  isDetailsModalVisibleRef.value = true
  riskIdRef.value = id
}

function loader() {
  new Promise((resolve) => {
    setTimeout(() => {
      resolve(loadItems({ page: 1, itemsPerPage: itemsPerPage.value }))
    }, 1200)
  })
}

const getAllRisks = async (id) => {
  const url = `${user.url}risk/getAllRisksByProjectId?id=${id}`
  try {
    const res = await fetch(url, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: 'Bearer ' + token
      }
    })
    if (res.status == 404) {
      user.hasRiskData = false
    }
    const response = await res.json()
    serverItems.value = response.data
    totalItems.value = response.data.length
    loading.value = false
  } catch (error) {
    console.log('response-getAllRisks', error)
  }
}

const deleteRisk = async (id) => {
  const url = `${user.url}risk/deleteRisk?id=${id}`
  try {
    const res = await fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: 'Bearer ' + token
      }
    })
    const response = await res.json()
    toast.success(response.data)
    loadItems({ page: 1, itemsPerPage: itemsPerPage.value })
  } catch (error) {
    toast.error(error.message)
    console.log('response-deleteRisk', error)
  }
}

function gotoSolutions(id) {
  router.push({ name: 'solutions', params: { id: route.params.id, riskId: id } })
}

onMounted(() => {
  loadItems({ page: 1, itemsPerPage: itemsPerPage.value })
})

watch(
  () => user.createRiskModal,
  async (newval) => {
    isCreateModalVisibleRef.value = newval
    if (newval == false) {
      loader()
    }
  }
)

watch(
  () => user.updateRiskModal,
  async (newval) => {
    isUpdateModalVisibleRef.value = newval
    if (newval == false) {
      loader()
    }
  }
)

watch(
  () => user.riskDetailsModal,
  async (newval) => {
    isDetailsModalVisibleRef.value = newval
    if (newval == false) {
      loader()
    }
  }
)

watch(
  () => user.riskUpdateItems,
  async (newval) => {
    updateItems.value = newval
  }
)
</script>

<style>
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
  height: 65vh;
  overflow-y: auto;
}

.btn-txt {
  display: flex;
  font-family: 'Yekan-Bakh-Heavy';
  font-size: medium;
  justify-content: center;
  align-items: center;
}

</style>
