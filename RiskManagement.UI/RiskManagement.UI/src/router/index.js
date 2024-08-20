import { createRouter, createWebHistory } from 'vue-router'
import PanelLayout from '@/layout/PanelLayout.vue'
import DefaultLayout from '@/layout/default.vue'
const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'landing',
      component:() => import('@/views/landing/Index.vue'),
      meta: {
        layout: DefaultLayout
      }
    },
    {
      path: '/projects',
      name: 'projects',
      component:() => import('@/views/projects/Index.vue'),
      meta: {
        layout: DefaultLayout
      }
    },

    {
      path: '/risks/:id',
      name: 'risks',
      component:() => import('@/views/risk/Index.vue'),
      meta: {
        layout: PanelLayout
      },
    },

    {
      path: '/risks/solution/:id/:riskId',
      name: 'solutions',
      component:() => import('@/views/solution/Index.vue'),
      meta: {
        layout: PanelLayout
      }
    },

    {
      path: '/report/:id',
      name: 'reports',
      component:() => import('@/views/report/Index.vue'),
      meta: {
        layout: PanelLayout
      }
    },

    {
      path: '/status/:id',
      name: 'status',
      component:() => import('@/views/status/Index.vue'),
      meta: {
        layout: PanelLayout
      }
    },
    {
      path: '/setting/:id',
      name: 'setting',
      component:() => import('@/views/setting/Index.vue'),
      meta: {
        layout: PanelLayout
      }
    }

  ]
})

export default router
