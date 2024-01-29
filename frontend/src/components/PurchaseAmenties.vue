<script setup lang='ts'>
import { onMounted, ref, Ref } from 'vue';
import { authGet, authPost } from 'src/utils';

const bookingRef = ref('');

const availibleFlightsList = ref([]);
const firstFlight = ref('');

const fullName = ref('');
const cabinClass = ref('');

const flightsArray = ref<
  Array<{
    id: string;
    flightNumber: string;
    fromAirportName: string;
    toAirportName: string;
    date: string;
    time: string;
    firstname: string;
    lastname: string;
    phone: string;
    cabinTypeName: string;
  }>
>([]);

const serviceNames = ref<
  Array<{
    id: string;
    service: string;
    price: string;
  }>
>([]);

const id1 = ref(0);
const submitBookingRef = () => {
  authGet(`/Amenities/GetTicketsByBookRef?bookRef=${bookingRef.value}`).then(
    (response) => {
      const tableArray = response.data.data;
      flightsArray.value = tableArray;
      tableArray.forEach((tuple: any) => {
        const str =
          tuple.id +
          ', ' +
          tuple.flightNumber +
          ', ' +
          tuple.fromAirportName +
          '-' +
          tuple.toAirportName +
          ', ' +
          tuple.date +
          ', ' +
          tuple.time;
        availibleFlightsList.value.push(str);
      });
      firstFlight.value = availibleFlightsList.value[0];
    }
  );
};
const submitFlight = () => {
  flightsArray.value.forEach((tuple: any) => {
    id1.value = Number(tuple.id);
    if (tuple.id === id1.value) {
      fullName.value = tuple.firstname + ' ' + tuple.lastname;
      cabinClass.value = tuple.cabinTypeName;
    }
  });
};

const itemsSelected = ref(0);
const totalPayable = ref(0);

onMounted(async () => {
  await authGet('/Amenities/GetAmenities').then((response) => {
    const tableArray = response.data.data;
    tableArray.forEach((tuple: any) => {
      serviceNames.value.push({
        id: tuple.id,
        service: tuple.service,
        price: tuple.price
      });
    });
  });
});

const extra = ref(false);
const next = ref(false);
const neighbour = ref(false);
const tablet = ref(false);
const laptop = ref(false);
const lounge = ref(false);
const soft = ref(false);
const premium = ref(false);
const extraBag = ref(false);
const fast = ref(false);
const mb50 = ref(false);
const mb250 = ref(false);

const checkBoxArray: Ref<boolean>[] = [];
checkBoxArray.push(
  extra,
  next,
  neighbour,
  tablet,
  laptop,
  lounge,
  soft,
  premium,
  extraBag,
  fast,
  mb50,
  mb250
);

const submitChecks = () => {
  for (let i = 0; i < checkBoxArray.length; i++) {
    if (checkBoxArray[i].value) {
      itemsSelected.value += 1;
      totalPayable.value += Number(serviceNames.value[i].price);
      console.log('dsa', serviceNames.value[i].price);
    }
  }
};

const reset = () => {
  for (let i = 0; i < checkBoxArray.length; i++) {
    checkBoxArray[i].value = false;
  }
  totalPayable.value = 0;
  itemsSelected.value = 0;
};
let array = [];
const id2 = Number(id1.value);
const confirm = () => {
  if (!itemsSelected.value) {
    return;
  }
  let arrayNames = [];
  for (let i = 0; i < checkBoxArray.length; i++) {
    if (checkBoxArray[i].value) {
      arrayNames.push(serviceNames.value[i].service);
    }
  }
  const formData = {
    ticketId: id1.value,
    amenityNamesToApply: arrayNames
  };
  authPost('/Amenities/ApplyAmenities', formData);
  for (let i = 0; i < checkBoxArray.length; i++) {
    if (checkBoxArray[i].value) {
      checkBoxArray[i].value = false;
    }
  }
  itemsSelected.value = 0;
  totalPayable.value = 0;
};
</script>

