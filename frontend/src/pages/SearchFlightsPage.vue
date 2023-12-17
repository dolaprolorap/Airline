<script setup lang='ts'>
import { Ref, ref } from 'vue';
import { authGet, authPost } from 'src/utils';
import { IRow } from 'components/FlightTable.vue';
import FlightTable from 'components/FlightTable.vue';

interface Airport {
  country: string,
  iataCode: string,
  name: string,
  label: string
}

const airportOptions: Ref<Airport[]> = ref([]);

const airportFrom = ref(airportOptions.value[0]);
const airportTo = ref(airportOptions.value[0]);

authGet('/Airport/Get')
  .then(response => {
    airportOptions.value = response.data.data.map((airport: Airport) => {
      return { ...airport, label: `${airport.name} (${airport.iataCode})` };
    });
    airportFrom.value = airportOptions.value.find(elem => elem.iataCode === 'DOH')
      || airportOptions.value[0];
    airportTo.value = airportOptions.value.find(elem => elem.iataCode === 'CAI')
      || airportOptions.value[0];
  });

const cabinOptions: Ref<string[]> = ref([]);

const cabinType = ref('');

authGet('/CabinType/Get')
  .then(response => {
    cabinOptions.value = response.data.data;
    cabinType.value = cabinOptions.value[0];
  });


const withReturn = ref(0);

const today = new Date();
const dateString = `${today.getFullYear()}-${today.getMonth()}-${today.getDate()}`;

const outboundDate = ref('2017-12-18');
const returnDate = ref('2017-12-18');

interface FlightGroup {
  numberOfStops: number,
  flights: { date: string, time: string, flightNumber: string, economyPrice: number }[]
}

const parseFlights = (flightGroups: FlightGroup[]) => {
  return flightGroups.map(flightGroup => {
    return {
      numberOfStops: flightGroup.numberOfStops,
      from: airportFrom.value.label,
      to: airportTo.value.label,
      date: flightGroup.flights[0].date,
      time: flightGroup.flights[0].time,
      flightIDs: `[ ${flightGroup.flights[0].flightNumber} ]`
        .concat(flightGroup.flights.slice(1).reduce(
          (acc: string, elem) => `${acc} - [ ${elem.flightNumber} ]`,
          ''
        )),
      basePrice: flightGroup.flights.reduce(
        (acc: number, elem) => acc + elem.economyPrice,
        0
      )
    };
  });
};

const search = () => {
  authPost('/Book/SearchFlights', {
    fromCode: airportFrom.value.iataCode,
    toCode: airportTo.value.iataCode,
    cabinType: cabinType.value,
    withReturn: Boolean(withReturn.value),
    outboundDate: outboundDate.value,
    returnDate: returnDate.value
  })
    .then(response => {
      if (response.data.data.forwardManyFlights)
        rowsOutbound.value = parseFlights(response.data.data.forwardManyFlights);

      if (response.data.data.returnManyFlights)
        rowsReturn.value = parseFlights(response.data.data.returnManyFlights);
    });
};

const isRangeOutbound = ref(false);
const isRangeReturn = ref(false);

const rowsOutbound: Ref<IRow[]> = ref([]);
const rowsReturn: Ref<IRow[]> = ref([]);

const selectedOutbound: Ref<IRow[]> = ref([]);
const selectedReturn: Ref<IRow[]> = ref([]);

const isLoading = ref(false);

const getSelectedRowsText = (amountOfRows: number) => `${amountOfRows} flights selected.`;

</script>

<template>
  <q-page class='row justify-center items-center'>
    <q-card class='column fit' style='max-width: 60%'>
      <q-card-section class='row items-end justify-end text-h3'>
        Book flight
      </q-card-section>
      <q-splitter horizontal />
      <q-card-section class='column items-end' style='row-gap: 16px'>
        <span class='text-primary text-h6 absolute'> Search parameters </span>
        <div class='fit row items-end justify-around'>
          <div class='column'>
            <q-select v-model='airportFrom' :options='airportOptions' label='From' />
            <q-select v-model='airportTo' :options='airportOptions' label='To' />
          </div>
          <q-select style='min-width: 7rem' v-model='cabinType' :options='cabinOptions' label='Cabin type' />
          <div class='column self-center'>
            <span class='q-pa-sm text-primary'> Ticket type </span>
            <q-radio v-model='withReturn' :val=1 label='Return' />
            <q-radio v-model='withReturn' :val=0 label='One way' />
          </div>
          <div class='column'>
            <q-input v-model='outboundDate' label='Outbound' type='date' />
            <q-input v-model='returnDate' label='Return' type='date' :disable='!withReturn' />
          </div>
          <q-btn icon-right='search' @click='search' class='bg-primary text-white tex-gyre-adventor-bold'
                 label='Search' />
        </div>
      </q-card-section>
      <q-splitter horizontal />
      <q-card-section class='column items-between' style='row-gap: 16px'>
        <div class='q-pb-xs row items-end justify-between'>
          <q-checkbox v-model='isRangeOutbound' label='Display three days range' />
          <span class='text-primary text-h6'> Outbound options </span>
        </div>
        <FlightTable
          :is-loading='isLoading'
          :rows='rowsOutbound'
          :cabin-type='cabinType'
          v-model:selected='selectedOutbound'
        />
      </q-card-section>
      <q-splitter v-if='withReturn' horizontal />
      <q-card-section v-if='withReturn' class='column items-between' style='row-gap: 16px'>
        <div class='q-pb-xs row items-end justify-between'>
          <q-checkbox v-model='isRangeReturn' label='Display three days range' />
          <span class='text-primary text-h6'> Return options </span>
        </div>
        <FlightTable
          :is-loading='isLoading'
          :rows='rowsReturn'
          :cabin-type='cabinType'
          v-model:selected='selectedReturn'
        />
      </q-card-section>
    </q-card>
  </q-page>
</template>

<style lang='sass'>
thead tr th
  position: sticky
  z-index: 1

thead tr th
  top: 0
</style>
