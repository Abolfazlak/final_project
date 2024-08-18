<template>
  <div :class="routName == 'report' ? 'sideBarContainer' : 'sideBarContainer-sp' ">
    <div class="sideBarContainer-top">
      <div
        v-for="item in items"
        :class="
          item.name == routName || item.name2 == routName
            ? 'sideBarContainer-top-item sideBarContainer-top-item-avtive'
            : 'sideBarContainer-top-item'
        "
        @click="routeToPages(item.name)"
      >
        <svg class="icon-sidebar" :style="{ width: '32px', height: '32px' }" viewBox="0 0 28 28">
          <path :d="item.icon" :style="{ fill: getIconColor(item) }" />
        </svg>
        <div>{{ item.text }}</div>
      </div>
    </div>
    <div class="sideBarContainer-down">
      <div></div>
    </div>
  </div>
</template>

<style>
.sideBarContainer {
  /* background-color: #f2f2f2; */
  background-color: #f9f9f9;
  width: 360px;
  height: 92vh;
  box-shadow: rgba(33, 35, 38, 0.1) 0px 20px 10px 2px;
  margin-top: -1vh;
  display: flex;
  flex-direction: column;
  padding: 50px 0px;
  justify-content: space-between;
}

.sideBarContainer-sp {
  /* background-color: #f2f2f2; */
  background-color: #f9f9f9;
  width: 360px;
  height: 92vh;
  box-shadow: rgba(33, 35, 38, 0.1) 0px 20px 10px 2px;
  margin-top: -1vh;
  display: flex;
  flex-direction: column;
  padding: 50px 0px;
  justify-content: space-between;
}

.sideBarContainer-top {
  display: flex;
  flex-direction: column;
  gap: 20px;
}
.sideBarContainer-top-item {
  padding: 20px;
  display: flex;
  align-items: center;
  padding-right: 30px;
  border-radius: 5px;
  gap: 10px;
  font-size: 18px;
  font-family: 'Yekan-Bakh-Heavy';
}
.sideBarContainer-top-item:hover {
  transition: 0.5s;
  cursor: pointer;
  /* background-color: rgb(169, 179, 214); */
  /* background-color: #4da35a; */
  background-color: #EC622E;
}
.sideBarContainer-top-item-avtive {
  /* background-color: rgb(169, 179, 214); */
  /* background-color: #386B7B; */
  background-color: #EC622E;

  /* background-color: #5688b0; */
  color: white;
}
</style>

<script setup>
import { mdiCog, mdiChartBar, mdiBookMultiple, mdiListStatus, mdiShieldPlus } from '@mdi/js'
import { useRoute, useRouter } from 'vue-router'
import { useUserStore } from '@/stores/store.js'
import { ref, watch } from 'vue'

const user = useUserStore()
const route = useRoute()
const router = useRouter()

const routName = ref(user.routeName)
const id = route.params.id

const isAdmin = JSON.parse(localStorage.getItem('isAdmin'))

const allItems = [
  { text: 'مدیریت پروژه', icon: mdiBookMultiple, name: 'projects', name2: '' },
  { text: 'مدیریت ریسک', icon: mdiShieldPlus, name: 'risks', name2: 'solutions' },
  { text: 'وضعیت ریسک', icon: mdiListStatus, name: 'status', name2: '' },
  { text: 'گزارشات', icon: mdiChartBar, name: 'reports', name2: '' },
  { text: 'مدیریت کاربران', icon: mdiCog, name: 'setting', name2: '' }
]

const items = ref(allItems.filter(item => isAdmin || item.name !== 'setting'))


function routeToPages(name) {
  if (name == 'projects') {
    router.push({ name: name })
  } else if (name == 'setting') {
    router.push({ name: name })
  } else {
    router.push({ name: name, params: { id: id } })
  }
}

function getIconColor(item) {
  return item.name == this.routName || item.name2 == this.routName
    ? '#ffffff' // Active color
    : '#000000' // Default color
}

watch(
  () => user.routeName,
  async (newval) => {
    if (newval) {
      routName.value = newval
    }
  }
)
</script>
