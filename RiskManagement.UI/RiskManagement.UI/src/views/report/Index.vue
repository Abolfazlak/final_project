<template>
  <div>
    <div class="flex flex-row gap-12 justify-center mt-16">
      <div class="reportContiner flex flex-col justify-center items-center  w-2/5 px-16">
        <div class="flex mb-6 text-xl text-black font-bold">ماتریس احتمال - شدت ریسک</div>
        <risk-chart :data="riskData"></risk-chart>
      </div>
      <div class="reportContiner flex flex-col justify-center items-center  w-2/5 px-16">
        <div class="flex mb-6 text-xl text-black font-bold">نمودار وضعیت ریسک‌ها</div>
        <risk-status-chart></risk-status-chart>
      </div>
    </div>
    <div class="flex flex-row gap-12 justify-center mt-16 pb-8">
      <div class="reportContiner flex flex-col justify-center items-center  w-2/5 px-16">
        <div class="flex mb-6 text-xl text-black font-bold">ماتریس احتمال - شدت فرصت</div>
        <opportunity-chart :data="oppData"></opportunity-chart>
      </div>
      <div class="reportContiner flex flex-col justify-center items-center w-2/5 px-16" v-if="Object.keys(rupData).length !== 0">
        <div class="flex mb-6 text-xl text-black font-bold">نمودار فازهای آبشاری</div>
        <rup-chart></rup-chart>
      </div>
      <div class="flex flex-col justify-center items-center w-2/5 px-16" v-else></div>
    </div>
  </div>
</template>
<script setup>
import { useRoute, useRouter } from 'vue-router'
import { useUserStore } from '@/stores/store.js'
import RiskChart from './RiskChart.vue'
import RiskStatusChart from './RiskStatusChart.vue'
import OpportunityChart from './OpportunityChart.vue'
import RupChart from './RupChart.vue'

const user = useUserStore()
const route = useRoute()

user.routeName = route.name

const riskData = [
  [1, 2, 1, 1, 1],
  [2, 1, 1, 5, 1],
  [1, 1, 1, 1, 1],
  [1, 1, 1, 1, 1],
  [1, 1, 1, 1, 1]
]

const oppData = [
  [1, 2, 1],
  [2, 1, 1],
  [1, 1, 1]
]

const statusData = [10, 5, 3]

const rupData = {}

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
