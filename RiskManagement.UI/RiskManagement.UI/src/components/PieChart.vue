<template>
  <v-locale-provider rtl>
    <div :style="{ width: '400px', height: '400px' }">
      <Pie :data="chartData" :options="chartOptions" />
    </div>
  </v-locale-provider>
</template>

<script setup>
import { computed, ref } from 'vue'
import { Pie } from 'vue-chartjs'
import { Chart as ChartJS, Title, Tooltip, Legend, ArcElement, CategoryScale } from 'chart.js'
import ChartDataLabels from 'chartjs-plugin-datalabels'

// Register required components and plugins for Chart.js
ChartJS.register(Title, Tooltip, Legend, ArcElement, CategoryScale, ChartDataLabels)

// Define props for labels and dataset
const props = defineProps({
  labels: {
    type: Array,
    required: true
  },
  dataset: {
    type: Array,
    required: true
  }
})

// Compute the chart data using the props
const chartData = computed(() => ({
  labels: props.labels,
  datasets: [
    {
      label: 'تعداد',
      backgroundColor: ['#6d6b66c5', '#ff0000d9', '#4da35a'],
      data: props.dataset
    }
  ]
}))

// Chart options
const chartOptions = ref({
  responsive: true,
  plugins: {
    legend: {
      position: 'bottom'
    },
    datalabels: {
      display: true,
      color: '#fff',
      font: {
        size: 20, // Set the font size
        weight: 'bold' // Set the font weight (optional)
      },
      formatter: (value) => {
        return value > 0 ? value : '' // Show value only if it's greater than 0
      }
    },
    title: {
      display: false,
      text: 'Pie Chart Example'
    }
  }
})
</script>
