<template>
  <!-- v-if="data.length && data[0].length" -->
  <div  class="w-full">
    <div class="RCO-Matrix">
      <div class="RCO-Matrix-Row" v-for="(row, rowIndex) in user.oppData" :key="rowIndex">
        <div
          v-for="(item, columnIndex) in row"
          :key="columnIndex"
          :style="{ backgroundColor: getColor(rowIndex, columnIndex) }"
          class="RCO-Matrix-Column"
        >
          {{ item }}
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { useRoute } from 'vue-router'
import { useUserStore } from '@/stores/store.js'
import { watch, ref } from 'vue'

const user = useUserStore()
const route = useRoute()
user.routeName = route.name

const props = defineProps(['data'])


function getColor(row, col) {
  // Custom function to return colors based on row and column indices
  // You can use any logic to determine the color
  const colors = [
    ['green', '#94CD8B', 'yellow'],
    ['#94CD8B', 'orange', '#f54444e1'],
    ['yellow', '#f54444e1', 'red']
  ]
  return colors[row][col] || 'red'
}
</script>

<style>
.RCO-Matrix {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
}
.RCO-Matrix-Column {
  display: flex;
  align-items: center;
  justify-content: center;
  background-color: red;
  width: 80px;
  height: 80px;
  font-size: 25px;
  font-family: 'Yekan-Bakh-Heavy';
}
.RCO-Matrix-Row {
  display: flex;
  direction: ltr;
}
</style>
