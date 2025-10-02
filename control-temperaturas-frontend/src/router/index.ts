import { createRouter, createWebHistory } from 'vue-router'
import Formulario from '../views/Formulario.vue'
import Reporte from '../views/Reporte.vue'

const routes = [
  { path: '/', redirect: '/formulario' },
  { path: '/formulario', component: Formulario },
  { path: '/reporte', component: Reporte }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
