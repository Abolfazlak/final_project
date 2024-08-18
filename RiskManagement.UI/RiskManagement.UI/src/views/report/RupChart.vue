<template>
    <div>
      <!-- Pass custom width, height, labels, and dataset to the BarChart component -->
      <BarChart :labels="labels" :dataset="dataset" :w="600" :h="400" />
      
    </div>
  </template>
  
  <script setup>
  import { ref } from 'vue';
  import BarChart from '@/components/BarChart.vue';
  
  // Initial chart data
  const labels = ref(['Label 1', 'Label 2', 'Label 3', 'Label 4']);
  const dataset = ref([10, 20, 30, 40]);
  
  // Function to update the chart data
  const updateChartData = () => {
    labels.value = ['New Label 1', 'New Label 2', 'New Label 3', 'New Label 4'];
    dataset.value = [15, 25, 35, 45];
  };
  </script>
  