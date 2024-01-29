<script setup lang='ts'>
import { ref } from 'vue';
import PurchaseAmenties from "components/PurchaseAmenties.vue";
import ReportAmenties from "components/ReportAmenties.vue";

const tab = ref('purchase');
</script>

<template>
  <q-page class='q-pt-md column justify-start items-center' style=''>
    <q-tabs
      v-model='tab'
      class='text-primary'
    >
      <q-tab name='purchase' icon='description' label='purchase' />
      <q-tab name='report' icon='feed' label='report' />
    </q-tabs>
    <q-tab-panels v-model='tab' animated >
      <q-tab-panel name='purchase'>
        <PurchaseAmenties />
      </q-tab-panel>
      <q-tab-panel name='report'>
        <ReportAmenties />
      </q-tab-panel>
    </q-tab-panels>
  </q-page>
</template>
