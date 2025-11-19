import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router'
import MainLayout from '@/layouts/MainLayout.vue'
import Home from '@/views/Home.vue'
import ListaEmpleados from '@/components/ListaEmpleados.vue'
import CrearEmpleado from '@/components/CrearEmpleado.vue'
import EditarEmpleado from '@/components/EditarEmpleado.vue'

const routes: RouteRecordRaw[] = [
  {
    path: '/',
    component: MainLayout,
    children: [
      {
        path: '',
        name: 'Home',
        component: Home,
        meta: { title: 'Inicio' }
      },
      {
        path: 'empleados/lista',
        name: 'ListaEmpleados',
        component: ListaEmpleados,
        meta: { title: 'Empleados' }
      },
      {
        path: 'empleados/nuevo',
        name: 'CrearEmpleado',
        component: CrearEmpleado,
        meta: { title: 'Nuevo Empleado' }
      },
      {
        path: 'empleados/:id/editar',
        name: 'EditarEmpleado',
        component: EditarEmpleado,
        props: true,
        meta: { title: 'Editar Empleado' }
      },
    ]
  },
  {
    path: '/:pathMatch(.*)*',
    redirect: '/'
  }
]
const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes
})

import type { NavigationGuardNext, RouteLocationNormalized } from 'vue-router'

router.beforeEach(
  (to: RouteLocationNormalized, from: RouteLocationNormalized, next: NavigationGuardNext) => {
    document.title = to.meta.title
      ? `${to.meta.title} | Gestión Empleados`
      : 'Gestión Empleados'
    next()
  }
)

export default router
