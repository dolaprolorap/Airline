<template>
  <q-page class="row items-center justify-center">
    <q-card
        class="q-pa-lg full-width column justify-center items-center"
        style="max-width: 25%"
    >
      <q-img src="logo.png" alt="Logo"/>
      <q-card-section class="full-width column" style="row-gap: 16px">
        <q-input
            ref="usernameRef"
            label="Username"
            class="fit"
            v-model="username"
            outlined
            dense
            lazy-rules
            :rules="usernameRules"
        />
        <q-input
            ref="ageRef"
            label="Password"
            class="fit"
            v-model="password"
            outlined
            dense
            lazy-rules
            :rules="passwordRules"
        />
      </q-card-section>
      <q-card-section class="row full-width justify-between q-px-xl">
        <q-btn @click="SubmitForm" class="col-4" color="primary" label="Log in"/>
        <q-btn @click="Exit" class="col-4" color="primary" label="Exit"/>
      </q-card-section>
    </q-card>
  </q-page>
</template>

<script setup lang="ts">
import {ref} from 'vue'
import axios from 'axios'

const username = ref(null)
const usernameRef = ref(null)
const password = ref(null)
const passwordRef = ref(null)

const usernameRules = [
  (val?: string) => (val && val.length > 0) || 'Please enter username'
]
const passwordRules = [
  (val?: string) => (val && val.length > 0) || 'Please enter password',
]

function SubmitForm() {
  const formData = {
    username: username,
    password: password
  }
  axios.post("/api/Auth/Login", formData)
      .then(response => {
        const msg = response.data.msg;
        const accessToken = response.data.data.accessToken;
        const refreshToken = response.data.data.refreshToken;

      })
}

function Exit() {
  return;
}

</script>

<style scoped lang="sass"></style>
