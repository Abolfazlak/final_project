import { createRouter, createWebHistory } from 'vue-router'
import PanelLayout from '@/layout/PanelLayout.vue'
import DefaultLayout from '@/layout/default.vue'
const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component:() => import('@/views/panel/Index.vue'),
      meta: {
        layout: PanelLayout
      }
    },
    {
      path: '/login',
      name: 'login',
      component:() => import('@/views/landing/Index.vue'),
      meta: {
        layout: DefaultLayout
      }
    }
  ]
})

export default router
