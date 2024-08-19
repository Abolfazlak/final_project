<template>
  <div>
    <PieChart :labels="labels" :dataset="user.pichartData" />
  </div>
</template>

<script setup>
import { ref } from 'vue'
import PieChart from '@/components/PieChart.vue'
import { watch } from 'vue'
import { useUserStore } from '@/stores/store.js'

const user = useUserStore()

// Initial chart data
const labels = ref(['هنوز نه', 'اتفاق افتاده', 'اتفاق نیفتاده'])
</script>
