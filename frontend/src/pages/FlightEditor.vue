<template>

  <q-page class="row items-center justify-center" style="">
    <q-card>
      <q-card-section class="row justify-end text-h3">Flight Schedule</q-card-section>
      <q-splitter horizontal></q-splitter>
      <q-card-section class="row items-center q-pb-none justify-between">
        <q-card-section class="q-mx-xs">From:</q-card-section>
        <q-select
          v-model="airportListFrom"
          :options="airportListFromOptions"
          label="Airport list"
          dense
          outlined
        />
        <q-card-section class="q-ml-xl">To:</q-card-section>
        <q-select
          v-model="airportListTo"
          :options="airportListToOptions"
          label="Airport list"
          dense
          outlined
        />
        <q-card-section class="q-ml-xl">Flight Number:</q-card-section>
        <q-input
          v-model="flightNumber"
          outlined
          label="Flight number"
          stack-label
          dense
        />
        <q-card-section class="q-mx-xl">
          <q-btn
            class="tex-gyre-adventor-bold col-2"
            style="font-size: medium; width: 170%"
            color="primary"
            @click="applyFilters"
            >Apply
          </q-btn>
        </q-card-section>
        <q-card-section class="q-mx-xl">
          <q-btn
            class="tex-gyre-adventor-bold col-2"
            style="font-size: medium; width: 170%"
            color="primary"
            @click="resetFilters"
            >reset
          </q-btn>
        </q-card-section>
      </q-card-section>
      <q-card-section class="column items-start" style="row-gap: 16px">
        <q-table
          style="max-height: 69vh"
          flat
          bordered
          separator="vertical"
          table-header-class="bg-primary"
          :rows="rows"
          :columns="columns"
          row-key="id"
          selection="multiple"
          v-model:selected="selectedFlights"
          :selected-rows-label="getSelectedRowsText"
        >
          <template v-slot:header-cell="props">
            <q-th
              v-if="props.col.name !== 'id'"
              :props="props"
              class="text-white tex-gyre-adventor-bold"
              style="font-size: large"
            >
              {{ props.col.label }}
            </q-th>
          </template>
          <template v-slot:body="props">
            <q-tr :props="props" @click="props.selected = !props.selected" v-if="props.row.active === true">
              <q-td>
                <q-checkbox v-model="props.selected" color="secondary" />
              </q-td>
              <q-td
                v-for="col in visibleColumns"
                :key="col"
                :props="props"
                style="font-size: medium"
              >
                {{ props.row[col] }}
              </q-td>
            </q-tr>
            <q-tr v-else>
              <q-td class="bg-red text-white">
                <q-checkbox v-model="props.selected" color="secondary" />
              </q-td>
              <q-td
                v-for="col in visibleColumns"
                :key="col"
                :props="props"
                style="font-size: medium"
                class="bg-red text-white"
              >
                {{ props.row[col] }}
              </q-td>
            </q-tr>
          </template>
        </q-table>
        <div class="row fit justify-between no-wrap">
          <q-btn
            @click="cancelFlight"
            class="tex-gyre-adventor-bold col-2"
            style="font-size: medium"
            color="primary"
          >
            Cancel flight
          </q-btn>
          <q-btn
            @click="acceptFlight"
            class="tex-gyre-adventor-bold col-2"
            style="font-size: medium"
            color="primary"
          >
            Accept flight
          </q-btn>
          <q-btn
            @click="editFlight"
            class="tex-gyre-adventor-bold col-2"
            style="font-size: medium"
            color="primary"
            >edit flight
          </q-btn>
          <q-btn
            @click="importChanges"
            class="tex-gyre-adventor-bold col-2"
            style="font-size: medium"
            color="primary"
          >
            Import Changes
          </q-btn>
        </div>
      </q-card-section>
    </q-card>
    <q-dialog label="Cancel flight" v-model="editFlightButton">
      <q-card class="column" style="min-width: 350px">
        <q-input
          class="q-mx-md q-mt-md"
          v-model="selectedFromName"
          readonly
          label="From"
          stack-label
          dense
        />
        <q-input
          class="q-mx-md q-mt-md"
          v-model="selectedToName"
          readonly
          label="To"
          stack-label
          dense
        />
        <q-input
          class="q-mx-md q-mt-md"
          v-model="selectedAircraft"
          readonly
          label="Aircraft"
          stack-label
          dense
        />
        <q-input
          class="q-mx-md q-mt-md"
          v-model="date"
          type="date"
          label="Date"
          stack-label
          dense
        />
        <q-input
          class="q-mx-md q-mt-md"
          v-model="time"
          type="time"
          label="Time"
          stack-label
          dense
        />
        <q-input
          class="q-mx-md q-mt-md"
          v-model="ecoPrice"
          type="number"
          label="Economy price"
          stack-label
          dense
        />
        <div class="justify-between">
          <q-btn
            @click="update"
            no-wrap
            class="tex-gyre-adventor-bold q-mx-md q-mt-md q-mb-md"
            style="font-size: medium; width: 150px"
            color="primary"
            >update
          </q-btn>
          <q-btn
            no-wrap
            class="tex-gyre-adventor-bold q-mx-md q-mt-md q-mb-md"
            style="font-size: medium; width: 150px"
            color="primary"
            v-close-popup
            >cancel
          </q-btn>
        </div>
      </q-card>
    </q-dialog>
    <q-dialog label="" v-model="importChangeButton">
      <q-card class="column" style="min-width: 400px">
        <q-card-section
          >Please select the text file with the changes
        </q-card-section>
        <div class="row">
          <q-file
            dense
            clearable
            class="q-mx-md"
            style="width: 300px"
            outlined
            v-model="file"
            label="File"
          >
          </q-file>
          <q-btn icon="send" @click="importish"></q-btn>
        </div>
        <div class="column">
          <q-card-section
            >Successful changes applied: {{ successChanges }}
          </q-card-section>
          <q-card-section
            >Duplicate records discarded: {{ duplicateChanges }}
          </q-card-section>
          <q-card-section
            >Records with missing fields discarded:
            {{ missingChanges }}
          </q-card-section>
        </div>
      </q-card>
    </q-dialog>
  </q-page>
