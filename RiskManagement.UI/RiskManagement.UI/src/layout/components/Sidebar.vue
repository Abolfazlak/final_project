<template>
  <div class="sideBarContainer">
      <div class="sideBarContainer-top">
        <div   v-for="item in items" :class=" item.name == routName ? 'sideBarContainer-top-item sideBarContainer-top-item-avtive' : 'sideBarContainer-top-item' ">
          <svg :style="{ width: '32px', height: '32px', color: 'black' }" viewBox="0 0 28 28">
        <path :d=item.icon />
      </svg>
      <div>{{item.text}}</div>
        </div>
      </div>
      <div class="sideBarContainer-down">
        <div></div>
      </div>
  </div>
</template>


<style>
  .sideBarContainer{
    background-color: #F6F6F6;
    width: 420px;
    height: 91vh;
    box-shadow: rgba(33, 35, 38, 0.1) 0px 20px 10px 2px;
    margin-top: -1vh;
    display: flex;
    flex-direction: column;
    padding: 50px 0px;
    justify-content: space-between;

  }
  .sideBarContainer-top{
    display: flex;
    flex-direction: column;
    gap: 20px;
    
  }
  .sideBarContainer-top-item{
      padding: 20px;
      display: flex;
      align-items: center;
      padding-right: 30px;
      border-radius: 5px;
      gap: 10px;
      font-size: 25px;
      font-family: 'Yekan-Bakh-Heavy';
  }
  .sideBarContainer-top-item:hover{
    transition: 0.5s;
    cursor: pointer;
    background-color: rgb(169, 179, 214);
  }
  .sideBarContainer-top-item-avtive{
    background-color: rgb(169, 179, 214);
  }

</style>

<script setup>
import {mdiCog , mdiChartBar ,mdiBookMultiple , mdiListStatus , mdiShieldPlus } from '@mdi/js'
import {useRoute} from 'vue-router'

const route = useRoute()
const items =  [
        { text: 'مدیریت پروژه', icon: mdiBookMultiple, name: 'projects'},
        { text: 'مدیریت ریسک', icon: mdiShieldPlus,  name: 'risks'},
        { text: 'وضعیت ریسک', icon: mdiListStatus, name: 'stasus' },
        { text: 'گزارشات', icon: mdiChartBar, name: 'reports' },
        { text: 'مدیریت کاربران', icon: mdiCog, name: 'setting' },
      ]

const routName = route.name

</script>