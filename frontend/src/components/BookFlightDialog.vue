<script setup lang='ts'>
import { IRow } from 'components/FlightTable.vue';
import { computed, Ref, ref } from 'vue';
import { QTableColumn } from 'quasar';
import BillingFlightDialog from 'components/BillingFlightDialog.vue';

const props = defineProps<{
  active: boolean,
  selectedOutbound: IRow[],
  selectedReturn: IRow[],
  cabinType: string
}>();

defineEmits<{
  'update:active': [payload: boolean]
}>();

const firstName = ref('');
const lastName = ref('');
const birthdate = ref('');
const idNumber = ref(0);
const idCountry = ref('Russia');
const phone = ref('');

const amount = computed(() => {
  return (props.selectedOutbound.reduce((acc, elem) => elem.basePrice + acc, 0) +
    props.selectedReturn.reduce((acc, elem) => elem.basePrice + acc, 0)) *
    passengers.value.length;
});

const clearForm = () => {
  firstName.value = '';
  lastName.value = '';
  birthdate.value = '';
  idNumber.value = 0;
  idCountry.value = '';
  phone.value = '';
};
const clearDialog = () => {
  clearForm();
  passengers.value = [];
};

interface Passenger {
  firstName: string,
  lastName: string,
  birthdate: string,
  idNumber: number,
  idCountry: string,
  phone: string
}

const passengers: Ref<Passenger[]> = ref([]);

const addPassenger = () => {
  passengers.value = [
    ...passengers.value,
    {
      firstName: firstName.value,
      lastName: lastName.value,
      birthdate: birthdate.value,
      idNumber: idNumber.value,
      idCountry: idCountry.value,
      phone: phone.value
    }
  ];
  clearForm();
};

const columns: QTableColumn[] = [
  {
    name: 'firstName',
    field: 'firstName',
    label: 'First name',
    align: 'center'
  },

  {
    name: 'lastName',
    field: 'lastName',
    label: 'Last name',
    align: 'center'
  },
  {
    name: 'birthdate',
    field: 'birthdate',
    label: 'Birthdate',
    align: 'center'
  },
  {
    name: 'idNumber',
    field: 'idNumber',
    label: 'ID Number',
    align: 'center'
  },
  {
    name: 'idCountry',
    field: 'idCountry',
    label: 'ID Country',
    align: 'center'
  },
  {
    name: 'phone',
    field: 'phone',
    label: 'Phone',
    align: 'center'
  }
];

const selectedPassenger: Ref<Passenger[]> = ref([]);

const removePassenger = () => {
  passengers.value = passengers.value.filter(
    passenger =>
      passenger.idNumber !== selectedPassenger.value[0].idNumber
  );
};

const billingDialog = ref(false);
</script>

<template>
  <q-dialog :model-value='active'
            @update:model-value='$emit("update:active", $event.value)'
            @hide='clearDialog'
  >
    <q-card class='column items-center no-wrap' style='min-width: 40%'>
      <q-card-section class='fit row items-end justify-end text-h3'>
        Confirm booking
      </q-card-section>
      <q-splitter class='fit' v-if='selectedReturn.length !== 0' horizontal />
      <q-card-section class='fit column items-end'>
        <span class='text-primary text-h6'>Outbound flight</span>
        <div class='fit row justify-around' v-for='row in selectedOutbound' :key='row.id'>
          <div>
            <q-input dense :model-value='row.from' readonly label='From' />
            <q-input dense :model-value='row.to' readonly label='To' />
          </div>
          <q-input dense :model-value='cabinType' readonly label='Cabin type' />
          <div>
            <q-input dense :model-value='row.date' readonly label='Date' type='date' />
            <q-input dense :model-value='row.flightIDs' readonly label='Flight numbers' />
          </div>
        </div>
      </q-card-section>
      <q-splitter class='fit' v-if='selectedReturn.length !== 0' horizontal />
      <q-card-section v-if='selectedReturn.length !== 0' class='fit column items-end'>
        <span class='text-primary text-h6'>Return flight</span>
        <div class='fit row justify-around' v-for='row in selectedReturn' :key='row.id'>
          <div>
            <q-input dense :model-value='row.from' readonly label='From' />
            <q-input dense :model-value='row.to' readonly label='To' />
          </div>
          <q-input dense :model-value='cabinType' readonly label='Cabin type' />
          <div>
            <q-input dense :model-value='row.date' readonly label='Date' type='date' />
            <q-input dense :model-value='row.flightIDs' readonly label='Flight numbers' />
          </div>
        </div>
      </q-card-section>
      <q-splitter class='fit' horizontal />
      <q-card-section class='fit column items-end'>
        <span class='text-primary text-h6 q-pb-md'>Passenger details</span>
        <q-form class='fit row justify-around'>
          <div>
            <q-input dense v-model='firstName' label='Firstname' />
            <q-input dense v-model='lastName' label='Lastname' />
          </div>
          <div>
            <q-input dense v-model='idNumber' label='ID Number' mask='#######' />
            <q-input dense v-model='idCountry' label='ID Country' />
          </div>
          <div>
            <q-input dense v-model='birthdate' label='Birthday' type='date' />
            <q-input dense v-model='phone' label='Phone' />
          </div>
        </q-form>
        <q-btn class='q-mt-md bg-primary text-white tex-gyre-adventor-bold' label='Add passenger'
               @click='addPassenger'
        />
      </q-card-section>
      <q-splitter class='fit' horizontal />
      <q-card-section class='fit column'>
        <q-table
          dense
          style='max-height: 30vh'
          flat
          bordered
          separator='vertical'
          table-header-class='bg-primary'
          :rows='passengers'
          :columns='columns'
          row-key='idNumber'
          hide-bottom
          selection='single'
          v-model:selected='selectedPassenger'
        >
          <template v-slot:header-cell='props'>
            <q-th :props='props' class='text-white tex-gyre-adventor-bold'
                  style='font-size: large;'>
              {{ props.col.label }}
            </q-th>
          </template>
          <template v-slot:body='props'>
            <q-tr :props='props' @click='props.selected = !props.selected'>
              <q-td>
                <q-checkbox v-model='props.selected' color='secondary' />
              </q-td>
              <q-td v-for='col in props.cols' :key='col.name' :props='props' style='font-size: medium'>
                {{ col.value }}
              </q-td>
            </q-tr>
          </template>
        </q-table>
        <q-btn class='self-end q-mt-md bg-red text-white tex-gyre-adventor-bold'
               label='Remove passenger'
               @click='removePassenger'
        />
      </q-card-section>
      <q-splitter horizontal class='fit' />
      <q-card-actions align='around' class='fit text-secondary q-pa-md'>
        <q-btn class='bg-primary text-white tex-gyre-adventor-bold' label='Cancel' v-close-popup />
        <q-btn class='bg-primary text-white tex-gyre-adventor-bold' label='Submit'
               @click='billingDialog = true'
               :disable='passengers.length === 0'
        />
      </q-card-actions>
    </q-card>
  </q-dialog>
  <billing-flight-dialog :amount='amount' v-model:active='billingDialog' />
</template>

<style scoped lang='sass'>

</style>