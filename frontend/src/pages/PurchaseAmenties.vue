<script setup lang="ts">
import { ref } from 'vue';

const bookingRef = ref('');

const firstAvailibleFlight = ref('');
const availibleFlightsList = ref(['dsa', 'dsa', 'dsa']);

const fullName = ref('dsadsa');
const passportNumber = ref('dsads');
const cabinClass = ref('dasd');

const submitBookingRef = () => {
  return;
};

const submitFlight = () => {
  return;
};
</script>

<template>
  <q-page class="row items-center justify-center">
    <q-card style="min-width: 1000px">
      <q-card-section class="row justify-end text-h3"
        >Purchase Amenties
      </q-card-section>
      <q-splitter horizontal></q-splitter>
      <q-card-section class="row justify-start text-h6 q-mt-sm q-mx-md"
        >
        <span>Booking reference:</span>
        <q-input
          class="q-mx-lg"
          v-model="bookingRef"
          dense
          clearable
          style="width: 20%"
        />
        <q-btn
          class="tex-gyre-adventor-bold col-2 q-mx-md"
          style="font-size: medium; width: 10%"
          @click="submitBookingRef"
          color="primary"
          >Submit
        </q-btn>
      </q-card-section>
      <q-separator class="q-mt-sm" inset />
      <q-card-section class="row justify-start text-h6 q-mt-sm q-mx-md">
        <span>Choose your flights:</span>
        <q-select
          class="q-mx-lg q-mt-none"
          v-model="firstAvailibleFlight"
          :options="availibleFlightsList"
          label="Airport list"
          dense
        />
        <q-btn
          class="tex-gyre-adventor-bold col-2 q-mx-md"
          style="font-size: medium; width: 20%"
          @click="submitFlight"
          color="primary"
          >Show Amenies
        </q-btn>
      </q-card-section>
      <q-separator class="q-mt-sm" inset />
      <q-card-section class="column justify-start text-h6 q-mx-md">
        <q-card-section class="row">
          <span>Full name:</span>
          <q-input
            class="q-mx-lg"
            v-model="fullName"
            dense
            readonly
            style="width: 30%"
          />
        </q-card-section>
        <q-card-section class="row">
          <span>Passport number:</span>
          <q-input
            class="q-mx-lg"
            v-model="passportNumber"
            dense
            readonly
            style="width: 30%"
          />
        </q-card-section>
        <q-card-section class="row">
          <span>Your cabin class is:</span>
          <q-input
            class="q-mx-lg"
            v-model="cabinClass"
            dense
            readonly
            style="width: 30%"
          />
        </q-card-section>
      </q-card-section>
      <q-separator class="q-mt-md" inset />
      <q-card-section class="row">
        <q-card-section></q-card-section>
      </q-card-section>
    </q-card>
  </q-page>
</template>

<style lang="sass"></style>
