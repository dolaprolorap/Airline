<script setup lang='ts'>
import { ref } from 'vue';
import SummarySurvey from 'components/SummarySurvey.vue';
import DetailedSurvey from 'components/DetailedSurvey.vue';

const tab = ref('summary');
</script>

<template>
  <q-page class='q-pt-md column justify-start items-center' style=''>
    <q-tabs
      v-model='tab'
      class='text-primary'
    >
      <q-tab name='summary' icon='description' label='Summary' />
      <q-tab name='detailed' icon='feed' label='Details' />
    </q-tabs>
    <q-tab-panels v-model='tab' animated keep-alive>
      <q-tab-panel name='summary'>
        <SummarySurvey />
      </q-tab-panel>
      <q-tab-panel name='detailed'>
        <DetailedSurvey />
      </q-tab-panel>
    </q-tab-panels>
  </q-page>
</template>
