<template>
  <div class="w-full h-full pb-20">
    <v-card class="ml-8 mt-8">
      <v-card-title class="d-flex align-center pe-2 mb-16">
        <div class="flex justify-between w-full pt-4">
          <div class="w-1/4 font-bold text-2xl mt-2">مدیریت راه‌حل‌های پروژه</div>
          <div class="flex w-1/3">
            <v-text-field
              class="mt-2 h-16"
              items-per-page="5"
              v-model="search"
              density="compact"
              label="جستجو"
              prepend-inner-icon="mdi-magnify"
              variant="solo-filled"
              flat
              hide-details
              single-line
            ></v-text-field>
            <v-btn class="mt-2 mr-4 px-6 ml-4 h-12" color="primary" @click="showCreateModal">ایجاد</v-btn>
          </div>
        </div>
      </v-card-title>
      <v-data-table
        class="table-content px-8 pb-14"
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
        <template v-slot:item.amount="{ item }">
          {{ formatCurrency(item.amount) }}
        </template>

        <template v-slot:item.actions="{ item }">
          <v-btn
            @click="item.showBtn = !item.showBtn"
            class="my-4"
            density="default"
            icon="mdi-format-list-bulleted"
            color="info"
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

  <create-solution :isCreateModalVisible="isCreateModalVisibleRef"></create-solution>
  <update-solution :isUpdateModalVisible="isUpdateModalVisibleRef" :items="updateItems"></update-solution>

</template>

<script setup>
import { ref, onMounted, watch } from 'vue'
import { useRoute } from 'vue-router'
import { useUserStore } from '@/stores/store.js'
import { toast } from 'vue3-toastify'
import CreateSolution from '@/views/solution/CreateSolution.vue'
import UpdateSolution from '@/views/solution/UpdateSolution.vue'

const user = useUserStore()
const route = useRoute()

const isCreateModalVisibleRef = ref(false)
const isUpdateModalVisibleRef = ref(false)

const token = JSON.parse(localStorage.getItem('token'))
const search = ref('')
const itemsPerPage = ref(5)
const currentPage = ref(1)

const headers = [
  { title: 'ردیف', align: 'start', sortable: false, key: 'rowNumber' },
  { title: 'عنوان راه‌حل', key: 'description', align: 'center' },
  { title: 'هزینه برآورد شده پس از آن', key: 'amount', align: 'center' },
  { title: '', key: 'actions', sortable: false }
]
const serverItems = ref([])
const loading = ref(true)
const totalItems = ref(0)
const updateItems = ref(null)

const riskId = route.params.riskId

const loadItems = ({ page, itemsPerPage, sortBy }) => {
  currentPage.value = page // Track the current page
  loading.value = true
  getAllSolutions(riskId)
}

function showCreateModal() {
  user.createSolutionModal = true
  isCreateModalVisibleRef.value = true
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
    serverItems.value = response.data
    totalItems.value = response.data.length
    loading.value = false
  } catch (error) {
    console.log('response-getAllSolutions', error)
  }
}

const deleteSolution = async (id) => {
  const url = `${user.url}risk/solution/deleteSolution?id=${id}`
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
    console.log('response-deleteSolution', error)
  }
}

// Method to format amount with comma separators and add the "ریال" symbol
const formatCurrency = (amount) => {
  if (!amount) return ''
  return `${amount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")} ریال`
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
  font-size: large;
}

.v-data-table__td {
  font-weight: normal;
  font-size: medium;
}

.table-content{
  height: 69vh;
  overflow-y: auto;
}
</style>
