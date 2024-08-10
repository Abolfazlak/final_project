<script setup>
import NavBar from '@/layout/components/NavBar.vue'
import Sidebar from '@/layout/components/Sidebar.vue'
</script>

<template>
  <div class="flex flex-col">
    <NavBar class="w-full"></NavBar>
    <div class="flex ">
        <Sidebar  />
        <div class="flex  w-full">
            <div class="w-1/6 mt-4"></div>
            <RouterView class="w-5/6 "/>
        </div>
  
    </div>
  </div>
</template>

<style scoped></style>
