<template>
  <div>
    <canvas ref="chartCanvas"></canvas>
  </div>
</template>

<script setup>
import { ref, onMounted, watch } from 'vue'
import { Chart, registerables } from 'chart.js'
import ChartDataLabels from 'chartjs-plugin-datalabels'
import { useUserStore } from '@/stores/store.js'

Chart.register(...registerables, ChartDataLabels)

const chartCanvas = ref(null)
const user = useUserStore()

const amount = ref(null)

function loader() {
  const chartData = {
    labels: ['هزینه پیش‌بینی شده', 'هزینه نهایی'],
    datasets: [
      {
        label: 'نامشخص',
        data: [amount.value[0].sumOfEstimatedAmount, user.amountData[0].sumOfFinalAmount],
        backgroundColor: '#707070',
        borderColor: 'rgba(255, 255, 255, 1)',
        borderWidth: 1
      },
      {
        label: 'مرتفع شده',
        data: [amount.value[1].sumOfEstimatedAmount, user.amountData[1].sumOfFinalAmount],
        backgroundColor: '#008000d9',
        borderColor: 'rgba(75, 192, 192, 1)',
        borderWidth: 1
      },
      {
        label: 'اتفاق افتاده',
        data: [amount.value[2].sumOfEstimatedAmount, user.amountData[2].sumOfFinalAmount],
        backgroundColor: '#ff0000d9',
        borderColor: 'rgba(153, 102, 255, 1)',
        borderWidth: 1
      }
    ]
  }

  const chartOptions = {
    responsive: true,
    scales: {
      x: {
        stacked: false
      },
      y: {
        stacked: false,
        beginAtZero: true
      }
    },
    plugins: {
      legend: {
        position: 'bottom'
      },
      datalabels: {
        anchor: 'end',
        align: 'top',
        formatter: (value) => {
          return '' // Format number with commas
        },
        display: 'white'
      }
    }
  }

  new Chart(chartCanvas.value, {
    type: 'bar',
    data: chartData,
    options: chartOptions
  })
}


watch(
  () => user.amountData,
  async (newval) => {
    amount.value = newval
    loader()
  }
)
</script>

<style scoped>
canvas {
  width: 500px;
}
</style>
