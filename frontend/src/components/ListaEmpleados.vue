<template>
  <div class="empleados-container">
    <header class="empleados-header">
      <h2>👥 Ver Empleados</h2>
      <router-link to="/empleados/nuevo" class="btn-nuevo">
        <i class="fas fa-plus"></i> Nuevo Empleado
      </router-link>
    </header>

    <div class="filtros">
      <div class="buscador">
        <i class="fas fa-search"></i>
        <input
          type="text"
          placeholder="Buscar por nombre o apellido..."
          v-model="search"
        />
      </div>
      <select v-model="filtroDepartamento">
        <option value="">Todos los departamentos</option>
        <option v-for="dep in departamentos" :key="dep" :value="dep">{{ dep }}</option>
      </select>
      <select v-model="filtroEstado">
        <option value="">Todos los estados</option>
        <option v-for="estado in estados" :key="estado" :value="estado">{{ estado }}</option>
      </select>
    </div>

    <div class="tabla-container">
      <div v-if="cargando" class="loader-global">
        <div class="spinner-global"></div>
      </div>

      <table class="empleados-table" v-else>
        <thead>
          <tr>
            <th>Nombres</th>
            <th>Apellidos</th>
            <th>Departamento</th>
            <th>Estado</th>
            <th>Salario</th>
            <th>Fecha Ingreso</th>
            <th class="acciones-header">Acciones</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="empleado in paginatedEmpleados" :key="empleado.id" class="fila-empleado">
            <td>{{ empleado.nombre }}</td>
            <td>{{ empleado.apellido }}</td>
            <td>{{ empleado.departamento }}</td>
            <td>
              <span
                :class="['estado', empleado.estadoEmpleado === 'Activo' ? 'activo' : 'inactivo']"
              >
                {{ empleado.estadoEmpleado }}
              </span>
            </td>
            <td>{{ formatCurrency(empleado.salario) }}</td>
            <td>{{ formatDate(empleado.fechaIngreso) }}</td>
            <td class="acciones">
              <button 
                @click="openDetailsModal(empleado.id)"
                class="btn-accion ver" 
                title="Ver detalles"
              >
                <i class="fas fa-eye"></i>
              </button>

              <router-link 
                :to="`/empleados/${empleado.id}/editar`" 
                class="btn-accion editar" 
                title="Editar empleado"
              >
                <i class="fas fa-pen"></i>
              </router-link>

              <button 
                @click="eliminarEmpleado(empleado.id)" 
                class="btn-accion eliminar"
                title="Eliminar empleado"
                :disabled="eliminandoId === empleado.id"
              >
                <span v-if="eliminandoId === empleado.id" class="spinner"></span>
                <i v-else class="fas fa-trash"></i>
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <div class="paginacion" v-if="totalPaginas > 1 && !cargando">
      <button 
        :disabled="paginaActual === 1" 
        @click="paginaActual--"
      >
        ⬅
      </button>
      <span>Página {{ paginaActual }} de {{ totalPaginas }}</span>
      <button 
        :disabled="paginaActual === totalPaginas" 
        @click="paginaActual++"
      >
        ➡
      </button>
    </div>

     <DetallesEmpleados
      v-if="isModalVisible"
      :employee-id="selectedEmployeeId!"
      @close="closeDetailsModal"
    /> 
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, getCurrentInstance } from 'vue'
import api from '@/services/api'
import DetallesEmpleados from './DetallesEmpleados.vue'

const isModalVisible = ref(false)
const selectedEmployeeId = ref<number | null>(null)

const openDetailsModal = (id: number) => {
  selectedEmployeeId.value = id
  isModalVisible.value = true
}

const closeDetailsModal = () => {
  isModalVisible.value = false
  selectedEmployeeId.value = null
}

interface Empleado {
  id: number
  nombre: string
  apellido: string
  departamento: string
  estadoEmpleado: string
  salario: number
  fechaIngreso: string
}

const empleados = ref<Empleado[]>([])
const search = ref('')
const filtroDepartamento = ref('')
const filtroEstado = ref('')
const departamentos = ref<string[]>([])
const estados = ref(['Activo', 'Inactivo'])

const paginaActual = ref(1)
const itemsPorPagina = 5

const cargando = ref(true)

const eliminandoId = ref<number | null>(null)

const instance = getCurrentInstance()
const $alert = instance?.proxy?.$alert

const cargarEmpleados = async () => {
  try {
    cargando.value = true
    const response = await api.get<Empleado[]>('/empleado')
    empleados.value = response.data
    departamentos.value = [...new Set(response.data.map((e) => e.departamento))]
  } catch (error) {
    console.error(error)
    $alert?.error('Error', 'No se pudieron cargar los empleados')
  } finally {
    cargando.value = false
  }
}

onMounted(cargarEmpleados)

const empleadosFiltrados = computed(() =>
  empleados.value.filter((emp) => {
    const textoMatch =
      emp.nombre.toLowerCase().includes(search.value.toLowerCase()) ||
      emp.apellido.toLowerCase().includes(search.value.toLowerCase())
    const depMatch = filtroDepartamento.value
      ? emp.departamento === filtroDepartamento.value
      : true
    const estadoMatch = filtroEstado.value
      ? emp.estadoEmpleado === filtroEstado.value
      : true
    return textoMatch && depMatch && estadoMatch
  })
)

