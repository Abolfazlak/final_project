<script setup>
import {provide} from 'vue'
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
