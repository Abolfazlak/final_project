<template>
  <!-- v-if="chartData.length && chartData[0].length" -->
  <div  class="w-full">
    <div class="RC-Matrix">
      <div class="RC-Matrix-Row" v-for="(row, rowIndex) in user.riskData" :key="rowIndex">
        <div
          v-for="(item, columnIndex) in row"
          :key="columnIndex"
          :style="{ backgroundColor: getColor(rowIndex, columnIndex) }"
          class="RC-Matrix-Column"
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
    ['green', '#008000d9', '#0080009f', '#e1ff00b5', 'yellow'],
    ['#008000d9', '#008000bc', '#ffff00b0', '#ffdd00cb', 'orange'],
    ['#0080009f', '#ffff00b0', '#ffa600c5', 'orange', '#ff0000aa'],
    ['#ffff00b0', '#ffa600c5', 'orange', '#ff0000bc', '#ff0000d9'],
    ['yellow', '#ffa600c5', '#ff0000aa', '#ff0000d9', 'red']
  ]
  return colors[row][col] || 'red'
}
</script>

<style></style>
