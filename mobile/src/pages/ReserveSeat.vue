<template>
  <div class="main">
  </div>
  <div class="button-container">
    <button class="reserve-seat__button">Reserve Seat</button>
    <router-link class="mainmenu-router__link" to="/">
      <button class="back-button">Back</button>
    </router-link>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
</script>

<style scoped>
* {
  margin: 0;
  padding: 0;
}

.main {
  background-repeat: no-repeat;
  background-position: center top;
  width: 100%;
  min-height: 600px;
  margin-top: 40px;
  position: relative;
}

.text-window ul {
  list-style-type: none;
}

.text-window li {
  border-bottom: 2px solid black;
  padding: 10px;
  overflow: hidden;
}

.text-window li:last-child{
  border-bottom: none;
}

.mainmenu-router__link {
  text-decoration: none;
  display: block;
  margin: auto;
  width: 200px;
}

.back-button {
  padding: 15px;
  border-radius: 30px;
  font-size: 17px;
  font-weight: 800;
  background-color: white;
  display: block;
  cursor: pointer;
  width: 200px;
}

.reserve-seat__button{
  padding: 15px;
  border-radius: 30px;
  font-size: 17px;
  font-weight: 800;
  cursor: pointer;
  margin: 0;
  width: 200px;
  background-color: white;
}

.button-container{
  display: flex;
  flex-direction: row;
  margin-left: 25px;
}
</style>