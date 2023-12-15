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
            ref="passwordRef"
            v-model="password"
            class="fit"
            dense
            outlined
            lazy-rules
            :rules="passwordRules"
            :type="isPwd ? 'password' : 'text'"
            label="Password"
        >
          <template v-slot:append>
            <q-icon
                :name="isPwd ? 'visibility_off' : 'visibility'"
                class="cursor-pointer"
                @click="isPwd = !isPwd"
            />
          </template>
        </q-input>
      </q-card-section>
      <q-card-section
          @submit.prevent="SubmitForm"
          v-if="error.length !== 0"
          class="row full-width justify-center text-negative">
        {{ error }}
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
import {api} from 'src/boot/axios'
import {useRouter} from "vue-router";
import {useAuthStore} from "../stores/store";

const username = ref(null)
const usernameRef = ref(null)
const password = ref(null)
const passwordRef = ref(null)
const authStore = useAuthStore();
const isPwd = ref(true);

const error = ref('');

const router = useRouter();

const usernameRules = [
  (val?: string) => (val && val.length > 0) || 'Please enter username'
]
const passwordRules = [
  (val?: string) => (val && val.length > 0) || 'Please enter password',
]

function SubmitForm() {

  error.value = '';

  const formData = {
    email: username.value,
    password: password.value
  }

  if (username.value === null || password.value === null)
    error.value = 'Wrong username or password';

  api.post("/Auth/Login", formData)
      .then(response => {

        const msg = response.data.msg;

        if (msg === 'UserNotFound') {
          error.value = 'Wrong username or password';
        }

        const accessToken = response.data.data.accessToken;
        const refreshToken = response.data.data.refreshToken;

        authStore.SetTokens({accessToken, refreshToken});
        authStore.SetUsername(username.value);

        api.defaults.headers.common['Authorization'] = `Bearer ${accessToken}`;


        api.get('/Auth/GetMyself')
            .then(response => {
              const role = response.data.data.user.roleName;
              if (role === 'Administrator')
                router.push({path: '/admin'});
              if (role === 'User')
                router.push({path: '/user'});
            })
      })
}

function Exit() {
  return;
}

</script>

<style scoped lang="sass"></style>
