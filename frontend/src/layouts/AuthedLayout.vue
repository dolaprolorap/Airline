<script setup lang='ts'>
import { exit } from 'src/utils';


</script>

<template>
  <q-layout view='hHh Lpr fFf'> <!-- Be sure to play with the Layout demo on docs -->
    <!-- (Optional) The Header -->
    <q-header elevated>
      <q-toolbar>
        <q-toolbar-title>
          <q-btn flat to='/'>
            <q-img src='logo_white.png' fit='scale-down' width='170px' />
          </q-btn>
        </q-toolbar-title>
        <q-btn
          round
          color='white'
          text-color='primary'
          icon='logout'
          to='/'
          @click='exit'
        />
      </q-toolbar>
    </q-header>

    <q-page-container>
      <!-- This is where pages get injected -->
      <router-view />
    </q-page-container>

  </q-layout>
</template>

