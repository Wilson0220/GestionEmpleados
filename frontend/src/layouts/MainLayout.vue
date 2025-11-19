<template>
  <div :class="['layout', darkMode ? 'dark' : '']">
    <aside :class="['sidebar', { collapsed: isCollapsed }]">
      <div class="sidebar-header">
        <div class="logo">Empleados</div>
        <button class="collapse-btn" @click="toggleSidebar">
          <span v-if="isCollapsed">➡</span>
          <span v-else>⬅</span>
        </button>
      </div>

      <nav class="sidebar-nav">
        <router-link to="/" class="nav-link" active-class="active-link">
          <i class="icon">🏠</i>
          <span v-if="!isCollapsed">Home</span>
        </router-link>

        <div class="nav-item">
          <div class="nav-link" @click="toggleSubmenu('empleados')">
            <i class="icon">👥</i>
            <span v-if="!isCollapsed">Empleados</span>
            <span v-if="!isCollapsed" class="arrow">
              {{ openSubmenu === 'empleados' ? '▲' : '▼' }}
            </span>
          </div>
          <transition name="submenu">
            <div v-show="openSubmenu === 'empleados'" class="submenu">
              <router-link to="/empleados/lista" class="nav-link">Lista</router-link>
              <router-link to="/empleados/nuevo" class="nav-link">Nuevo</router-link>
            </div>
          </transition>
        </div>

        <router-link to="/departamentos" class="nav-link" active-class="active-link">
          <i class="icon">🏢</i>
          <span v-if="!isCollapsed">Departamentos</span>
        </router-link>

        <router-link to="/reportes" class="nav-link" active-class="active-link">
          <i class="icon">📊</i>
          <span v-if="!isCollapsed">Reportes</span>
        </router-link>
      </nav>
    </aside>

    <div class="main">
      <header class="header">
        <div class="header-left">
          <h1>Sistema de Empleados</h1>
        </div>
        <div class="header-right">
          <input type="text" placeholder="🔍 Buscar..." class="search" />
          <button @click="darkMode = !darkMode" class="mode-btn">
            {{ darkMode ? '☀️' : '🌙' }}
          </button>
          <div class="user">👤 Admin</div>
        </div>
      </header>

<main class="content">
  <div v-if="$route.path === '/'">
    <DashboardCharts />
    <div class="cards">
      <div class="card" v-for="(item, index) in cardData" :key="index">
        <h3>{{ item.title }}</h3>
        <p>{{ item.value }}</p>
      </div>
    </div>
  </div>

  <div v-else class="router-content">
    <router-view />
  </div>
</main>

      <footer class="footer">
        &copy; 2025 Mis Empleados
      </footer>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue';
import DashboardCharts from '@/components/DashboardCharts.vue';
import api from '@/services/api.ts';

const isCollapsed = ref(false);
const darkMode = ref(false);
const openSubmenu = ref(null);

const toggleSidebar = () => (isCollapsed.value = !isCollapsed.value);
const toggleSubmenu = (menu) => {
  openSubmenu.value = openSubmenu.value === menu ? null : menu;
};

const resumen = ref({
  totalEmpleados: 0,
  totalDepartamentos: 0,
  totalEmpleadosActivos: 0
});

const cargarDatosDashboard = async () => {
  try {
    const response = await api.get('/Helper/resumen');
    resumen.value = response.data;
  } catch (error) {
    console.error('Error cargando datos del dashboard:', error);
  }
};

onMounted(() => {
  cargarDatosDashboard();
});

const cardData = computed(() => [
  { title: 'Total Departamentos', value: resumen.value.totalDepartamentos },
  { title: 'Total Empleados', value: resumen.value.totalEmpleados },
  { title: 'Empleados Activos', value: resumen.value.totalEmpleadosActivos }
]);
</script>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Poppins:wght@400;500;600&display=swap');

:root {
  --primary: #8be9fd;
  --primary-dark: #50fa7b;
  --accent: #bd93f9;
  --bg-main: #0b1d51;
  --text-light: #f8f8f2;
  --card-bg: rgba(255,255,255,0.05);
  --card-bg-dark: rgba(15,25,40,0.85);
  --border-color: #1a2a65;
}