</template>

<script setup lang="ts">
import { ref, onMounted} from 'vue';
import { authGet, authPost } from 'src/utils';

const airportListFrom = ref('Airport list');

const file = ref('');

const airportListFromOptions = ref([
  'Airport list',
  'Abu Dhabi',
  'Cairo',
  'Bahrain',
  'Aden',
  'Doha',
  'Riyadh',
]);

const airportListTo = ref('Airport list');

const airportListToOptions = ref([
  'Airport list',
  'Abu Dhabi',
  'Cairo',
  'Bahrain',
  'Aden',
  'Doha',
  'Riyadh',
]);

const flightNumber = ref('');

const columns = ref([
  {
    name: 'id',
    required: true,
    label: 'id',
    field: 'id',
    align: 'left',
    sortable: true,
  },
  {
    name: 'date',
    required: true,
    label: 'Date',
    field: 'date',
    align: 'center',
    sortable: true,
  },
  {
    name: 'time',
    required: true,
    label: 'Time',
    field: 'time',
    align: 'center',
    sortable: false,
  },
  {
    name: 'from',
    required: true,
    label: 'From',
    field: 'from',
    align: 'center',
    sortable: false,
  },
  {
    name: 'to',
    required: true,
    label: 'To',
    field: 'to',
    align: 'center',
    sortable: false,
  },
  {
    name: 'flightnumber',
    required: true,
    label: 'Flight number',
    field: 'flightnumber',
    align: 'center',
    sortable: false,
  },
  {
    name: 'aircraft',
    required: true,
    label: 'Aircraft',
    field: 'aircraft',
    align: 'left',
    sortable: false,
  },
  {
    name: 'economyprice',
    required: true,
    label: 'Economy price',
    field: 'economyprice',
    align: 'center',
    sortable: true,
  },
  {
    name: 'businessprice',
    required: true,
    label: 'Business price',
    field: 'businessprice',
    align: 'center',
    sortable: false,
  },
  {
    name: 'firstclassprice',
    required: true,
    label: 'First class price',
    field: 'firstclassprice',
    align: 'center',
    sortable: false,
  },
  {
    name: 'active',
    required: true,
    label: 'Accepted',
    field: 'active',
    align: 'center',
    sortable: true,
  },
]);

const rows = ref<
  Array<{
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
  }>
>([]);

const copyrows = ref<
  Array<{
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
  }>
>([]);

const selectedFlights = ref<
  Array<{
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
  }>
>([]);

onMounted(async () => {
  await authGet('/Schedule/GetSchedule').then((response) => {
    let tableArray = response.data.data;

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
      const first = Math.round(Number(bus) * 1.3).toString();
      rows.value.push({
        id: tuple.id,
        date: date,
        time: time,
        from: from,
        to: to,
        flightnumber: flight,
        aircraft: aircraft,
        economyprice: '$' + eco,
        businessprice: '$' + bus,
        firstclassprice: '$' + first,
        active: active,
      });
      copyrows.value.push({
        id: tuple.id,
        date: date,
        time: time,
        from: from,
        to: to,
        flightnumber: flight,
        aircraft: aircraft,
        economyprice: '$' + eco,
        businessprice: '$' + bus,
        firstclassprice: '$' + first,
        active: active,
      });
    });
  });
});

