import './assets/main.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'


import App from './App.vue'
import router from './router'
import vuetify from './plugins/vuetify'
import 'vuetify/styles' 
import '@mdi/font/css/materialdesignicons.css'
import './app.css'
import Vue3Toastify from 'vue3-toastify';
import 'vue3-toastify/dist/index.css';
import Vue3PersianDatetimePicker from 'vue3-persian-datetime-picker'


const app = createApp(App)

app.use(createPinia())
app.use(router)

app.use(vuetify)

app.use(
    Vue3Toastify,
    {
      autoClose: 3000, // Auto-close after 3 seconds
      position: "bottom-center",
      hideProgressBar: false,
      closeOnClick: true,
      pauseOnHover: true
    }
  );
  
  app.component('DatePicker', Vue3PersianDatetimePicker)

  app.use(Vue3PersianDatetimePicker, {
    name: 'CustomDatePicker',
    props: {
      format: 'YYYY-MM-DD HH:mm',
      displayFormat: 'jYYYY-jMM-jDD HH:mm',
      editable: true,
      inputClass: 'form-control my-custom-class-name',
      placeholder: 'تاریخ',
      altFormat: 'YYYY-MM-DD HH:mm',
      color: 'black',
      autoSubmit: false,
    }
  })

app.mount('#app')
