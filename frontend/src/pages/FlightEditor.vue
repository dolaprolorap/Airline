<template>
  <q-page class='row items-center justify-center' style=''
  >
    <q-card>
      <q-card-section class="row items-center q-pb-none justify-between">
        <q-card-section class="q-mx-xs">From:</q-card-section>
        <q-select v-model='airportListFrom' :options='airportListFromOptions' label='Airport list' dense outlined/>
        <q-card-section class="q-ml-xl">To:</q-card-section>
        <q-select v-model='airportListTo' :options='airportListToOptions' label='Airport list' dense outlined/>
        <q-card-section class="q-ml-xl">Flight Number:</q-card-section>
        <q-input v-model="flightNumber" outlined label="Flight number" stack-label dense/>
        <q-card-section class="q-mx-xl">
          <q-btn class='tex-gyre-adventor-bold col-2' style='font-size: medium; width: 170%; '
                 color='primary' @click="applyFilters">Apply
          </q-btn>
        </q-card-section>
        <q-card-section class="q-mx-xl">
          <q-btn class='tex-gyre-adventor-bold col-2' style='font-size: medium; width: 170%'
                 color='primary' @click="resetFilters">reset
          </q-btn>
        </q-card-section>
      </q-card-section>
      <q-card-section class='column items-start' style='row-gap: 16px'>
        <q-table
            style='max-height: 69vh'
            flat
            bordered
            separator='vertical'
            table-header-class='bg-primary'
            :rows='rows'
            :columns='columns'
            row-key='id'
            selection='multiple'
            v-model:selected='selectedFlights'
            :selected-rows-label='getSelectedRowsText'

        >
          <template v-slot:header-cell='props'>
            <q-th v-if='props.col.name !== "id"' :props='props' class='text-white tex-gyre-adventor-bold'
                  style='font-size: large;'>
              {{ props.col.label }}
            </q-th>
          </template>
          <template v-slot:body='props'>
            <q-tr :props='props' @click='props.selected = !props.selected'>
              <q-td>
                <q-checkbox v-model='props.selected' color='secondary'/>
              </q-td>
              <q-td v-for='col in visibleColumns' :key='col' :props='props' style='font-size: medium'>
                {{ props.row[col] }}
              </q-td>
            </q-tr>
          </template>
        </q-table>
        <div class='row fit justify-between no-wrap'>
          <q-btn class='tex-gyre-adventor-bold col-2' style='font-size: medium' color='primary'>Cancel flight</q-btn>
          <q-btn class='tex-gyre-adventor-bold col-2' style='font-size: medium' color='primary'>Accept flight</q-btn>
          <q-btn class='tex-gyre-adventor-bold col-2' style='font-size: medium' color='primary'>edit flight
          </q-btn>
          <q-btn class='tex-gyre-adventor-bold col-2 ' style='font-size: medium' color='primary'>Add user
          </q-btn>
        </div>
      </q-card-section>
    </q-card>
  </q-page>
</template>

<script setup lang="ts">
import {ref, onMounted} from 'vue';
import {api} from "boot/axios";

const airportListFrom = ref('Airport list');

const airportListFromOptions = ref([
  'Airport list',
  'Abu Dhabi',
  'Cairo',
  'Bahrain',
  'Aden',
  'Doha',
  'Riyadh'
])

const airportListTo = ref('Airport list');

const airportListToOptions = ref([
  'Airport list',
  'Abu Dhabi',
  'Cairo',
  'Bahrain',
  'Aden',
  'Doha',
  'Riyadh'
])

const flightNumber = ref('')

