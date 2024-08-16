<template>
  <div class="Navbar">
    <div class="Navbar-Right">
      <img class="Navbar-Right-Logo" src="/logo.png" alt="logo" />
    </div>
    <div class="Navbat-Left" v-if="token || loginResRef">
      <svg :style="{ width: '32px', height: '32px', color: 'black' }" viewBox="0 0 28 28">
        <path :d="mdiLogout" />
      </svg>

      <div class="loginBtn" @click="logoutFunc">خروج</div>
    </div>
    <div  v-else class="Navbat-Left">
      <svg :style="{ width: '32px', height: '32px', color: 'black' }" viewBox="0 0 28 28">
        <path :d="mdiAccount" />
      </svg>

      <div class="loginBtn" @click="showLoginModalFunc">ورود / ثبت نام</div>
    </div>
  </div>

  <modal :show="isLoginModalVisibleRef" @close="closeLoginModal" class="">
    <template v-slot:header v-if="!isRegisterForm">
      <div class="font-bold text-base">ورود</div>
    </template>
    <template v-slot:header v-if="isRegisterForm">
      <div class="font-bold text-base">ثبت نام</div>
    </template>

    <template v-slot:body v-if="!isRegisterForm">
      <div class="">
        <div class="flex text-lg font-medium items-center text-center justify-center">
          به سامانه مدیریت ریسک خوش آمدید
        </div>
        <v-container class="mt-4">
          <v-locale-provider rtl>
            <v-form ref="loginFormRef">
              <v-text-field
                v-model="loginModel.username"
                label="نام کاربری"
                variant="outlined"
                rounded="lg"
                :rules="[usernameRule.required]"
                required
              ></v-text-field>
              <v-text-field
                class="mt-2"
                v-model="loginModel.password"
                label="رمز عبور"
                variant="outlined"
                rounded="lg"
                type="password"
                :rules="loginPasswordRules"
                required
              ></v-text-field>
              <v-btn class="mt-6 w-full h-12" color="primary" @click="submitLoginForm">ورود</v-btn>
            </v-form>
          </v-locale-provider>
        </v-container>
      </div>
    </template>

    <template v-slot:body v-if="isRegisterForm">
      <div class="">
        <div class="flex text-lg font-medium items-center text-center justify-center">
          به سامانه مدیریت ریسک خوش آمدید
        </div>
        <v-container class="mt-4 h-64 overflow-auto">
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
                v-model="registerModel.companyName"
                class="mt-2"
                label="نام شرکت"
                variant="outlined"
                rounded="lg"
                :rules="[companyNameRule.required]"
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
        <v-btn class="mt-2 w-full h-12" color="primary" @click="submitRegisterForm">ثبت نام</v-btn>
      </div>
    </template>

    <template v-slot:footer v-if="!isRegisterForm">
      <div class="flex mt-4 text-sm items-center text-center justify-center">
        <div>حساب کاربری ندارید؟ &nbsp</div>
        <div class="registerBtn" @click="registerBtnClicked">ثبت نام کنید</div>
      </div>
    </template>

    <template v-slot:footer v-if="isRegisterForm">
      <div class="flex mt-4 text-sm items-center text-center justify-center">
        <div>قبلا ثبت نام کرده‌اید؟&nbsp</div>
        <div class="registerBtn" @click="loginBtnClicked">وارد شوید</div>
      </div>
    </template>
  </modal>
</template>

<script setup>
import { mdiAccount, mdiLogout } from '@mdi/js'
import modal from '@/components/Modal.vue'
import { reactive, ref, watch, onMounted } from 'vue'
import { useUserStore } from '@/stores/store.js'
import { toast } from 'vue3-toastify'

import { useRouter } from 'vue-router'

onMounted(() => {
  token.value = localStorage.getItem('token')
})

const router = useRouter()

const user = useUserStore()

const props = defineProps(['isLoginModalVisible'])
const emit = defineEmits('click', 'closeLoginModal')

const loginFormRef = ref(null) // Add ref to access v-form
const registerFormRef = ref(null)
const token = ref(null)
const loginResRef = ref(user.loginRes)


const isLoginModalVisibleRef = ref(props.isLoginModalVisible)
const isRegisterForm = ref(false)

const loginModel = reactive({ username: '', password: '' })
const registerModel = reactive({
  username: '',
  password: '',
  repeatPassword: '',
  fullName: '',
  email: '',
  companyName: ''
})

/*
 ****************************** validations *****************************
 */

const usernameRule = {
  required: (value) => !!value || 'نام کاربری اجباری است!'
}

const fullnameRule = {
  required: (value) => !!value || 'نام و نام خانوادگی اجباری است!'
}

const companyNameRule = {
  required: (value) => !!value || 'نام شرکت اجباری است!'
}

const emailRule = [
  (v) => !!v || 'ایمیل اجباری است!',
  (v) =>
    /^(([^<>()[\]\\.,;:\s@']+(\.[^<>()\\[\]\\.,;:\s@']+)*)|('.+'))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/.test(
      v
    ) || 'ساختار ایمیل اشتباه است!'
]

const loginPasswordRules = [(v) => !!v || 'رمز عبور اجباری است!']

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

/*
 ****************************** functions *****************************
 */
function closeLoginModal() {
  isLoginModalVisibleRef.value = false
  isRegisterForm.value = false
}

function showLoginModalFunc() {
  isLoginModalVisibleRef.value = true
}

function registerBtnClicked() {
  isRegisterForm.value = true
}

function loginBtnClicked() {
  isRegisterForm.value = false
}

function logoutFunc(){
  user.loginRes = false
  user.Logout()
}

function submitRegisterForm() {
  registerFormRef.value?.validate().then(async ({ valid: isValid }) => {
    if (isValid) {
      await user.Register(registerModel)
    } else {
    }
  })
}

function submitLoginForm() {
  loginFormRef.value?.validate().then(async ({ valid: isValid }) => {
    if (isValid) {
      await user.Login(loginModel)

      if (user.loginRes) {
        closeLoginModal()
        router.push({ name: 'projects' })
      } else {
        closeLoginModal()
      }
    } else {
    }
  })
}

/*
 ****************************** watch *****************************
 */
watch(
  () => props.isModalVisible,
  async (newval) => {
    isLoginModalVisibleRef.value = newval
  }
)

watch(
  () => user.loginRes,
  async (newval, oldval) => {
    loginResRef.value = newval
  }
)
</script>

<style>
.Navbar {
  height: 80px;
  display: flex;
  justify-content: space-between;
  align-items: center;
  text-align: center;
  padding: 0 60px;
  padding-top: 10px;
  box-shadow: rgba(33, 35, 38, 0.1) 0px 10px 10px -10px;
}
.Navbat-Left {
  display: flex;
  gap: 10px;
}

.Navbar-Right-Logo {
  width: 70px;
}
.Navbar-Right-Logo:hover {
  cursor: pointer;
}

.loginBtn:hover {
  cursor: pointer;
}

.registerBtn {
  text-decoration: underline;
  cursor: pointer;
}
</style>
