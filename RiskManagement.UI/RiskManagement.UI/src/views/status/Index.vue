<template>
  <div class="w-full px-4 h-full">
    <v-card class="mx-4 mt-8">
      <v-card-title class="d-flex align-center mb-8">
        <div class="flex justify-between w-full pt-4">
          <div class="w-1/4 font-bold text-xl mt-2">مدیریت وضعیت ریسک‌های پروژه</div>
          <div class="flex w-1/4">
            <v-locale-provider rtl>
              <v-text-field
                dir="rtl"
                class="mt-2 h-12 text-xs"
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
          </div>
        </div>
      </v-card-title>
      <v-data-table
        class="table-content px-8"
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

        <!-- Format the amount with commas and add the ریال symbol -->
        <template v-slot:item.estimatedAmount="{ item }">
          {{ formatCurrency(item.estimatedAmount) }}
        </template>

        <template v-slot:item.finalAmount="{ item }">
          {{ formatCurrency(item.finalAmount) }}
        </template>

        <template v-slot:item.estimatedFinishedDate="{ item }">
          {{ persianTimeInput(item.estimatedFinishedDate) }}
        </template>

        <template v-slot:item.finishedDate="{ item }">
          {{ persianTimeInput(item.finishedDate) }}
        </template>

        <template v-slot:item.status="{ item }">
          {{
            item.status == 0
              ? 'هنوز اتفاق نیفتاده'
              : item.status == 1
                ? 'اتفاق افتاد'
                : 'اتفاق نیفتاد'
          }}
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
              <v-btn class="w-20" color="warning" @click="showUpdateModal(item)">ویرایش</v-btn>
            </v-list-item>
            <v-list-item>
              <v-btn class="w-20" color="red" @click="deleteSolution(item.id)"> حذف</v-btn>
            </v-list-item>
          </v-list>
        </template>
      </v-data-table>
    </v-card>
  </div>
</template>

<script setup>
import { ref, onMounted, watch } from 'vue'
import { useRoute } from 'vue-router'
import { useUserStore } from '@/stores/store.js'
import moment from 'jalali-moment'

const user = useUserStore()
const route = useRoute()

user.routeName = route.name

const isCreateModalVisibleRef = ref(false)
const isUpdateModalVisibleRef = ref(false)

const token = JSON.parse(localStorage.getItem('token'))
const search = ref('')
const itemsPerPage = ref(5)
const currentPage = ref(1)

const headers = [
  { title: 'ردیف', align: 'center', sortable: false, key: 'rowNumber' },
  { title: 'عنوان ریسک', key: 'title', align: 'start' },
  { title: 'وضعیت', key: 'status', align: 'start' },
  { title: 'تاریخ پیش‌بینی شده', key: 'estimatedFinishedDate', align: 'start' },
  { title: 'تاریخ رخداد', key: 'finishedDate', align: 'start' },
  { title: 'هزینه پیش‌بینی شده', key: 'estimatedAmount', align: 'start' },
  { title: 'هزینه نهایی', key: 'finalAmount', align: 'start' },
  { title: 'راه حل انتخابی', key: 'bestSolution.description', align: 'start' },
  { title: '', key: 'actions', sortable: false }
]

const serverItems = ref([])
const loading = ref(true)
const totalItems = ref(0)
const updateItems = ref(null)

const projectId = route.params.id

const loadItems = ({ page, itemsPerPage, sortBy }) => {
  currentPage.value = page // Track the current page
  loading.value = true
  getAllRiskStatus(projectId)
}

function showCreateModal() {
  user.createSolutionModal = true
  isCreateModalVisibleRef.value = true
}

function persianTimeInput(date) {
  return moment(new Date(date)).locale('fa').format('YYYY/MM/DD HH:mm')
}

function showUpdateModal(item) {
  user.updateSolutionModal = true
  isUpdateModalVisibleRef.value = true
  user.solutionUpdateItems = item
  updateItems.value = item
}

function loader() {
  new Promise((resolve) => {
    setTimeout(() => {
      resolve(loadItems({ page: 1, itemsPerPage: itemsPerPage.value }))
    }, 1200)
  })
}

const getAllRiskStatus = async (id) => {
  const url = `${user.url}risk/status/getAllRiskStatusByProjectId?id=${id}`
  try {
    const res = await fetch(url, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: 'Bearer ' + token
      }
    })
    const response = await res.json()
    serverItems.value = response.data
    totalItems.value = response.data.length
    loading.value = false
  } catch (error) {
    console.log('response-getAllRiskStatus', error)
  }
}

// Method to format amount with comma separators and add the "ریال" symbol
const formatCurrency = (amount) => {
  if (!amount) return ''
  return `${amount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',')} ریال`
}

onMounted(() => {
  loadItems({ page: 1, itemsPerPage: itemsPerPage.value })
})

watch(
  () => user.createSolutionModal,
  async (newval) => {
    isCreateModalVisibleRef.value = newval
    if (newval == false) {
      loader()
    }
  }
)

watch(
  () => user.updateSolutionModal,
  async (newval) => {
    isUpdateModalVisibleRef.value = newval
    if (newval == false) {
      loader()
    }
  }
)

watch(
  () => user.solutionUpdateItems,
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
