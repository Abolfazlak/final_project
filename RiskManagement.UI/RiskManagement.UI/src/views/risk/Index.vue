<template>
  <v-card class="ml-24 mt-24">
    <v-card-title class="d-flex align-center pe-2">
      <div class="flex justify-between w-full pt-4 pb-16">
        <div class="w-1/4 font-bold text-2xl mt-2">مدیریت ریسک های پروژه</div>
        <div class="flex w-1/3">
          <v-text-field
            class="mt-2 h-16"
            items-per-page="10"
            v-model="search"
            density="compact"
            label="جستجو"
            prepend-inner-icon="mdi-magnify"
            variant="solo-filled"
            flat
            hide-details
            single-line
          ></v-text-field>
          <v-btn class="mt-2 mr-4 px-6 ml-4 h-12" color="primary">ایجاد</v-btn>
        </div>
      </div>
    </v-card-title>
    <v-data-table
    class="px-8"
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
          class="my-4"
          density="default"
          icon="mdi-format-list-bulleted"
          color="info"
        ></v-btn>
        <v-list class="absolute z-50 mr-16 -mt-12 shadow-lg rounded-lg" v-if="item.showBtn">
          <v-list-item><v-btn class="w-20" color="info">جزيیات</v-btn></v-list-item>
          <v-list-item> <v-btn class="w-20" color="success">راه‌حل‌ها</v-btn></v-list-item>
          <v-list-item> <v-btn class="w-20" color="warning"> ویرایش</v-btn></v-list-item>
          <v-list-item> <v-btn class="w-20" color="red"> حذف</v-btn></v-list-item>
        </v-list>
      </template>
    </v-data-table>
  </v-card>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { useUserStore } from '@/stores/store.js'

const user = useUserStore()
const route = useRoute()

const token = JSON.parse(localStorage.getItem('token'))
const search = ref('')
const itemsPerPage = ref(10)
const currentPage = ref(1)

const headers = [
{
    title: 'ردیف',
    align: 'start',
    sortable: false,
    key: 'rowNumber'
  },
  { title: 'عنوان ریسک', key: 'title', align: 'start' },
  { title: 'گروه ریسک', key: 'mainRiskCategory.title', align: 'start' },
  { title: 'دسته‌بندی ریسک', key: 'secondaryRiskCategory.title', align: 'start' },
  { title: '', key: 'actions', sortable: false }
]
const serverItems = ref([])
const loading = ref(true)
const totalItems = ref(0)

const loadItems = ({ page, itemsPerPage, sortBy }) => {
  currentPage.value = page  // Track the current page
  loading.value = true
  getAllRisks(route.params.id)
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
    const response = await res.json()
    serverItems.value = response.data
    totalItems.value = response.data.length
    loading.value = false
  } catch (error) {
    console.log('response-getAllRisks', error)
  }
}

onMounted(() => {
  loadItems({ page: 1, itemsPerPage: itemsPerPage.value })
})
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
.v-data-table-header__content span{
  font-weight: bold;
  font-size: large;
}

.v-data-table__td {
  font-weight: normal;
  font-size: medium;
}
</style>
