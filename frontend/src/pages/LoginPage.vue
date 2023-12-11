<template>
  <q-page class="row items-center justify-center">
    <q-card
      class="q-pa-lg full-width column justify-center items-center"
      style="max-width: 25%"
    >
      <q-img src="logo.png" alt="Logo"/>
      <q-card-section class="full-width column" style="row-gap: 16px">
        <q-input
          label="Username"
          class="fit"
          v-model="username"
          outlined
          dense
        />
        <q-input
          label="Password"
          class="fit"
          v-model="password"
          outlined
          dense
        />
      </q-card-section>
      <q-card-section class="row full-width justify-between q-px-xl">
        <q-btn class="col-4" color="primary" label="Log in"/>
        <q-btn class="col-4" color="primary" label="Exit"/>
      </q-card-section>
    </q-card>
  </q-page>
</template>

<script setup lang="ts">
import {ref} from 'vue';

const username = ref('');
const password = ref('');
</script>

<style scoped lang="sass"></style>
