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

      if (msg === 'UserNotFound' || msg === 'InvalidPwd') {
        error.value = 'Wrong username or password';
        return;
      }

      if (msg === 'UnactiveUser') {
        error.value = 'User unactive';
        return;
      }

      const accessToken = response.data.data.accessToken;
      const refreshToken = response.data.data.refreshToken;

      LocalStorage.set('accessToken', accessToken);
      LocalStorage.set('refreshToken', refreshToken);
      LocalStorage.set('email', email.value);

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
          console.log(role);

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
  window.close();
  return;
}

</script>
<template>
  <q-page class='column justify-center items-center'>
    <q-card
      class='q-px-lg q-pt-lg full-width column justify-center items-center'
      style='max-width: 25%'
    >
      <q-img src='logo.png' alt='Logo' />
      <q-form class='fit column' @submit='submitForm'>
        <q-card-section class='q-pb-none'>
          <q-input
            label='Email'
            v-model='email'
            outlined
            dense
            lazy-rules
            :rules='usernameRules'
          />
          <q-input
            v-model='password'
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
        <q-card-section class='q-py-none'>
          <div>
            <q-banner
              v-if='error !== ""'
              class='relative fit bg-red-2 text-negative row'
              style='border-radius: 10px'
              @submit.prevent='submitForm'
            >
              <div class='row justify-center'>
                {{ error }}
              </div>
              <q-btn round flat size='8px'
                     @click='closeBanner'
                     class='q-mt-sm q-mr-lg absolute-top-right'
                     text-color='negative'
                     icon='close'
              />
            </q-banner>
          </div>
          <div class='row full-width justify-between q-pt-md q-px-md'>
            <q-btn class='col-4' color='primary' label='Log in' type='submit' />
            <q-btn @click='exit' class='col-4' color='primary' label='Exit' />
          </div>
        </q-card-section>
      </q-form>
    </q-card>
  </q-page>
</template>

<style scoped lang='sass'></style>
