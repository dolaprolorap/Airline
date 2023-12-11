<template>
  <div class="image-container">
    <img class="Logo" src="../assets/Amonic-Logo.png" alt="">
  </div>
  <div class="q-pa-md">
    <div class="input-group">
      <label>Username:</label>
      <q-input class="custom-input" v-model="username" outlined :dense="dense"/>
    </div>
    <div class="input-group">
      <label>Password:</label>
      <q-input class="custom-input" v-model="password" outlined :dense="dense"/>
    </div>
    <q-btn class="login-button" color="primary" label="Log in"/>
  </div>
</template>

<script setup lang="ts">
import {ref} from 'vue'

const username = ref('');
const password = ref('');
const dense = ref(true);
</script>

<style scoped>
.image-container {
  text-align: center;
  margin-top: 25vh;
}

.q-pa-md {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
}

.custom-input {
  width: 25vw;
}

.input-group {
  display: flex;
  flex-direction: column;
  margin-bottom: 1em;
}

label {
  margin-bottom: 0.5em;
}

.login-button {
  width: 10vw;
}
</style>
