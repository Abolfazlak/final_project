<template>
  <div>
    <div class="flex flex-row gap-12 justify-center mt-16">
      <div class="reportContiner flex flex-col justify-center items-center w-2/5 px-16">
        <div class="flex mb-6 text-xl text-black font-bold">ماتریس احتمال - شدت ریسک</div>
        <risk-chart></risk-chart>
      </div>
      <div class="reportContiner flex flex-col justify-center items-center w-2/5 px-16">
        <div class="flex mb-6 text-xl text-black font-bold">نمودار وضعیت ریسک‌ها و فرصت‌ها</div>
        <risk-status-chart v-if="user.pichartData"></risk-status-chart>
      </div>
    </div>
    <div class="flex flex-row gap-12 justify-center mt-16 pb-8">
      <div class="reportContiner flex flex-col justify-center items-center w-2/5 px-16">
        <div class="flex mb-6 text-xl text-black font-bold">ماتریس احتمال - شدت فرصت</div>
        <opportunity-chart v-if="user.oppData"></opportunity-chart>
      </div>
      <div class="reportContiner flex flex-col justify-center items-center w-2/5 px-16">
        <div class="flex mb-6 text-xl text-black font-bold">نمودار هزینه </div>
        <bar-chart v-if="user.amountData"></bar-chart>
      </div>
    </div>
  </div>
</template>
<script setup>
import { useRoute, useRouter } from 'vue-router'
import { useUserStore } from '@/stores/store.js'
import RiskChart from './RiskChart.vue'
import BarChart from './BarChart.vue'
import RiskStatusChart from './RiskStatusChart.vue'
import OpportunityChart from './OpportunityChart.vue'
import RupChart from './RupChart.vue'
import { ref, watch } from 'vue'

const user = useUserStore()
const route = useRoute()

user.routeName = route.name

const projectId = route.params.id

// const riskData = [
//   [1, 2, 1, 1, 1],
//   [2, 1, 1, 5, 1],
//   [1, 1, 1, 1, 1],
//   [1, 1, 1, 1, 1],
//   [1, 1, 1, 1, 1]
// ]

const riskData = ref([])

// const oppData = [
//   [1, 2, 1],
//   [2, 1, 1],
//   [1, 1, 1]
// ]

let rupData = {}

const res = ref(null)
let token = JSON.parse(localStorage.getItem('token'))

async function getReports(id) {
  let url = `${user.url}report/getReport?id=${id}`
  try {
    let res = await fetch(url, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: 'Bearer ' + token
      }
    })
    if (res.status == 401) {
      user.Logout()
    }
    await res.json().then((response) => {
      console.log('response-getReports', response)
      res.value = response
      user.riskData = response.risks
      user.oppData = response.opportunities
      user.pichartData = [response.status[0], response.status[1], response.status[2]]
      user.rupchartData = response.rup
      user.amountData = response.amount

      rupData = response.rup
    })
  } catch (error) {
    console.log('response-getReports', error)
  }
}

getReports(projectId)
</script>
<style>
.reportContiner {
  height: 70vh !important;
  background-color: white;
  box-shadow: rgba(165, 159, 149, 0.3) 0px 8px 16px;
}
.RC-Matrix {
  display: flex;
  flex-direction: column;
}
.RC-Matrix-Column {
  display: flex;
  align-items: center;
  justify-content: center;
  background-color: rgb(255, 255, 255);
  width: 80px;
  height: 80px;
  font-size: 25px;
  font-family: 'Yekan-Bakh-Heavy';
}
.RC-Matrix-Row {
  display: flex;
  direction: ltr;
}
</style>
