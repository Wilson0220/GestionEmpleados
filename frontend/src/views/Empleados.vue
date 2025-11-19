<template>
  <div class="container">
    <h2>Lista de Empleados</h2>

    <div class="filters">
      <input
        type="text"
        placeholder="Buscar por nombre o apellido..."
        v-model="search"
      />
      <select v-model="filtroDepartamento">
        <option value="">Todos los departamentos</option>
        <option v-for="dep in departamentos" :key="dep" :value="dep">{{ dep }}</option>
      </select>
      <select v-model="filtroEstado">
        <option value="">Todos los estados</option>
        <option v-for="estado in estados" :key="estado" :value="estado">{{ estado }}</option>
      </select>
    </div>

    <table class="empleados-table">
      <thead>
        <tr>
          <th>Nombres</th>
          <th>Apellidos</th>
          <th>Departamento</th>
          <th>Estado</th>
          <th>Salario</th>
          <th>Fecha Ingreso</th>
          <th>Acciones</th>
        </tr>
      </thead>
      <tbody>
        <tr
          v-for="empleado in empleadosFiltrados"
          :key="empleado.id"
        >
          <td>{{ empleado.nombre }}</td>
          <td>{{ empleado.apellido }}</td>
          <td>{{ empleado.departamento }}</td>
          <td>{{ empleado.estadoEmpleado }}</td>
          <td>{{ formatCurrency(empleado.salario) }}</td>
          <td>{{ formatDate(empleado.fechaIngreso) }}</td>
          <td>
            <button @click="editarEmpleado(empleado)" class="btn-edit">Editar</button>
            <button @click="eliminarEmpleado(empleado.id)" class="btn-delete">Eliminar</button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';
import api from '@/services/api.ts';

const empleados = ref([]);
const search = ref('');
const filtroDepartamento = ref('');
const filtroEstado = ref('');

const departamentos = ref([]);
const estados = ref(['Activo', 'Inactivo']);

onMounted(async () => {
  try {
    const response = await api.get('/empleado');
    empleados.value = response.data;

    departamentos.value = [...new Set(response.data.map(e => e.departamento))];
  } catch (error) {
    console.error('Error al obtener empleados:', error);
  }
});

const empleadosFiltrados = computed(() => {
  return empleados.value.filter(emp => {
    const textoMatch = emp.nombre.toLowerCase().includes(search.value.toLowerCase()) ||
                       emp.apellido.toLowerCase().includes(search.value.toLowerCase());
    const depMatch = filtroDepartamento.value ? emp.departamento === filtroDepartamento.value : true;
    const estadoMatch = filtroEstado.value ? emp.estadoEmpleado === filtroEstado.value : true;
    return textoMatch && depMatch && estadoMatch;
  });
});

const formatCurrency = value =>
  new Intl.NumberFormat('es-DO', { style: 'currency', currency: 'DOP' }).format(value);

const formatDate = dateStr => new Date(dateStr).toLocaleDateString('es-DO');

const editarEmpleado = empleado => {
  alert(`Editar empleado: ${empleado.nombre} ${empleado.apellido}`);
};

const eliminarEmpleado = id => {
  if (confirm('¿Seguro que desea eliminar este empleado?')) {
    empleados.value = empleados.value.filter(e => e.id !== id);
  }
};
</script>

<style scoped>
.container {
  max-width: 1000px;
  margin: 0 auto;
  padding: 1rem;
}

h2 {
  margin-bottom: 1rem;
  color: #1e3a8a;
}

.filters {
  display: flex;
  gap: 0.5rem;
  margin-bottom: 1rem;
}

.filters input,
.filters select {
  padding: 0.5rem;
  border-radius: 5px;
  border: 1px solid #ccc;
}

.empleados-table {
  width: 100%;
  border-collapse: collapse;
  background: white;
  border-radius: 8px;
  overflow: hidden;
  box-shadow: 0 2px 6px rgba(0,0,0,0.1);
}

.empleados-table th {
  background-color: #3b82f6;
  color: white;
  text-align: left;
  padding: 0.75rem;
}

.empleados-table td {
  padding: 0.75rem;
  border-bottom: 1px solid #e5e7eb;
}

.empleados-table tr:hover {
  background-color: #f0f9ff;
}

.btn-edit {
  background-color: #f59e0b;
  color: white;
  border: none;
  padding: 0.4rem 0.6rem;
  border-radius: 4px;
  cursor: pointer;
  margin-right: 0.3rem;
}

.btn-delete {
  background-color: #ef4444;
  color: white;
  border: none;
  padding: 0.4rem 0.6rem;
  border-radius: 4px;
  cursor: pointer;
}
.btn-edit:hover { background-color: #d97706; }
.btn-delete:hover { background-color: #b91c1c; }
</style>
