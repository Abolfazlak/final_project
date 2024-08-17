<template>
  <modal :show="isCreateModalVisibleRef" @close="closeCreateModal">
    <template v-slot:header> <div class="font-bold text-base">ایجاد پروژه جدید</div></template>
    <template v-slot:body>
      <v-locale-provider rtl>
        <v-row class="flex gap-3 items-center text-center justify-center mt-2 px-3">
          <v-text-field
            v-model="createModel.projectName"
            label="عنوان پروژه"
            variant="outlined"
            rounded="lg"
            required
          ></v-text-field>
        </v-row>
        <v-textarea
          v-model="createModel.description"
          class="mt-2"
          label="توضیحات"
          variant="outlined"
          rounded="lg"
        ></v-textarea>
        <v-row class="flex gap-3 items-center text-center justify-center mt-2 px-3">
          <div class="w-full flex">
            <v-autocomplete
              class="w-1/2 ml-2"
              v-model="createModel.assigneeUserId"
              label="مسئول"
              :items="people"
              item-title="fullName"
              item-value="id"
              variant="outlined"
              rounded="lg"
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
              v-model="createModel.methodology"
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
          @click="createProjectBtn"
          >ثبت</v-btn
        >
        <v-btn
          class="flex w-2.4/5 py-6 text-center items-center"
          color="red"
          variant="outlined"
          rounded="lg"
          @click="closeCreateModal"
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

const user = useUserStore()

const avatar = '/avatar.png'

const emit = defineEmits('click', 'closeCreateModal')

const props = defineProps(['isCreateModalVisible'])
const people = ref(null)

let token = JSON.parse(localStorage.getItem('token'))

const isCreateModalVisibleRef = ref(props.isCreateModalVisible)

const methodologies = ['agile', 'rup']

const createModel = reactive({
  projectName: '',
  methodology: '',
  assigneeUserId: '',
  description: ''
})

function closeCreateModal() {
  isCreateModalVisibleRef.value = false
  user.createProjectModal = false
}

function createProjectBtn() {
  createProject()
  closeCreateModal()
}
onMounted(() => {
  getUsers()
})

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
    await res.json().then((response) => {
      people.value = response.data
    })
  } catch (error) {
    console.log('response-users', error)
  }
}

async function createProject() {
  let url = `${user.url}project/createProject`
  try {
    let res = await fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: 'Bearer ' + token
      },
      body: JSON.stringify(createModel)
    })
    await res.json().then((response) => {
      toast.success(response.message)
    })
  } catch (error) {
    toast.error('عملیات با خطا مواجه شد')
  }
}

watch(
  () => props.isCreateModalVisible,
  async (newval) => {
    isCreateModalVisibleRef.value = newval
  }
)
</script>

<style></style>
