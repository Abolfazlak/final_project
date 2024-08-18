<template>
  <div class="Navbar">
    <div class="Navbar-Right">
      <img class="Navbar-Right-Logo" src="/logo.png" alt="logo" />
      <div class="Navbar-Right-title">سامانه مدیریت ریسک راسا</div>
    </div>
    <div class="Navbat-Left" v-if="token || loginResRef" @click="showEditProfile">
      <svg
        class="icon-navbar"
        :style="{ width: '32px', height: '32px', color: 'black' }"
        viewBox="0 0 28 28"
      >
        <path :d="mdiAccount" />
      </svg>

      <div class="loginBtn">{{ user.user.fullName }}</div>
    </div>
    <div v-else class="Navbat-Left">
      <svg
        class="icon-navbar"
        :style="{ width: '32px', height: '32px', color: 'black' }"
        viewBox="0 0 28 28"
      >
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
              <v-btn class="mt-6 w-full h-12" color="#4da35a" @click="submitLoginForm">ورود</v-btn>
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
        <v-btn class="mt-2 w-full h-12" color="#4da35a" @click="submitRegisterForm">ثبت نام</v-btn>
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

  <modal :show="isProfileModalVisibleRef" @close="closeUpdateModal" class="">
    <template v-slot:header>
      <div class="font-bold text-base">پروفایل کاربری</div>
    </template>

    <template v-slot:body v-if="!isRegisterForm">
      <div class="">
        <v-container class="mt-4">
          <v-locale-provider rtl>
            <v-form ref="updateFormRef">
              <v-text-field
                v-model="userProfile.userName"
                label="نام کاربری"
                variant="outlined"
                rounded="lg"
                disabled
                required
              ></v-text-field>
              <v-text-field
                v-model="userProfile.fullName"
                class="mt-2"
                label="نام و نام خانوادگی"
                variant="outlined"
                rounded="lg"
                :rules="[fullnameRule.required]"
              ></v-text-field>
              <v-text-field
                v-model="userProfile.email"
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
      <v-row class="flex text-center items-center justify-center mt-8 px-4 gap-5 mb-1">
        <v-btn
          class="flex w-1.1/2 py-6 text-center items-center text-white"
          color="#4da35a"
          rounded="lg"
          @click="submitUpdateForm"
          >ویرایش</v-btn
        >
        <v-btn
          class="flex w-1.1/2 py-6 text-center items-center"
          color="red"
          rounded="lg"
          variant="outlined"
          @click="closeUpdateModal"
          >انصراف</v-btn
        >
      </v-row>
      <v-btn
        class="flex w-11/12 py-6 justify-center mr-4 mt-6 text-black"
        color="#7FC2A9"
        rounded="lg"
        variant="outlined"
        @click="logoutFunc"
      >
        <svg
          :style="{ width: '24px', height: '24px', color: 'black', paddingLeft: '4px' }"
          viewBox="0 0 28 28"
        >
          <path :d="mdiLogout" />
        </svg>
        خروج از حساب کاربری
      </v-btn>
    </template>
  </modal>
</template>

<script setup>
import { mdiAccount, mdiLogout } from '@mdi/js'
import modal from '@/components/Modal.vue'
import { reactive, ref, watch, onMounted } from 'vue'
import { useUserStore } from '@/stores/store.js'

import { useRouter } from 'vue-router'
import { toast } from 'vue3-toastify'

onMounted(() => {
  token.value = localStorage.getItem('token')
  if (token.value) {
    user.GetUser()
  }
})

const router = useRouter()

const user = useUserStore()

const props = defineProps(['isLoginModalVisible', 'isProfileModalVisible'])
const emit = defineEmits('click', 'closeLoginModal')

const loginFormRef = ref(null) // Add ref to access v-form
const registerFormRef = ref(null)
const updateFormRef = ref(null)

const userProfile = ref(user.user)

const token = ref(null)
const loginResRef = ref(user.loginRes)

const isLoginModalVisibleRef = ref(props.isLoginModalVisible)
const isProfileModalVisibleRef = ref(props.isProfileModalVisible)

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

function closeUpdateModal() {
  isProfileModalVisibleRef.value = false
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

function logoutFunc() {
  closeUpdateModal()
  user.loginRes = false
  user.Logout()
}

function showEditProfile() {
  isProfileModalVisibleRef.value = true
  console.log(userProfile.value)
}

function submitRegisterForm() {
  registerFormRef.value?.validate().then(async ({ valid: isValid }) => {
    if (isValid) {
      await user.Register(registerModel)
    } else {
    }
  })
}

function submitUpdateForm() {
  updateFormRef.value?.validate().then(async ({ valid: isValid }) => {
    if (isValid) {
      await user.Update(userProfile.value)
      closeUpdateModal()
      toast.success('.اطلاعات کاربر با موفقیت ویرایش شد')
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
    console.log(newval)
  }
)

watch(
  () => user.user,
  async (newval, oldval) => {
    userProfile.value = newval
  }
)
</script>

<style>
.Navbar {
  height: 10vh;
  display: flex;
  justify-content: space-between;
  align-items: center;
  text-align: center;
  padding: 0 10px;
  padding-top: 10px;
  box-shadow: rgba(33, 35, 38, 0.1) 0px 10px 10px -10px;
  background-color: #f9f9f9;
}

.Navbar-Right {
  display: flex;
  align-items: center;
  justify-content: center;
}
.Navbar-Right-Logo {
  width: 70px;
  -webkit-filter: grayscale(100%); /* Safari 6.0 - 9.0 */
  filter: grayscale(100%);
}
.Navbar-Right-Logo:hover {
  cursor: pointer;
}

.Navbar-Right-title {
  font-family: 'Yekan-Bakh-Fat';
  font-size: 20px;
  color: #1a2731;
}
.Navbat-Left {
  background-color: #fff;
  display: flex;
  gap: 5px;
  align-items: center;
  justify-content: center;
  margin-left: 30px;
  font-size: 15px;
  font-family: 'Yekan-Bakh-Heavy';
  padding: 8px 20px;
  border-radius: 5px;
  border: 0.5px solid #dcdcdc;
  transition: 0.5s;
  box-shadow: rgba(165, 159, 149, 0.1) 0px 8px 16px;
}
.Navbat-Left:hover {
  border: solid 1.3px #ec622e;
  color: #ec622e;
  cursor: pointer;
  transition: 0.5s;
}
.loginBtn {
  display: flex;
}

.loginBtn:hover {
  cursor: pointer;
}

.registerBtn {
  text-decoration: underline;
  cursor: pointer;
}

.Navbat-Left .icon-navbar path {
  fill: black; /* Default icon color */
  transition: fill 0.3s ease; /* Smooth transition for color change */
}

.Navbat-Left:hover .icon-navbar path {
  fill: #ec622e; /* Icon color on hover */
}
</style>
