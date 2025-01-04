import { createRouter, createWebHistory } from 'vue-router'
import UrlShortner from '../components/UrlShortner.vue'
import NotFound from '../components/NotFound.vue'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: '/',
      name: 'home',
      component: UrlShortner
    },
    {
      path: '/not-found',
      name: 'not-found',
      component: NotFound
    },
    {
      path: '/:pathMatch(.*)*',
      redirect: '/not-found'
    }
  ]
})

export default router
