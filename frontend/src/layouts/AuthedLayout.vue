<script setup lang="ts">
import { exit } from 'src/utils';
import { LocalStorage } from 'quasar';

const roleName = LocalStorage.getItem('roleName');
</script>

<template>
  <q-layout view="hHh Lpr fFf">
    <!-- Be sure to play with the Layout demo on docs -->
    <!-- (Optional) The Header -->
    <q-header elevated>
      <q-toolbar>
        <q-toolbar-title>
          <q-btn flat :to="roleName === 'Administrator' ? '/admin' : '/user'">
            <q-img src="logo_white.png" fit="scale-down" width="170px" />
          </q-btn>
          <q-btn style="font-size: large" flat v-if="roleName==='Administrator'" :to="'/admin'">
            Admin panel
          </q-btn>
          <q-btn style="font-size: large" flat :to="'/user'"> User panel</q-btn>
          <q-btn style="font-size: large" flat :to="'/flight-manager'"> flight schedule</q-btn>
          <q-btn style="font-size: large" flat :to="'/book-flights'"> book flight </q-btn>
          <q-btn style="font-size: large" flat :to="'/survey'"> Survey </q-btn>
        </q-toolbar-title>
        <q-btn
          round
          class="q-mr-lg"
          color="white"
          text-color="primary"
          icon="logout"
          to="/"
          @click="exit"
        />
      </q-toolbar>
    </q-header>

    <q-page-container>
      <!-- This is where pages get injected -->
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<style scoped lang='sass'>
</style>