<template>
  <q-card style='min-width: 1000px; max-height: 80vh; overflow: auto'>
    <q-card-section class='row justify-end text-h3'
    >Purchase Amenties
    </q-card-section>
    <q-splitter horizontal></q-splitter>
    <q-card-section class='row justify-start text-h6 q-mt-sm q-mx-lg'>
      <span>Booking reference:</span>
      <q-input
        class='q-mx-lg'
        v-model='bookingRef'
        dense
        clearable
        style='width: 20%'
      />
      <q-btn
        class='tex-gyre-adventor-bold col-2 q-mx-md'
        style='font-size: medium; width: 10%'
        @click='submitBookingRef'
        color='primary'
      >Submit
      </q-btn>
    </q-card-section>
    <q-separator class='q-mt-sm' inset />
    <q-card-section class='row justify-start text-h6 q-mt-sm q-mx-lg'>
      <span>Choose your flights:</span>
      <q-select
        class='q-mx-lg q-mt-none'
        v-model='firstFlight'
        :options='availibleFlightsList'
        label='Airport list'
        dense
      />
      <q-btn
        class='tex-gyre-adventor-bold col-2 q-mx-md'
        style='font-size: medium; width: 20%'
        @click='submitFlight'
        color='primary'
      >Show Amenties
      </q-btn>
    </q-card-section>
    <q-separator class='q-mt-sm' inset />
    <q-card-section class='row justify-between text-h6 q-mx-md'>
      <q-card-section class='row'>
        <span>Full name:</span>
        <q-input
          class='q-mx-lg'
          v-model='fullName'
          dense
          readonly
          style='width: 30%'
        />
      </q-card-section>
      <q-card-section class='row'>
        <span>Your cabin class is:</span>
        <q-input
          class='q-mx-lg'
          v-model='cabinClass'
          dense
          readonly
          style='width: 30%'
        />
      </q-card-section>
    </q-card-section>
    <q-separator class='q-mt-md' inset />
    <q-card-section class='row text-primary justify-end text-h6'>
      <span>AMONIC Airlines Amenties</span>
    </q-card-section>
    <q-card-section class='row'>
      <q-card-section class='column'>
        <q-checkbox
          v-model='extra'
          class='text-h6'
          label='Extra Blanket ($10)'
        ></q-checkbox>
        <q-checkbox
          v-model='next'
          class='text-h6'
          label='Next Seat Free ($30)'
        ></q-checkbox>
        <q-checkbox
          v-model='neighbour'
          class='text-h6'
          label='Two Neighboring Seats Free ($50)'
        ></q-checkbox>
        <q-checkbox
          v-model='tablet'
          class='text-h6'
          label='Tablet Rental ($12)'
        ></q-checkbox>
      </q-card-section>
      <q-card-section class='column'>
        <q-checkbox
          v-model='laptop'
          class='text-h6'
          label='Laptop Rental ($15)'
        ></q-checkbox>
        <q-checkbox
          v-model='lounge'
          class='text-h6'
          label='Lounge Access ($25)'
        ></q-checkbox>
        <q-checkbox
          v-model='soft'
          class='text-h6'
          label='Soft Drinks ($0)'
        ></q-checkbox>
        <q-checkbox
          v-model='premium'
          class='text-h6'
          label='Premium Headphones Rental ($5)'
        ></q-checkbox>
      </q-card-section>
      <q-card-section class='column'>
        <q-checkbox
          v-model='extraBag'
          class='text-h6'
          label='Extra Bag ($15)'
        ></q-checkbox>
        <q-checkbox
          v-model='fast'
          class='text-h6'
          label='Fast Checkin Lane ($10)'
        ></q-checkbox>
        <q-checkbox
          v-model='mb50'
          class='text-h6'
          label='Wi-Fi 50 mb ($0)'
        ></q-checkbox>
        <q-checkbox
          v-model='mb250'
          class='text-h6'
          label='Wi-Fi 250 mb ($25)'
        ></q-checkbox>
      </q-card-section>
    </q-card-section>
    <q-card-section class='row justify-between'>
      <q-btn
        class='tex-gyre-adventor-bold col-2 q-mx-md'
        style='font-size: medium; width: 30%'
        @click='reset'
        color='primary'
      >reset
      </q-btn>
      <q-btn
        class='tex-gyre-adventor-bold col-2 q-mx-md'
        style='font-size: medium; width: 30%'
        @click='submitChecks'
        color='primary'
      >submit
      </q-btn>
    </q-card-section>
    <q-separator class='q-mt-md' inset />
    <q-card-section class='row'>
      <q-card-section class='column justify-start text-h6 q-mt-sm q-mx-md'>
        <span>Items selected: {{ itemsSelected }} </span>
        <span>Total payable: ${{ totalPayable }} </span>
      </q-card-section>
    </q-card-section>
    <q-card-section class='row justify-end'>
      <q-btn
        class='tex-gyre-adventor-bold col-2 q-mx-md'
        style='font-size: medium; width: 30%'
        @click='confirm'
        color='primary'
      >Confirm
      </q-btn>
    </q-card-section>
  </q-card>
</template>

<style lang='sass'></style>
