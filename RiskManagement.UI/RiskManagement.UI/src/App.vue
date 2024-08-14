<script setup>
import {watch} from 'vue'
import { useRouter } from 'vue-router'
import { useUserStore } from '@/stores/store.js'

const router = useRouter()
const user = useUserStore()

watch(
  () => user.token,
  async (newval, oldval) => {
    if(!newval){
      router.push({ path: '/' })
    }
  }
)

if(!user.token){
  router.push({ path: '/' })
}

</script>

<template>

<main>
  <component :is="$route.meta.layout || 'div'">
    <RouterView />
  </component>

</main>



</template>

<style scoped>

</style>
