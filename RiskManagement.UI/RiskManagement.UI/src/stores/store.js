import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import { toast } from 'vue3-toastify';

export const useUserStore = defineStore("user", {

  state: () => ({
    user: {

    },
    isFirstGetCategory: false,
    riskUpdateItems: null,
    updateItems: null,
    solutionUpdateItems: null,
    createSolutionModal: false,
    updateSolutionModal: false,
    createRiskModal: false,
    riskDetailsModal: false,
    updateRiskModal: false,
    createProjectModal: false,
    updateProjectModal: false,
    url: "http://172.20.10.3:5151/",
    loginRes: false,
    token: JSON.parse(localStorage.getItem('token')),
    isAdmin: JSON.parse(localStorage.getItem('isAdmin'))
  }),


  actions: {
    async Login(req) {
      let url = `${this.url}users/login`;
      try {
        let res = await fetch(url, {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(req),
        });
        await res.json().then((response) => {
          console.log('response-login', response)

          localStorage.setItem('token', JSON.stringify(response.data.token))
          localStorage.setItem('isAdmin', JSON.stringify(response.data.isAdmin))   
          
          this.loginRes = true;
          this.token = JSON.parse(localStorage.getItem('token'))
          this.isAdmin = JSON.parse(localStorage.getItem('isAdmin'))

          this.GetUser();
        });

      } catch (error) {
        console.log('response-login', error)
        this.loginRes = false;
        this.token = null
      }
    },

    async Register(req) {
      let url = `${this.url}users/registerAdmin`;
      try {
        let res = await fetch(url, {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(req),
        });
        await res.json().then((response) => {
          console.log('response-login', response)
        });
      } catch (error) {
        console.log('response-login', error)
      }
    },

    async Update(req) {
      let url = `${this.url}users/updateUser`;
      try {
        let res = await fetch(url, {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
            Authorization: 'Bearer ' + this.token
          },
          body: JSON.stringify(req),
        });
        await res.json().then((response) => {
          console.log('response-Update', response)
        });
      } catch (error) {
        console.log('response-Update', error)
      }
    },

    async GetUser() {
      let url = `${this.url}users/getUser`;
      let t = JSON.parse(localStorage.getItem('token'))
      try {
        let res = await fetch(url, {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
            Authorization: 'Bearer ' + t
          },
        });
        await res.json().then((response) => {
          this.user = response.data
          console.log('response-GetUser', response)
        });
      } catch (error) {
        console.log('response-GetUser', error)
      }
    },

    async Logout(){
      localStorage.clear()
      localStorage.clear()
      this.token = null
      this.loginRes = false
      this.isAdmin = false
    }
  },
});