.layout {
  display: flex;
  min-height: 100vh;
  font-family: 'Poppins', sans-serif;
  background-color: var(--bg-main);
  color: var(--text-light);
}

.sidebar {
  width: 220px;
  background: var(--bg-main);
  color: var(--text-light);
  display:flex; flex-direction:column;
  border-right: 2px solid var(--border-color);
  transition: all 0.4s ease;
}
.sidebar.collapsed { width: 70px; }

.sidebar-header {
  display:flex; justify-content:space-between; align-items:center;
  padding:1rem; font-weight:bold; font-size:1.5rem;
  border-bottom:2px solid var(--border-color);
}
.collapse-btn {
  background:transparent; border:none; color:white;
  cursor:pointer; font-size:1.3rem; transition: transform 0.3s;
}
.collapse-btn:hover { transform: scale(1.2); }

.sidebar-nav { display:flex; flex-direction:column; margin-top:1rem; flex:1; }
.nav-link {
  padding:0.8rem 1rem; display:flex; align-items:center; gap:0.8rem;
  border-radius:12px; margin:0.3rem 0; transition: all 0.3s;
  color:var(--text-light);
  text-decoration:none;
}
.nav-link:hover {
  background: rgba(255,255,255,0.15);
  box-shadow:0 0 10px var(--accent);
}
.active-link {
  background: rgba(255,255,255,0.2);
  box-shadow:0 0 12px var(--accent);
}
.submenu {
  display:flex; flex-direction:column;
  margin-left:1rem;
  overflow:hidden;
}
.submenu-enter-active, .submenu-leave-active { transition: all 0.3s ease; }
.submenu-enter-from, .submenu-leave-to { max-height:0; opacity:0; }
.submenu-enter-to, .submenu-leave-from { max-height:200px; opacity:1; }

.main {
  flex:1;
  display:flex;
  flex-direction:column;
  background-color: var(--bg-main);
}

.header {
  display:flex; justify-content:space-between; align-items:center;
  padding:0.1rem 1rem;
  background: var(--bg-main);
  border-bottom:2px solid var(--border-color);
  box-shadow:0 4px 10px rgba(0,0,0,0.1);
  position:sticky; top:0; z-index:10;
}
.header-left h1 { font-size:1.4rem; color: var(--accent); font-weight:600; }

.header-right { display:flex; align-items:center; gap:1rem; }
.search {
  padding:0.5rem 0.8rem; border-radius:12px; border:none; outline:none;
  background: rgba(255,255,255,0.1); color: var(--text-light);
}
.mode-btn {
  background: var(--accent); border:none; border-radius:50%;
  width:42px; height:42px; display:flex; align-items:center; justify-content:center;
  cursor:pointer; font-size:1.3rem;
}
.mode-btn:hover { transform:scale(1.1); box-shadow:0 0 10px var(--accent); }

.user { font-weight:500; color: var(--accent); }

.content {
  flex:1;
  overflow-y:auto;
  padding-bottom:1rem;
}

.cards {
  display:grid; grid-template-columns:repeat(auto-fit,minmax(230px,1fr));
  gap:1.2rem; padding:1.2rem 2rem;
}
.card {
  background: var(--card-bg); padding:1.8rem; border-radius:16px;
  box-shadow:0 10px 25px rgba(0,0,0,0.08); text-align:center;
  transition: all 0.4s ease; backdrop-filter: blur(6px);
}
.card h3 { margin-bottom:0.6rem; color: var(--primary); font-weight:600; }
.card p { font-size:1.8rem; font-weight:bold; color: var(--primary-dark); }

.router-content {
  padding:1.5rem 2rem; background: var(--card-bg); border-radius:16px; margin:1rem 2rem;
  transition: all 0.4s ease; backdrop-filter: blur(6px);
}

.footer {
  text-align: center;
  padding: 1rem;
  background-color: var(--bg-main);
  color: var(--text-light);
  font-size: 0.95rem;
  border-top: 2px solid var(--border-color);
  box-shadow: 0 -2px 3px rgba(0,0,0,0.3);
}

@media (max-width:768px){
  .sidebar { position:fixed; left:-220px; top:0; height:100%; z-index:20; border-radius:0; }
  .sidebar.collapsed { left:0; }
  .main { margin-left:0; }
}
</style>
