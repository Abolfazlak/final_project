import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import { toast } from 'vue3-toastify';
import { useRoute, useRouter } from 'vue-router'

export const useUserStore = defineStore("user", {

  state: () => ({
    user: {

    },
    hasRiskData: true,
    hasRiskDetailsData: true,
    hasRiskSolutionsData: true,
    hasRiskStatusData: true,
    oppData: [],
    riskData: [],
    rupchartData: [],
    pichartData: [],
    routeName: null,
    route: useRoute(),
    router: useRouter(),
    isFirstGetCategory: false,
    riskUpdateItems: null,
    updateItems: null,
    solutionUpdateItems: null,
    changeStatusModal: false,
    createSolutionModal: false,
    updateSolutionModal: false,
    createRiskModal: false,
    riskDetailsModal: false,
    updateRiskModal: false,
    createProjectModal: false,
    updateProjectModal: false,
    url: "http://localhost:5151/",
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

