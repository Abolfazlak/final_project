<template>
  <div class="reportContiner w-full h-full pb-20">
    <div class="RC-Matrix">
        <div class="RC-Matrix-Row">
            <div style="background-color: #35B08B;" class="RC-Matrix-Column">{{data[0][0]}}</div>
            <div style="background-color: #94CD8B;" class="RC-Matrix-Column">{{data[0][1]}}</div>
            <div style="background-color: #FEEA81;" class="RC-Matrix-Column">{{data[0][2]}}</div>
            <div style="background-color: #FEEA81;" class="RC-Matrix-Column">{{data[0][3]}}</div>
            <div style="background-color: aqua;" class="RC-Matrix-Column">{{data[0][4]}}</div>
        </div>
        <div class="RC-Matrix-Row">
            <div style="background-color: #94CD8B;" class="RC-Matrix-Column">{{data[1][0]}}</div>
            <div style="background-color: #FEEA81;" class="RC-Matrix-Column">{{data[1][1]}}</div>
            <div style="background-color: #FEEA81;" class="RC-Matrix-Column">{{data[1][2]}}</div>
            <div style="background-color: aqua;" class="RC-Matrix-Column">{{data[1][3]}}</div>
            <div style="background-color: aqua;" class="RC-Matrix-Column">{{data[1][4]}}</div>
        </div>
        <div class="RC-Matrix-Row">
            <div style="background-color: #FEEA81;" class="RC-Matrix-Column">{{data[2][0]}}</div>
            <div style="background-color: #FEEA81;" class="RC-Matrix-Column">{{data[2][1]}}</div>
            <div style="background-color: aqua;" class="RC-Matrix-Column">{{data[2][2]}}</div>
            <div style="background-color: aqua;" class="RC-Matrix-Column">{{data[2][3]}}</div>
            <div style="background-color: aqua;" class="RC-Matrix-Column">{{data[2][4]}}</div>
        </div>
        <div class="RC-Matrix-Row">
            <div style="background-color: #FEEA81;" class="RC-Matrix-Column">{{data[3][0]}}</div>
            <div style="background-color: aqua;" class="RC-Matrix-Column">{{data[3][1]}}</div>
            <div style="background-color: aqua;" class="RC-Matrix-Column">{{data[3][2]}}</div>
            <div style="background-color: aqua;" class="RC-Matrix-Column">{{data[3][3]}}</div>
            <div style="background-color: aqua;" class="RC-Matrix-Column">{{data[3][4]}}</div>
        </div>
        <div class="RC-Matrix-Row">
            <div style="background-color: aqua;" class="RC-Matrix-Column">{{data[4][0]}}</div>
            <div style="background-color: aqua;" class="RC-Matrix-Column">{{data[4][1]}}</div>
            <div style="background-color: aqua;" class="RC-Matrix-Column">{{data[4][2]}}</div>
            <div style="background-color: aqua;" class="RC-Matrix-Column">{{data[4][3]}}</div>
            <div style="background-color: aqua;" class="RC-Matrix-Column">{{data[4][4]}}</div>
        </div>

    </div>
  </div>
</template>
<script setup> 
import { useRoute, useRouter } from 'vue-router'
import { useUserStore } from '@/stores/store.js'

const user = useUserStore()
const route = useRoute()

user.routeName = route.name


const data = [[1,2,1,1,1],[2,1,1,5,1],[1,1,1,1,1],[1,1,1,1,1],[1,1,1,1,1]]
</script>
<style>
.reportContiner {
  display: flex;
  align-items: center;
  justify-content: center;
}
.RC-Matrix {
  display: flex;
  flex-direction: column;
}
.RC-Matrix-Column{
    display: flex;
    align-items: center;
    justify-content: center;
    background-color: red;
    width: 100px;
    height: 100px;
    font-size: 25px;
    font-family: 'Yekan-Bakh-Heavy';
}
.RC-Matrix-Row{
    display: flex;
    direction: ltr;
}
</style>
