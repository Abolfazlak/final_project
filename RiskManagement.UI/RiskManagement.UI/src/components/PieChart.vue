<template>
  <div :style="{ width: '400px', height: '400px' }">
    <Pie :data="chartData" :options="chartOptions" />
    </div>
  </template>
  
  <script setup>
  import { computed, ref } from 'vue';
  import { Pie } from 'vue-chartjs';
  import {
    Chart as ChartJS,
    Title,
    Tooltip,
    Legend,
    ArcElement,
    CategoryScale,
  } from 'chart.js';
  
  // Register required components for Chart.js
  ChartJS.register(Title, Tooltip, Legend, ArcElement, CategoryScale);
  
  // Define props for labels and dataset
  const props = defineProps({
    labels: {
      type: Array,
      required: true,
    },
    dataset: {
      type: Array,
      required: true,
    },
  });
  
  // Compute the chart data using the props
  const chartData = computed(() => ({
    labels: props.labels,
    datasets: [
      {
        label: 'Categories',
        backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56'],
        data: props.dataset,
      },
    ],
  }));
  
  // Chart options
  const chartOptions = ref({
    responsive: true,
    plugins: {
      legend: {
        position: 'bottom',
      },
      title: {
        display: false,
        text: 'Pie Chart Example',
      },
    },
  });
  </script>
  