const columns = ref([
  {
    name: 'id',
    required: true,
    label: 'id',
    field: 'id',
    align: 'left',
    sortable: true
  },
  {
    name: 'date',
    required: true,
    label: 'Date',
    field: 'date',
    align: 'center',
    sortable: true
  },
  {
    name: 'time',
    required: true,
    label: 'Time',
    field: 'time',
    align: 'center',
    sortable: false
  },
  {
    name: 'from',
    required: true,
    label: 'From',
    field: 'from',
    align: 'center',
    sortable: false
  },
  {
    name: 'to',
    required: true,
    label: 'To',
    field: 'to',
    align: 'center',
    sortable: false
  },
  {
    name: 'flightnumber',
    required: true,
    label: 'Flight number',
    field: 'flightnumber',
    align: 'center',
    sortable: false
  },
  {
    name: 'aircraft',
    required: true,
    label: 'Aircraft',
    field: 'aircraft',
    align: 'left',
    sortable: false
  },
  {
    name: 'economyprice',
    required: true,
    label: 'Economy price',
    field: 'economyprice',
    align: 'center',
    sortable: true
  },
  {
    name: 'businessprice',
    required: true,
    label: 'Business price',
    field: 'businessprice',
    align: 'center',
    sortable: false
  },
  {
    name: 'firstclassprice',
    required: true,
    label: 'First class price',
    field: 'firstclassprice',
    align: 'center',
    sortable: false
  },
  {
    name: 'active',
    required: true,
    label: 'Accepted',
    field: 'active',
    align: 'center',
    sortable: true
  },
]);

const rows = ref<Array<{
  id: string;
  active: string;
  date: string;
  time: string;
  from: string;
  to: string;
  flightnumber: string;
  aircraft: string;
  economyprice: string;
  businessprice: string;
  firstclassprice: string;
}>>([]);

const copyrows = ref<Array<{
  id: string;
  active: string;
  date: string;
  time: string;
  from: string;
  to: string;
  flightnumber: string;
  aircraft: string;
  economyprice: string;
  businessprice: string;
  firstclassprice: string;
}>>([]);

const selectedFlights = ref([]);

onMounted(async () => {
  await api.get('/Schedule/GetSchedule').then(response => {
    let tableArray = response.data.data;

    let id = 0;
    tableArray.forEach((tuple: any) => {
      const date = tuple.date;
      const time = tuple.time;
      const from = tuple.fromAirportName;
      const to = tuple.toAirportName;
      const flight = tuple.flightNumber;
      const aircraft = tuple.aircraft;
      const eco = tuple.economyPrice;
      const active = tuple.isActive;

      const bus = Math.round(Number(eco) * 1.35).toString();
      const first = (Math.round((Number(bus) * 1.30))).toString();
      rows.value.push({
        id: id.toString(),
        date: date,
        time: time,
        from: from,
        to: to,
        flightnumber: flight,
        aircraft: aircraft,
        economyprice: '$' + eco,
        businessprice: '$' + bus,
        firstclassprice: '$' + first,
        active: active
      })
      copyrows.value.push({
        id: id.toString(),
        date: date,
        time: time,
        from: from,
        to: to,
        flightnumber: flight,
        aircraft: aircraft,
        economyprice: '$' + eco,
        businessprice: '$' + bus,
        firstclassprice: '$' + first,
        active: active
      })
      id += 1;
    })
  })
});

const applyFilters = () => {

  rows.value = copyrows.value.map(row => ({...row}));
  let filteredRows = rows.value.filter(row => {
    const fromFilter = airportListFrom.value !== 'Airport list' ? row.from === airportListFrom.value : true;
    const toFilter = airportListTo.value !== 'Airport list' ? row.to === airportListTo.value : true;
    const flightNumberFilter = flightNumber.value !== '' ? row.flightnumber === flightNumber.value : true;

    return fromFilter && toFilter && flightNumberFilter;
  });
  rows.value = filteredRows;
};

const resetFilters = () => {
  airportListFrom.value = 'Airport list';
  airportListTo.value = 'Airport list';
  flightNumber.value = '';
  applyFilters();
};

const visibleColumns = ref([
  'date',
  'time',
  'from',
  'to',
  'flightnumber',
  'aircraft',
  'economyprice',
  'businessprice',
  'firstclassprice',
  'active'
]);

const getSelectedRowsText = (amountOfRows: number) => `${amountOfRows} users selected.`;


</script>

<style lang="sass" scoped>
thead tr th
  position: sticky
  z-index: 1

thead tr th
  top: 0
</style>