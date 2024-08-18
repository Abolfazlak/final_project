<template>
  <div>
    <div class="flex flex-row gap-12 justify-center mt-16">
      <div class="reportContiner flex flex-col justify-center items-center bg-red w-2/5 px-16">
        <div class="flex mb-6 text-xl text-black font-bold">نمودار وضعیت ریسک</div>
        <risk-chart :data="riskData"></risk-chart>
      </div>
      <div class="reportContiner flex flex-col justify-center items-center bg-red w-2/5 px-16">
        <div class="flex mb-6 text-xl text-black font-bold">نمودار وضعیت ریسک</div>
        <risk-chart :data="oppData"></risk-chart>
      </div>
    </div>
    <div class="flex flex-row gap-12 justify-center mt-16">
      <div class="reportContiner flex flex-col justify-center items-center bg-red w-2/5 px-16">
        <div class="flex mb-6 text-xl text-black font-bold">نمودار وضعیت ریسک</div>
        <risk-chart :data="data"></risk-chart>
      </div>
      <div class="reportContiner flex flex-col justify-center items-center bg-red w-2/5 px-16">
        <div class="flex mb-6 text-xl text-black font-bold">نمودار وضعیت ریسک</div>
        <risk-chart :data="data"></risk-chart>
      </div>
    </div>
  </div>
</template>
<script setup>
import { useRoute, useRouter } from 'vue-router'
import { useUserStore } from '@/stores/store.js'
import RiskChart from './RiskChart.vue'

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
  [1, 2, 1, 1, 1],
  [2, 1, 1, 5, 1],
  [1, 1, 1, 1, 1],
  [1, 1, 1, 1, 1],
  [1, 1, 1, 1, 1]
]

const data = [
  [1, 2, 1, 1, 1],
  [2, 1, 1, 5, 1],
  [1, 1, 1, 1, 1],
  [1, 1, 1, 1, 1],
  [1, 1, 1, 1, 1]
]
</script>
<style>
.reportContiner {
  height: 70vh !important;
}
.RC-Matrix {
  display: flex;
  flex-direction: column;
}
.RC-Matrix-Column {
  display: flex;
  align-items: center;
  justify-content: center;
  background-color: red;
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
