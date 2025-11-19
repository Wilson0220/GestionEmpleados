<template>
  <Transition name="modal-fade">
    <div class="modal-backdrop" @click="closeModal">
      <div class="modal-container" @click.stop>
        
        <header class="modal-header">
          <h3>Detalles del Empleado</h3>
          <button @click="closeModal" class="btn-close" title="Cerrar">
            &times;
          </button>
        </header>

        <main class="modal-body">
          <div v-if="loading" class="loading">Cargando...</div>
          
          <div v-if="error" class="error">
            {{ error }}
          </div>
          
          <div v-if="employee" class="employee-details">
            <div class="detail-item">
              <strong>Nombre Completo:</strong>
              <p>{{ employee.nombre }} {{ employee.apellido }}</p>
            </div>
            <div class="detail-item">
              <strong>Departamento:</strong>
              <p>{{ employee.departamento }}</p>
            </div>
             <div class="detail-item">
              <strong>Salario:</strong>
              <p>{{ formatCurrency(employee.salario) }}</p>
            </div>
            <div class="detail-item">
              <strong>Estado:</strong>
              <p>
                <span :class="['estado', employee.estadoEmpleado === 'Activo' ? 'activo' : 'inactivo']">
                  {{ employee.estadoEmpleado }}
                </span>
              </p>
            </div>
            <div class="detail-item">
              <strong>Fecha de Ingreso:</strong>
              <p>{{ formatDate(employee.fechaIngreso) }}</p>
            </div>
          </div>
        </main>

        <footer class="modal-footer">
          <button @click="closeModal" class="btn-secondary">Cerrar</button>
        </footer>

      </div>
    </div>
  </Transition>
</template>

<script setup lang="ts">
import { ref, onMounted, watch } from 'vue'
import api from '@/services/api'

const props = defineProps({
  employeeId: {
    type: [Number, String],
    required: true,
  },
})

const emit = defineEmits(['close'])

interface EmpleadoDetalle {
  id: number
  nombre: string
  apellido: string
  fechaIngreso: string
  salario: number
  departamentoId: number
  departamento: string
  estadoEmpleadoId: number
  estadoEmpleado: string
}

const employee = ref<EmpleadoDetalle | null>(null)
const loading = ref(true)
const error = ref<string | null>(null)

const fetchEmployeeDetails = async () => {
  if (!props.employeeId) return
  loading.value = true
  error.value = null
  employee.value = null

  try {
    const response = await api.get<EmpleadoDetalle>(`/empleado/${props.employeeId}`)
    employee.value = response.data

  } catch (e: any) {
    console.error(e)
    if (e.response) {
      error.value = `Error ${e.response.status}: No se pudo encontrar el empleado.`
    } else if (e.request) {
      error.value = 'No se pudo conectar al servidor. Revisa tu API.'
    } else {
      error.value = e.message
    }
  } finally {
    loading.value = false
  }
}

const closeModal = () => {
  emit('close')
}

const formatCurrency = (value: number) =>
  new Intl.NumberFormat('es-DO', { style: 'currency', currency: 'DOP' }).format(value)

const formatDate = (dateStr: string) => new Date(dateStr).toLocaleDateString('es-DO')


onMounted(fetchEmployeeDetails)
watch(() => props.employeeId, fetchEmployeeDetails)
</script>

<style scoped>
.modal-backdrop {
  position: fixed;
  top: 0;
  left: 0;
  width: 100vw;
  height: 100vh;
  background-color: rgba(0, 0, 0, 0.6);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

.modal-container {
  width: 500px;
  max-width: 90%;
  background-color: #ffffff;
  border-radius: 8px;
  box-shadow: 0 10px 30px rgba(0, 0, 0, 0.2);
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

.modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1rem 1.5rem;
  border-bottom: 1px solid #e0e0e0;
}

.modal-header h3 {
  margin: 0;
  font-size: 1.25rem;
  color: #333;
}

.btn-close {
  background: none;
  border: none;
  font-size: 2rem;
  line-height: 1;
  color: #888;
  cursor: pointer;
  padding: 0;
}
.btn-close:hover {
  color: #000;
}

.modal-body {
  padding: 1.5rem;
  max-height: 60vh;
  overflow-y: auto;
}

.loading, .error {
  text-align: center;
  padding: 2rem 0;
  font-size: 1.1rem;
}
.error {
  color: #d9534f;
}

.employee-details .detail-item {
  display: grid;
  grid-template-columns: 140px 1fr;
  gap: 8px;
  padding: 0.75rem 0;
  border-bottom: 1px solid #f0f0f0;
}

.employee-details .detail-item:last-child {
  border-bottom: none;
}

.employee-details strong {
  font-weight: 600;
  color: #555;
  text-align: right;
  padding-right: 1rem;
}

.employee-details p {
  margin: 0;
  color: #222;
  word-break: break-word;
}

.modal-footer {
  padding: 1rem 1.5rem;
  border-top: 1px solid #e0e0e0;
  background-color: #f9f9f9;
  text-align: right;
}

.btn-secondary {
  padding: 0.5rem 1rem;
  background-color: #6c757d;
  color: white;
  border: none;
  border-radius: 5px;
  cursor: pointer;
}
.btn-secondary:hover {
  background-color: #5a6268;
}

.modal-fade-enter-active,
.modal-fade-leave-active {
  transition: opacity 0.3s ease;
}

.modal-fade-enter-from,
.modal-fade-leave-to {
  opacity: 0;
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
</style>