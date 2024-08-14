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


app.mount('#app')
