<script setup>
import {  ref, getCurrentInstance } from 'vue';
const props = defineProps({
  show: Boolean
});

const modalRef = ref(null);
const instance = getCurrentInstance();

function closeModalOutside(event) {
  if (event.target === modalRef.value) {
    instance.emit('close');
  }
}
</script>

<template>
  <Transition name="modal">
    <div v-if="show" class="modal-mask" ref="modalRef" @click="closeModalOutside">
      <div class="modal-container md:w-2/5 max-h-50 rounded-lg">
        <div class="modal-header">
          <slot name="header"></slot>
     
        </div>

        <div class="modal-body ">
            <slot class="" name="body"></slot>
      
        </div>

        <div class="modal-footer">
          <slot name="footer">
        
    
          </slot>
        </div>
      </div>
    </div>
  </Transition>
</template>

<style scoped>
.modal-mask {
  position: fixed;
  z-index: 1000;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  transition: opacity 0.3s ease;
}

.modal-container {

  margin: auto;
  padding: 20px 30px;
  background-color: #fff;
  border-radius: 2px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.33);
  transition: all 0.3s ease;
  direction: rtl;
}

.modal-header h3 {
  margin-top: 0;
}

.modal-body {
  margin: 10px 0;
  padding-top: 20px;
  border-top: 1px solid rgba(181, 181, 181, 0.498);
}

.modal-default-button {
  float: right;
}

/*
 * The following styles are auto-applied to elements with
 * transition="modal" when their visibility is toggled
 * by Vue.js.
 *
 * You can easily play with the modal transition by editing
 * these styles.
 */

.modal-enter-from {
  opacity: 0;
}

.modal-leave-to {
  opacity: 0;
}

.modal-enter-from .modal-container,
.modal-leave-to .modal-container {
  -webkit-transform: scale(1.1);
  transform: scale(1.1);
}
</style>