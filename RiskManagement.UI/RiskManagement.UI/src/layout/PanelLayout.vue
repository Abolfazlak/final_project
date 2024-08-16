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
            <div class="empty-layout mt-2"></div>
            <RouterView class="main-layout"/>
        </div>
  
    </div>
  </div>
</template>

<style scoped>

.empty-layout{
  width: 2vw;
}

.main-layout{
  width: 98vw;
}
</style>