const totalPaginas = computed(() => Math.ceil(empleadosFiltrados.value.length / itemsPorPagina))

const paginatedEmpleados = computed(() => {
  const start = (paginaActual.value - 1) * itemsPorPagina
  return empleadosFiltrados.value.slice(start, start + itemsPorPagina)
})

const formatCurrency = (value: number) =>
  new Intl.NumberFormat('es-DO', { style: 'currency', currency: 'DOP' }).format(value)

const formatDate = (dateStr: string) => new Date(dateStr).toLocaleDateString('es-DO')

const eliminarEmpleado = async (id: number) => {
  const confirmado = await $alert?.confirmar(
    '¿Eliminar empleado?',
    'Esta acción no se puede deshacer.'
  )

  if (!confirmado) return

  try {
    eliminandoId.value = id
    await api.delete(`/empleado/${id}`)
    empleados.value = empleados.value.filter(e => e.id !== id)
    $alert?.toast('Empleado eliminado correctamente', 'success')
  } catch (error) {
    console.error(error)
    $alert?.error('Error', 'No se pudo eliminar el empleado.')
  } finally {
    eliminandoId.value = null
  }
}
</script>

<style scoped>
.empleados-container {
  background: #fff;
  padding: 2rem;
  border-radius: 1rem;
  box-shadow: 0 8px 30px rgba(0, 0, 0, 0.08);
  margin: 2rem;
  transition: all 0.3s;
}

.empleados-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1.5rem;
}
h2 {
  font-size: 1.8rem;
  font-weight: 700;
  color: #1e293b;
}

.btn-nuevo {
  background: #2563eb;
  color: white;
  padding: 0.6rem 1.2rem;
  border-radius: 0.5rem;
  font-weight: 600;
  text-decoration: none;
  transition: all 0.3s;
}
.btn-nuevo:hover {
  background: #1d4ed8;
  transform: translateY(-2px);
}

.filtros {
  display: flex;
  align-items: center;
  gap: 0.8rem;
  margin-bottom: 1rem;
  flex-wrap: wrap;
}
.buscador {
  display: flex;
  align-items: center;
  background: #f1f5f9;
  padding: 0.5rem 1rem;
  border-radius: 0.5rem;
  flex: 1;
  min-width: 240px;
  transition: all 0.2s;
}
.buscador input {
  border: none;
  outline: none;
  background: transparent;
  flex: 1;
}
select {
  padding: 0.5rem 0.8rem;
  border-radius: 0.5rem;
  border: 1px solid #cbd5e1;
  background: #f8fafc;
  font-size: 0.9rem;
}

.tabla-container {
  overflow-x: auto;
  margin-top: 1rem;
  position: relative;
}
.empleados-table {
  width: 100%;
  border-collapse: collapse;
  font-size: 0.95rem;
  transition: all 0.3s;
}
.empleados-table th {
  background: #f1f5f9;
  text-align: left;
  padding: 0.8rem;
  font-weight: 600;
  color: #334155;
  position: sticky;
  top: 0;
  z-index: 1;
}
.empleados-table td {
  padding: 0.8rem;
  border-bottom: 1px solid #e2e8f0;
}
.empleados-table tr.fila-empleado:hover {
  background: #e0f2fe;
  transform: translateX(2px);
  transition: all 0.2s;
}

.estado {
  padding: 0.3rem 0.7rem;
  border-radius: 0.5rem;
  font-weight: 600;
  font-size: 0.85rem;
  text-align: center;
  transition: all 0.3s;
}
.estado.activo {
  background: #dcfce7;
  color: #15803d;
}
.estado.inactivo {
  background: #fee2e2;
  color: #b91c1c;
}

.acciones {
  display: flex;
  gap: 0.5rem;
}
.btn-accion {
  border: none;
  background: none;
  cursor: pointer;
  padding: 0.4rem 0.6rem;
  border-radius: 0.4rem;
  transition: all 0.2s;
  display: flex;
  align-items: center;
  justify-content: center;
}
.btn-accion i { font-size: 0.95rem; }
.btn-accion.ver { color: #10b981; }
.btn-accion.ver:hover { background: #d1fae5; }
.btn-accion.editar { color: #2563eb; }
.btn-accion.editar:hover { background: #bfdbfe; }
.btn-accion.eliminar { color: #dc2626; }
.btn-accion.eliminar:hover { background: #fee2e2; }

.spinner {
  border: 2px solid #f3f3f3; 
  border-top: 2px solid #dc2626; 
  border-radius: 50%;
  width: 16px;
  height: 16px;
  animation: spin 1s linear infinite;
  display: inline-block;
}
@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}

.loader-global {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 200px;
}
.spinner-global {
  border: 4px solid #f3f3f3;
  border-top: 4px solid #2563eb;
  border-radius: 50%;
  width: 40px;
  height: 40px;
  animation: spin 1s linear infinite;
}

.paginacion {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 1rem;
  margin-top: 1rem;
  transition: all 0.3s;
}
.paginacion button {
  padding: 0.4rem 0.8rem;
  border-radius: 0.4rem;
  border: 1px solid #cbd5e1;
  background: #f8fafc;
  cursor: pointer;
  transition: all 0.2s;
}
.paginacion button:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}
.paginacion button:hover:not(:disabled) {
  background: #e0f2fe;
}
</style>
