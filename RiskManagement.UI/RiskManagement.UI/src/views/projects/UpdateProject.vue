<template>
  <modal :show="isUpdateModalVisibleRef" @close="closeUpdateModal">
    <template v-slot:header> <div class="font-bold text-base">ویرایش پروژه</div></template>
    <template v-slot:body>
      <v-locale-provider rtl>
        <v-row class="flex gap-3 items-center text-center justify-center mt-2 px-3">
          <v-text-field
            v-model="updateItems.projectName"
            label="عنوان پروژه"
            variant="outlined"
            rounded="lg"
            required
          ></v-text-field>
        </v-row>
        <v-textarea
          v-model="updateItems.description"
          class="mt-2"
          label="توضیحات"
          variant="outlined"
          rounded="lg"
        ></v-textarea>
        <v-row class="flex gap-3 items-center text-center justify-center mt-2 px-3">
          <div class="w-full flex">
            <v-autocomplete
              class="w-1/2 ml-2"
              v-model="updateItems.userDto"
              label="مسئول"
              :items="people"
              item-title="userName"
              item-value="id"
              variant="outlined"
              rounded="lg"
              :prepend-avatar="avatar"
            >
              <template v-slot:chip="{ props, item }">
                <v-chip v-bind="props" :prepend-avatar="avatar" :text="item.raw.name"></v-chip>
              </template>

              <template v-slot:item="{ props, item }">
                <v-list-item
                  v-bind="props"
                  :prepend-avatar="avatar"
                  :subtitle="item.raw.userName + '@'"
                  :title="item.raw.fullName"
                ></v-list-item>
              </template>
            </v-autocomplete>
            <v-select
              class="w-1/2 mr-2"
              label="متدولوژی"
              variant="outlined"
              rounded="lg"
              :items="methodologies"
            ></v-select>
          </div>
        </v-row>
      </v-locale-provider>
    </template>
    <template v-slot:footer>
      <v-row class="flex text-center items-center justify-center mt-8 px-3 gap-3 mb-1">
        <v-btn
          class="flex w-2.4/5 py-6 text-center items-center"
          color="#4da35a"
          rounded="lg"
          @click="updateProject"
          >ثبت</v-btn
        >
        <v-btn class="flex w-2.4/5 py-6 text-center items-center" variant="outlined" color="red" rounded="lg" @click="closeUpdateModal"
          >انصراف</v-btn
        >
      </v-row>
    </template>
  </modal>
</template>

<script setup>
import modal from '@/components/CreateUpdateModal.vue'
import { ref, onMounted, reactive, watch } from 'vue'
import { useUserStore } from '@/stores/store.js'
import { toast } from 'vue3-toastify'

const avatar = 'avatar.png'

const user = useUserStore()

const emit = defineEmits('click', 'closeUpdateModal')

const props = defineProps(['isUpdateModalVisible', 'items'])
const people = ref(null)

const isUpdateModalVisibleRef = ref(props.isUpdateModalVisible)
const updateItems = ref(props.items)

const methodologies = ['agile', 'rup']

let token = JSON.parse(localStorage.getItem('token'))

async function updateProject() {
  const updateModel = reactive({
    id: updateItems.value.id,
    projectName: updateItems.value.projectName,
    description: updateItems.value.description,
    methodology: updateItems.value.methodology,
    assigneeUserId: updateItems.value.userDto.id
  })

  let url = `${user.url}project/updateProject`
  try {
    let res = await fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: 'Bearer ' + token
      },
      body: JSON.stringify(updateModel)
    })
    if(res.status == 401){
      user.Logout();
    }
    await res.json().then((response) => {
      closeUpdateModal()
      toast.success(response.message)
    })
  } catch (error) {
    console.log('response-updateProject', error)
  }
}



async function getUsers() {
  let url = `${user.url}users/getAllUsers`
  try {
    let res = await fetch(url, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: 'Bearer ' + token
      }
    })
    if(res.status == 401){
      user.Logout();
    }
    await res.json().then((response) => {
        people.value = response.data      
    })
  } catch (error) {
    console.log('response-users', error)
  }
}
getUsers()

function closeUpdateModal() {
  isUpdateModalVisibleRef.value = false
  user.updateProjectModal = false
  user.updateItems = null
  window.location.reload()
}

watch(
  () => props.isUpdateModalVisible,
  async (newval) => {
    isUpdateModalVisibleRef.value = newval
  }
)

watch(
  () => props.items,
  async (newval) => {
    updateItems.value = newval
  }
)
</script>

<style></style>
