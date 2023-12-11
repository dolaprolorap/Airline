<script setup lang='ts'>
import { ref } from 'vue';

const office = ref('All offices');

const officeOptions = ref([
  'All offices',
  'Abu Dhabi',
  'Bahrain',
  'Doha'
])

</script>
<template>
  <q-page class='row items-center justify-center'>
    <q-card>
      <q-card-section>
        <q-select filled v-model='office' :options='officeOptions' label='Filled' />
      </q-card-section>
    </q-card>
  </q-page>
</template>

<style scoped lang='sass'>

</style>
