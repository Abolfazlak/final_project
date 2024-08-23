<template>
  <modal :show="isCreateModalVisibleRef" @close="closeCreateModal" class="">
    <template v-slot:header>
      <div class="font-bold text-base">ایجاد کاربر جدید</div>
    </template>
    <template v-slot:body>
      <div class="">
        <v-container class="mt-4 h-product-web overflow-auto">
          <v-locale-provider rtl>
            <v-form ref="registerFormRef">
              <v-text-field
                v-model="registerModel.username"
                label="نام کاربری"
                variant="outlined"
                rounded="lg"
                :rules="[usernameRule.required]"
              ></v-text-field>
              <v-text-field
                class="mt-2"
                v-model="registerModel.password"
                label="رمز عبور"
                variant="outlined"
                rounded="lg"
                type="password"
                :rules="passwordRules"
              ></v-text-field>
              <v-text-field
                v-model="registerModel.repeatPassword"
                class="mt-2"
                label="تکرار رمز عبور"
                variant="outlined"
                rounded="lg"
                type="password"
                :rules="repeatedPasswordRules"
              ></v-text-field>
              <v-text-field
                v-model="registerModel.fullName"
                class="mt-2"
                label="نام و نام خانوادگی"
                variant="outlined"
                rounded="lg"
                :rules="[fullnameRule.required]"
              ></v-text-field>
              <v-text-field
                v-model="registerModel.email"
                class="mt-2"
                label="ایمیل سازمانی"
                variant="outlined"
                rounded="lg"
                type="email"
                :rules="emailRule"
              ></v-text-field>
            </v-form>
          </v-locale-provider>
        </v-container>
      </div>
    </template>

    <template v-slot:footer>
      <v-row class="flex text-center items-center justify-center mt-8 px-3 gap-3 mb-1">
        <v-btn
          class="flex w-2.3/5 py-6 text-center items-center text-white"
          color="#4da35a"
          rounded="lg"
          @click="submitRegisterForm"
          >ثبت نام</v-btn
        >
        <v-btn
          class="flex w-2.3/5 py-6 text-center items-center"
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
import { ref, reactive, watch } from 'vue'
import { useUserStore } from '@/stores/store.js'
import { toast } from 'vue3-toastify'

const user = useUserStore()

const emit = defineEmits('click', 'closeCreateModal')
const props = defineProps(['isCreateModalVisible'])

let token = JSON.parse(localStorage.getItem('token'))

const isCreateModalVisibleRef = ref(props.isCreateModalVisible)
const registerFormRef = ref(null)
const registerModel = reactive({
  username: '',
  password: '',
  repeatPassword: '',
  fullName: '',
  email: '',
  companyName: ''
})

function closeCreateModal() {
  Object.assign(registerModel, {
    username: '',
    password: '',
    repeatPassword: '',
    fullName: '',
    email: ''
  })
  isCreateModalVisibleRef.value = false
  user.createUserModal = false
}

function submitRegisterForm() {
  registerFormRef.value?.validate().then(async ({ valid: isValid }) => {
    if (isValid) {
      await createUser(registerModel)
    } else {
    }
  })
}

async function createUser() {
  let url = `${user.url}users/registerUser`

  try {
    let res = await fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: 'Bearer ' + token
      },
      body: JSON.stringify(registerModel)
    })
    if (res.status == 401) {
      user.Logout()
    }
    await res.json().then((response) => {
      closeCreateModal()
      toast.success(response.message)
    })
  } catch (error) {
    toast.error('عملیات با خطا مواجه شد')
  }
}

const usernameRule = {
  required: (value) => !!value || 'نام کاربری اجباری است!'
}

const fullnameRule = {
  required: (value) => !!value || 'نام و نام خانوادگی اجباری است!'
}

const emailRule = [
  (v) => !!v || 'ایمیل اجباری است!',
  (v) =>
    /^(([^<>()[\]\\.,;:\s@']+(\.[^<>()\\[\]\\.,;:\s@']+)*)|('.+'))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/.test(
      v
    ) || 'ساختار ایمیل اشتباه است!'
]

const passwordRules = [
  (v) => !!v || 'رمز عبور اجباری است!',
  (v) =>
    /(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,}/.test(v) ||
    'رمز عبور باید شامل حرف کوچک، بزرگ، عدد و علامت باشد!'
]

const repeatedPasswordRules = [
  (v) => !!v || 'تکرار رمز عبور اجباری است!',
  (v) => v == registerModel.password || 'رمز عبور و تکرار رمز عبور باید یکسان باشند!'
]

watch(
  () => props.isCreateModalVisible,
  async (newval) => {
    isCreateModalVisibleRef.value = newval
  }
)
</script>

<style></style>
