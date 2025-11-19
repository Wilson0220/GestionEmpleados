<template>
  <div class="nuevo-empleado-container">
    <div class="form-card animate-fade-in">
      <h2 class="titulo-form">🧑‍💼 Registro de Nuevo Empleado</h2>

      <Form @submit="guardarEmpleado" :validation-schema="schema" class="formulario">
        <div class="campo">
          <label class="etiqueta">Nombre</label>
          <Field
            name="nombre"
            type="text"
            class="input"
            placeholder="Ej. Juan Carlos"
          />
          <ErrorMessage name="nombre" class="error" />
        </div>

        <div class="campo">
          <label class="etiqueta">Apellido</label>
          <Field
            name="apellido"
            type="text"
            class="input"
            placeholder="Ej. Pérez Gómez"
          />
          <ErrorMessage name="apellido" class="error" />
        </div>

        <div class="campo">
          <label class="etiqueta">Departamento</label>
          <Field name="departamentoId" as="select" class="input">
            <option value="">Seleccione un departamento</option>
            <option v-for="dept in departamentos" :key="dept.id" :value="dept.id">
              {{ dept.nombre }}
            </option>
          </Field>
          <ErrorMessage name="departamentoId" class="error" />
        </div>

        <Field type="hidden" name="estadoEmpleadoId" :value="1" />

        <div class="campo">
          <label class="etiqueta">Salario</label>
          <Field
            name="salario"
            type="number"
            step="0.01"
            class="input"
            placeholder="Ej. 30000.00"
          />
          <ErrorMessage name="salario" class="error" />
        </div>

        <div class="campo">
          <label class="etiqueta">Fecha de Ingreso</label>
          <Field name="fechaIngreso" type="date" class="input" />
          <ErrorMessage name="fechaIngreso" class="error" />
        </div>

        <div class="acciones">
          <button type="submit" class="btn-guardar">
            💾 Guardar Empleado
          </button>
        </div>
      </Form>
    </div>
  </div>
</template>

<script setup>
import { Form, Field, ErrorMessage } from 'vee-validate'
import * as yup from 'yup'
import { useRouter } from 'vue-router'
import api from '@/services/api.ts';
import { ref, onMounted, getCurrentInstance } from 'vue'

const router = useRouter()
const departamentos = ref([])

const instance = getCurrentInstance()
const $alert = instance?.proxy?.$alert

onMounted(async () => {
  try {
    const response = await api.get('/Departamento')
    departamentos.value = response.data
  } catch (error) {
    console.error('Error al cargar departamentos:', error)
  }
})

const schema = yup.object({
  nombre: yup.string().required('Nombre obligatorio'),
  apellido: yup.string().required('Apellido obligatorio'),
  departamentoId: yup
    .number()
    .typeError('Seleccione un departamento')
    .required('Departamento obligatorio'),
  estadoEmpleadoId: yup.number().required(),
  salario: yup
    .number()
    .required('Salario obligatorio')
    .positive('Debe ser mayor que 0'),
  fechaIngreso: yup.date().required('Fecha de ingreso obligatoria')
})

const guardarEmpleado = async (values) => {
  values.estadoEmpleadoId = 1
  values.departamentoId = Number(values.departamentoId)

  try {
    await api.post('/Empleado', values)
    $alert?.exito('Empleado creado exitosamente.', 'success')
    router.push('/empleados/lista')
  } catch (error) {
    console.error('Error al guardar empleado:', error)
    alert('❌ Error al guardar empleado')
  }
}
</script>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Poppins:wght@400;500;600&display=swap');

.nuevo-empleado-container {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: calc(100vh - 120px);
  background: linear-gradient(135deg, #f8f8f2, #f8f8f2);
  font-family: 'Poppins', sans-serif;
  color: #f8f8f2;
  padding: 2rem;
}

.form-card {
  background: #ffffff;
  color: #2c3e50;
  border-radius: 12px;
  padding: 2rem 2.5rem;
  width: 100%;
  max-width: 550px;
  box-shadow: 0 6px 18px rgba(0,0,0,0.1);
  transition: all 0.4s ease;
}

.form-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 20px 40px rgba(0, 0, 0, 0.15);
}

.titulo-form {
  text-align: center;
  font-size: 1.8rem;
  font-weight: 600;
  margin-bottom: 1.5rem;
  color: #2980b9;
}

.formulario {
  display: flex;
  flex-direction: column;
  gap: 1.2rem;
}

.campo {
  display: flex;
  flex-direction: column;
}

.etiqueta {
  font-weight: 500;
  margin-bottom: 0.3rem;
}

.input {
  padding: 10px 12px;
  border-radius: 8px;
  border: 1px solid #ccc;
  font-size: 14px;
  outline: none;
  transition: border-color 0.3s, box-shadow 0.3s;
}

.input:focus {
  border-color: #2980b9;
  box-shadow: 0 0 8px rgba(41, 128, 185, 0.5);
}

.error {
  color: #e74c3c;
  font-size: 0.85rem;
  margin-top: 0.2rem;
}

.acciones {
  text-align: center;
  margin-top: 1.5rem;
}

.btn-guardar {
  background: #2980b9;
  color: white;
  font-weight: 600;
  padding: 0.8rem 2.5rem;
  border-radius: 10px;
  border: none;
  cursor: pointer;
  font-size: 1rem;
  transition: all 0.3s ease;
  box-shadow: 0 4px 12px rgba(41, 128, 185, 0.3);
}

.btn-guardar:hover {
  background: #1f618d;
  transform: translateY(-2px);
  box-shadow: 0 8px 20px rgba(41, 128, 185, 0.45);
}

@keyframes fade-in {
  from { opacity: 0; transform: translateY(15px); }
  to { opacity: 1; transform: translateY(0); }
}

.animate-fade-in {
  animation: fade-in 0.7s ease forwards;
}
</style>
