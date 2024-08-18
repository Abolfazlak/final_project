<template>
  <div v-if="data.length && data[0].length" class="w-full">
    <div class="RC-Matrix">
      <div class="RC-Matrix-Row" v-for="(row, rowIndex) in data" :key="rowIndex">
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

let chartData = ref([]);

watch(
  () => props.data,
  (newval) => {
    if (newval && Array.isArray(newval) && newval.length > 0) {
      chartData.val = newval
    } else {
      data = []
    }
    console.log('data', data)
  }
)

function getColor(row, col) {
  // Custom function to return colors based on row and column indices
  // You can use any logic to determine the color
  const colors = [
    ['#35B08B', '#94CD8B', '#FEEA81', '#FEEA81', 'aqua'],
    ['#94CD8B', '#FEEA81', '#FEEA81', 'aqua', 'aqua'],
    ['#FEEA81', '#FEEA81', 'aqua', 'aqua', 'aqua'],
    ['#FEEA81', 'aqua', 'aqua', 'aqua', 'aqua'],
    ['aqua', 'aqua', 'aqua', 'aqua', 'aqua']
  ]
  return colors[row][col] || 'red';
}
</script>
