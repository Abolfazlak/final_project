<template>
    <div class="Navbar">
        <div class="Navbar-Right">
            <img class="Navbar-Right-Logo" src="/logo.png" alt="logo">
        </div>
        <div class="Navbat-Left">
            <svg :style="{ width: '32px', height: '32px', color: 'black' }" viewBox="0 0 28 28">
                <path :d="mdiAccount" />
            </svg>
                        
        <div class="loginBtn" @click="showModalFunc">ورود / ثبت نام</div>
        </div>
    </div>

    <modal :show="isModalVisibleRef" @close="closeModal" class="">
        <template v-slot:header>
            <div>
                ورود
            </div>
        </template>
        
    </modal>
</template>


<script setup>
import { mdiAccount } from '@mdi/js';
// import modal from './Modal.vue';
import { ref, watch } from 'vue';

const props = defineProps(['isModalVisible'])
// const emit = defineEmits('click', 'closeModal')
const isModalVisibleRef = ref(props.isModalVisible);


function closeModal() {
  isModalVisibleRef.value = false;
}

function showModalFunc(){
    isModalVisibleRef.value = true;
}

watch(() =>props.isModalVisible, async (newval) => {
    isModalVisibleRef.value = newval;
  }
)

</script>

<style>
.Navbar{
    height: 80px;
    display: flex;
    justify-content: space-between;
    align-items: center;
    text-align: center;
    padding: 0 60px;
    padding-top: 10px;
    box-shadow: rgba(33, 35, 38, 0.1) 0px 10px 10px -10px;
}
.Navbat-Left{
    display: flex;
    gap: 10px;
}

.Navbar-Right-Logo{
    width: 70px;
}
.Navbar-Right-Logo:hover{
    cursor: pointer;
}

.loginBtn:hover{
    cursor: pointer;
}
</style>
