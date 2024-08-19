<template>
    <div>
      <!-- Pass custom width, height, labels, and dataset to the BarChart component -->
      <BarChart :labels="labels" :dataset="user.rupchartData" :w="600" :h="400" />
      
    </div>
  </template>
  
  <script setup>
  import { ref } from 'vue';
  import BarChart from '@/components/BarChart.vue';
  import { useUserStore } from '@/stores/store.js'

const user = useUserStore()
  // Initial chart data
  const labels = ref(['Inception', 'Elaboration', 'Construction', 'Transition']);
  
  </script>
  