<template>
  <div class="charts">
    <div class="chart-card">
      <h3>Empleados por Departamento</h3>

      <div v-if="loading" class="loader">
        Cargando datos...
      </div>

      <div v-else-if="!barData.labels.length" class="no-data">
        No hay datos disponibles
      </div>

      <BarChart
        v-else
        :data="barData"
        :options="barOptions"
      />
    </div>
  </div>
</template>

<script setup>
import { reactive, ref, onMounted } from 'vue'
import {
  Chart as ChartJS,
  Title,
  Tooltip,
  Legend,
  BarElement,
  CategoryScale,
  LinearScale
} from 'chart.js'
import { Bar } from 'vue-chartjs'
import api from '@/services/api.ts';

ChartJS.register(Title, Tooltip, Legend, BarElement, CategoryScale, LinearScale)

const loading = ref(true)

const barData = reactive({
  labels: [],
  datasets: [
    {
      label: 'Cantidad de Empleados',
      data: [],
      backgroundColor: '#3b82f6',
      borderRadius: 6
    }
  ]
})

const barOptions = reactive({
  responsive: true,
  plugins: { legend: { display: false }, tooltip: { mode: 'index', intersect: false } },
  scales: { y: { beginAtZero: true, grid: { drawBorder: false } }, x: { grid: { display: false } } }
})

const BarChart = Bar

const loadDashboardData = async () => {
  loading.value = true
  try {
    const empleadosRes = await api.get('/Empleado/ObtenerEmpleadosPorDepartamento')
    if (empleadosRes.data && empleadosRes.data.length) {
      barData.labels = empleadosRes.data.map(d => d.departamento)
      barData.datasets[0].data = empleadosRes.data.map(d => d.cantidad)
    }
  } catch (error) {
    console.error('Error cargando datos del dashboard:', error)
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  loadDashboardData()
})
</script>

<style scoped>
.charts {
  display: flex;
  justify-content: center;
  padding: 1rem 2rem;
}

.chart-card {
  width: 100%;
  max-width: 700px;
  background-color: white;
  border-radius: 12px;
  padding: 1rem;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
}

.loader,
.no-data {
  text-align: center;
  padding: 2rem;
  color: #555;
  font-weight: bold;
  font-size: 1.1rem;
}
</style>