const applyFilters = () => {
  rows.value = copyrows.value.map((row) => ({ ...row }));
  let filteredRows = rows.value.filter((row) => {
    const fromFilter =
      airportListFrom.value !== 'Airport list'
        ? row.from === airportListFrom.value
        : true;
    const toFilter =
      airportListTo.value !== 'Airport list'
        ? row.to === airportListTo.value
        : true;
    const flightNumberFilter =
      flightNumber.value !== ''
        ? row.flightnumber === flightNumber.value
        : true;

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
  'active',
]);

const getSelectedRowsText = (amountOfRows: number) =>
  `${amountOfRows} users selected.`;

const editFlightButton = ref(false);

const selectedFromName = ref('');
const selectedToName = ref('');
const selectedAircraft = ref('');

const editFlight = () => {
  if (!selectedFlights.value.length || selectedFlights.value.length !== 1) {
    editFlightButton.value = false;
    return;
  }
  editFlightButton.value = true;

  selectedFlights.value.forEach((tuple: any) => {
    if (tuple.from) {
      selectedFromName.value = tuple.from;
    }
    if (tuple.to) {
      selectedToName.value = tuple.to;
    }
    if (tuple.aircraft) {
      selectedAircraft.value = tuple.aircraft;
    }
  });
};

const cancelFlight = () => {
  if (!selectedFlights.value.length) {
    return;
  }

  selectedFlights.value.forEach((tuple: any) => {
    if (tuple.active === true) {
      const formData = {
        scheduleId: tuple.id,
        activeState: false,
      };
      authPost('/Schedule/SetActiveFlight', formData);
      rows.value.forEach((tuple1: any) => {
        if (tuple.id === tuple1.id) {
          tuple.active = false;
        }
      });
    }
  });
};

const acceptFlight = () => {
  if (!selectedFlights.value.length) {
    return;
  }
  selectedFlights.value.forEach((tuple: any) => {
    if (tuple.active === false) {
      const formData = {
        scheduleId: tuple.id,
        activeState: true,
      };
      authPost('/Schedule/SetActiveFlight', formData);
      rows.value.forEach((tuple1: any) => {
        if (tuple.id === tuple1.id) {
          tuple.active = true;
        }
      });
    }
  });
};

const date = ref('');
const time = ref('');
const ecoPrice = ref('');

const update = () => {
  let newDate = date.value;
  let newTime = time.value;
  let newEcoPrice = ecoPrice.value.substring(0);
  const formData = {
    scheduleId: selectedFlights.value[0].id,
    date: newDate,
    time: newTime,
    economyPrice: newEcoPrice,
  };
  authPost('/Schedule/EditFlight', formData);
  rows.value.forEach((tuple: any) => {
    if (tuple.id === selectedFlights.value[0].id) {
      tuple.date = newDate;
      tuple.time = newTime + ':00';
      tuple.economyprice = '$' + newEcoPrice;
      tuple.businessprice =
        '$' + Math.round(Number(newEcoPrice) * 1.35).toString();
      tuple.firstclassprice =
        '$' + Math.round(Number(newEcoPrice) * 1.35 * 1.3).toString();
    }
  });
};

const importChangeButton = ref(false);

const importChanges = () => {
  importChangeButton.value = true;
};

const successChanges = ref('');
const duplicateChanges = ref('');
const missingChanges = ref('');

const importish = () => {
  const formData = {
    huUi: file.value,
  };
  authPost('/Schedule/EditFlightsByCsv', formData, {
    'Content-Type': 'multipart/form-data',
  }).then((response) => {
    successChanges.value = response.data.data.success;
    duplicateChanges.value = response.data.data.duplicates;
    missingChanges.value = response.data.data.fails;

    authGet('/Schedule/GetSchedule').then((response) => {
    let tableArray = response.data.data;

    rows.value =[];
    copyrows.value =[];
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
      const first = Math.round(Number(bus) * 1.3).toString();
      rows.value.push({
        id: tuple.id,
        date: date,
        time: time,
        from: from,
        to: to,
        flightnumber: flight,
        aircraft: aircraft,
        economyprice: '$' + eco,
        businessprice: '$' + bus,
        firstclassprice: '$' + first,
        active: active,
      });
      copyrows.value.push({
        id: tuple.id,
        date: date,
        time: time,
        from: from,
        to: to,
        flightnumber: flight,
        aircraft: aircraft,
        economyprice: '$' + eco,
        businessprice: '$' + bus,
        firstclassprice: '$' + first,
        active: active,
      });
    });
  });
  });
};
</script>

<style lang="sass">

thead tr th
  position: sticky
  z-index: 1

thead tr th
  top: 0
</style>
