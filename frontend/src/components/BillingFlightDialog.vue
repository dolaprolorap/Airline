<script setup lang='ts'>

import { ref } from 'vue';

const props = defineProps<{
  amount: number,
  active: boolean
}>();

defineEmits<{
  'update:active': [payload: boolean]
}>();

const payMethod = ref('');

const clearDialog = () => {
  payMethod.value = 'creditCard';
};

const submit = () => {
  window.location.reload();
};

</script>

<template>
  <q-dialog :model-value='active'
            @update:model-value='$emit("update:active", $event.value)'
            @hide='clearDialog'
  >
    <q-card>
      <q-card-section class='fit row items-end justify-end text-h3'>
        Confirm billing
      </q-card-section>
      <q-splitter horizontal />
      <q-card-section class='fit row items-end justify-between'>
        <q-input :model-value='`${amount} $`' readonly label='Total price'/>
        <span class='text-primary text-h6 q-pb-md'>Total price</span>
      </q-card-section>
      <q-splitter horizontal />
      <q-card-section class='fit column items-end'>
        <span class='text-primary text-h6 q-pb-md'>Payment method</span>
        <q-radio left-label v-model='payMethod' val='creditCard' label='Credit Card' />
        <q-radio left-label v-model='payMethod' val='cash' label='Cash' />
        <q-radio left-label v-model='payMethod' val='voucher' label='Voucher' />
      </q-card-section>
      <q-splitter horizontal />
      <q-card-actions align='around' class='fit text-secondary q-pa-md'>
        <q-btn class='bg-primary text-white tex-gyre-adventor-bold' label='Cancel' v-close-popup />
        <q-btn class='bg-primary text-white tex-gyre-adventor-bold' label='Submit' v-close-popup
               @click='submit' />
      </q-card-actions>
    </q-card>
  </q-dialog>
</template>

<style scoped lang='sass'>

</style>