<template>
  <div class="editar-empleado-container">
    <h2 class="titulo">✏️ Editar Empleado</h2>
    <form @submit.prevent="actualizarEmpleado" class="formulario">
      
      <div class="campo">
        <label>Nombre:</label>
        <input v-model="empleado.nombre" required placeholder="Nombre del empleado"/>
      </div>

      <div class="campo">
        <label>Apellido:</label>
        <input v-model="empleado.apellido" required placeholder="Apellido del empleado"/>
      </div>

      <div class="campo">
        <label>Departamento:</label>
        <select v-model.number="empleado.departamentoId" required>
          <option v-for="dep in departamentos" :key="dep.id" :value="dep.id">
            {{ dep.nombre }}
          </option>
        </select>
      </div>

      <div class="campo">
        <label>Estado:</label>
        <select v-model.number="empleado.estadoEmpleadoId" required>
          <option v-for="estado in estados" :key="estado.id" :value="estado.id">
            {{ estado.nombre }}
          </option>
        </select>
      </div>

      <div class="campo">
        <label>Salario:</label>
        <input type="number" v-model.number="empleado.salario" required placeholder="0.00"/>
      </div>

      <button type="submit" :disabled="guardando" class="btn-guardar">
        <span v-if="guardando" class="spinner"></span>
        Guardar Cambios
      </button>
    </form>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, getCurrentInstance } from 'vue'
import api from '@/services/api'
import { useRoute, useRouter } from 'vue-router'

const instance = getCurrentInstance()
const $alert = instance?.proxy?.$alert

interface Empleado {
  id: number
  nombre: string
  apellido: string
  departamentoId: number
  estadoEmpleadoId: number
  salario: number
}

interface Departamento {
  id: number
  nombre: string
}

interface Estado {
  id: number
  nombre: string
}

const route = useRoute()
const router = useRouter()
const id = Number(route.params.id)

const empleado = ref<Empleado>({
  id: 0,
  nombre: '',
  apellido: '',
  departamentoId: 0,
  estadoEmpleadoId: 1,
  salario: 0
})

const guardando = ref(false)
const departamentos = ref<Departamento[]>([])
const estados = ref<Estado[]>([
  { id: 1, nombre: 'Activo' },
  { id: 2, nombre: 'Inactivo' }
])

const cargarEmpleado = async () => {
  try {
    const { data } = await api.get<Empleado>(`/empleado/${id}`)
    empleado.value.nombre = data.nombre
    empleado.value.apellido = data.apellido
    empleado.value.salario = data.salario
    empleado.value.departamentoId = data.departamentoId
    empleado.value.estadoEmpleadoId = data.estadoEmpleadoId
  } catch (error) {
    console.error(error)
    alert('No se pudo cargar el empleado.')
  }
}

const cargarDepartamentos = async () => {
  try {
    const { data } = await api.get<Departamento[]>('/Departamento')
    departamentos.value = data
  } catch (error) {
    console.error(error)
    alert('No se pudieron cargar los departamentos.')
  }
}

onMounted(async () => {
  await cargarDepartamentos()
  await cargarEmpleado()
})

const actualizarEmpleado = async () => {
  try {
    guardando.value = true
    const payload = {
      id: id,
      nombre: empleado.value.nombre,
      apellido: empleado.value.apellido,
      salario: empleado.value.salario,
      departamentoId: Number(empleado.value.departamentoId),
      estadoEmpleadoId: Number(empleado.value.estadoEmpleadoId)
    }
    await api.put(`empleado/${id}`, payload)
    $alert?.exito('Empleado actualizado correctamente.', 'success')
    router.push('/empleados/lista')
  } catch (error) {
    console.error(error)
    alert('Error al actualizar empleado')
  } finally {
    guardando.value = false
  }
}
</script>

<style scoped>
.editar-empleado-container {
  max-width: 500px;
  margin: 40px auto;
  padding: 30px;
  background: #ffffff;
  border-radius: 12px;
  box-shadow: 0 6px 18px rgba(0,0,0,0.1);
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
}

.titulo {
  text-align: center;
  margin-bottom: 25px;
  color: #2c3e50;
  font-weight: 600;
}

.formulario .campo {
  display: flex;
  flex-direction: column;
  margin-bottom: 18px;
}

.formulario label {
  font-weight: 500;
  margin-bottom: 6px;
  color: #34495e;
}

.formulario input,
.formulario select {
  padding: 10px 12px;
  border-radius: 8px;
  border: 1px solid #ccc;
  font-size: 14px;
  transition: border-color 0.3s;
}

.formulario input:focus,
.formulario select:focus {
  border-color: #2980b9;
  outline: none;
}

.btn-guardar {
  width: 100%;
  padding: 12px;
  background: #2980b9;
  color: white;
  font-weight: 600;
  border: none;
  border-radius: 10px;
  cursor: pointer;
  transition: background 0.3s, transform 0.2s;
}

.btn-guardar:hover {
  background: #1f618d;
  transform: translateY(-2px);
}

.btn-guardar:disabled {
  background: #7fb3d5;
  cursor: not-allowed;
}

.spinner {
  border: 3px solid #f3f3f3;
  border-top: 3px solid #ffffff;
  border-radius: 50%;
  width: 14px;
  height: 14px;
  display: inline-block;
  animation: spin 1s linear infinite;
  margin-right: 8px;
  vertical-align: middle;
}

@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}
</style>
