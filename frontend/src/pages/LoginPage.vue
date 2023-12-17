<script setup lang='ts'>
import { ref } from 'vue';
import { api } from 'src/boot/axios';
import { useQuasar } from 'quasar';

import { LocalStorage } from 'quasar';
import { authGet } from 'src/utils';

import { Router } from 'src/router';

authGet('/Auth/GetMyself')
  .then(response => {
    const role = response.data.data.user.roleName;
    if (role === 'Administrator')
      Router.replace({ path: '/admin' });
    if (role === 'User')
      Router.replace({ path: '/user' });
  });

const email = ref('');
const password = ref('');

const isPwd = ref(true);

const error = ref('');

const $q = useQuasar();

const usernameRules = [
  (val?: string) => (val && val.length > 0) || 'Please enter username'
];
const passwordRules = [
  (val?: string) => (val && val.length > 0) || 'Please enter password'
];

const submitForm = () => {
  error.value = '';

  const formData = {
    email: email.value,
    password: password.value
  };

  if (email.value === '' || password.value === '') {
    return;
  }

  api.post('/Auth/Login', formData)
    .then(response => {

      const msg = response.data.msg;

      if (msg === 'UserNotFound') {
        error.value = 'Wrong username or password';
        return;
      }

      const accessToken = response.data.data.accessToken;
      const refreshToken = response.data.data.refreshToken;

      LocalStorage.set('accessToken', accessToken);
      LocalStorage.set('refreshToken', refreshToken);
      LocalStorage.set('email', email);

      api.defaults.headers.common['Authorization'] = `Bearer ${accessToken}`;

      $q.notify({
        message: 'Successful log in!',
        color: 'green-4',
        textColor: 'white',
        position: 'top',
        timeout: 1000
      });
      authGet('/Auth/GetMyself')
        .then(response => {
          const role = response.data.data.user.roleName;
          console.log(role)

          if (role === 'Administrator')
            Router.push({ path: '/admin' });
          if (role === 'User')
            Router.push({ path: '/user' });

          LocalStorage.set('roleName', role);
        });
    });
};

const closeBanner = () => {
  error.value = '';
  return;
};

function exit() {
  return;
}

</script>
<template>
  <q-page class='row items-center justify-center'>
    <q-card
      class='q-pa-lg full-width column justify-center items-center'
      style='max-width: 25%'
    >
      <q-img src='logo.png' alt='Logo' />
      <q-card-section class='full-width column' style='row-gap: 16px'>
        <q-input
          label='Email'
          class='fit'
          v-model='email'
          outlined
          dense
          lazy-rules
          :rules='usernameRules'
        />
        <q-input
          v-model='password'
          class='fit'
          dense
          outlined
          lazy-rules
          :rules='passwordRules'
          :type="isPwd ? 'password' : 'text'"
          label='Password'
        >
          <template v-slot:append>
            <q-icon
              :name="isPwd ? 'visibility_off' : 'visibility'"
              class='cursor-pointer'
              @click='isPwd = !isPwd'
            />
          </template>
        </q-input>
      </q-card-section>
      <q-banner v-if='error !== ""' class='bg-red-1 text-white' style='border-radius: 10px'>
        <q-card-section
          @submit.prevent='submitForm'
          v-if='error !== ""'
          class='row full-width justify-center text-negative'>
          {{ error }}
          <q-btn flat size='0px'
                 style='position: absolute; top: -8px; right: -15px; padding: 1px; border: 0px solid transparent;'
                 @click='closeBanner'>
            <img src='public/black_crosss.png' style='width: 24px; height: 24px'>
          </q-btn>
        </q-card-section>
      </q-banner>
      <q-card-section class='row full-width justify-between q-px-xl'>
        <q-btn @click='submitForm' class='col-4' color='primary' label='Log in' />
        <q-btn @click='exit' class='col-4' color='primary' label='Exit' />
      </q-card-section>
    </q-card>
  </q-page>
</template>

<style scoped lang='sass'></style>
