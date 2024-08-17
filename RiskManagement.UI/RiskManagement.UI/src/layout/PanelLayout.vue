<script setup>
import NavBar from '@/layout/components/NavBar.vue'
import Sidebar from '@/layout/components/Sidebar.vue'
</script>

<template>
  <div class="flex flex-col">
    <NavBar class="w-full"></NavBar>
    <div class="flex">
        <Sidebar  class="flex w-1/5"/>
        <div class="flex w-4/5">
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
  height: 85vh !important;
}
</style>
