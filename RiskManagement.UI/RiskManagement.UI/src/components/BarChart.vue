<template>
    <div :style="{ width:'500px', height:'300px' }">
      <Bar :data="chartData" :options="chartOptions" />
    </div>
  </template>
  
  <script setup>
  import { computed, ref } from 'vue';
  import { Bar } from 'vue-chartjs';
  import {
    Chart as ChartJS,
    Title,
    Tooltip,
    Legend,
    BarElement,
    CategoryScale,
    LinearScale,
  } from 'chart.js';
  
  // Register required components for Chart.js
  ChartJS.register(Title, Tooltip, Legend, BarElement, CategoryScale, LinearScale);
  
  // Define props for labels, dataset, width (w), and height (h)
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
        backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56', '#4BC0C0'],
        data: props.dataset,
      },
    ],
  }));
  
  // Chart options
  const chartOptions = ref({
    responsive: true,
    maintainAspectRatio: false, // Allow custom width/height
    plugins: {
      legend: {
        position: 'none',
      },
      title: {
        display: false,
        text: 'Bar Chart Example',
      },
    },
    scales: {
      y: {
        beginAtZero: true,
      },
    },
  });
  </script>
  