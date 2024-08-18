<template>
  <div>
    <PieChart :labels="labels" :dataset="dataset" />
  </div>
</template>

<script setup>
import { ref } from 'vue'
import PieChart from '@/components/PieChart.vue'

// Initial chart data
const labels = ref(['Category A', 'Category B', 'Category C'])
const dataset = ref([40, 30, 30])

</script>
