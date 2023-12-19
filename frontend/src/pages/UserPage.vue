<template>
  <q-page class="row items-center justify-center" style="">
    <q-card>
      <div class="row justify-end">
        <q-card-section class="text-h3">User Panel</q-card-section>
      </div>
      <q-splitter horizontal></q-splitter>
      <q-card-section class="q-pb-none row">
        <q-card-section class=" col" style="font-size: larger">
          Hi, {{ email }}, Welcome to AMONIC Airlines.
        </q-card-section>
        <q-card-section class="col" style="font-size: larger">
          Time spent on system: {{ time }}
        </q-card-section>
        <q-card-section class="offset-1" style="font-size: larger">
          Number of crashes: {{ numberOfCraches }}
        </q-card-section>
      </q-card-section>
      <q-card-section class="q-pt-none column items-start" style="row-gap: 16px">
        <q-table
          style="max-height: 60vh"
          flat
          bordered
          separator="vertical"
          table-header-class="bg-primary"
          :rows="rows"
          :columns="columns"
          row-key="id"
          :rows-per-page-options='[0, 10, 15, 20, 25, 50]'
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
            <q-tr :props="props" v-if="props.row.unsuccessfullogout === ''">
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
      </q-card-section>
    </q-card>
  </q-page>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { api } from 'src/boot/axios';
import { LocalStorage } from 'quasar';
import { authGet } from 'src/utils';

const email = LocalStorage.getItem('email');

const time = ref('');
const numberOfCraches = ref(0);

const visibleColumns = ref([
  'date',
  'logintime',
  'logouttime',
  'timespent',
  'unsuccessfullogout',
]);

const columns = ref([
  {
    name: 'date',
    required: true,
    label: 'Date',
    field: 'date',
    align: 'left',
    sortable: true,
  },
  {
    name: 'logintime',
    label: 'Login time',
    field: 'logintime',
    align: 'left',
    sortable: true,
  },
  {
    name: 'logouttime',
    label: 'Logout time',
    field: 'logouttime',
    align: 'left',
    sortable: true,
  },
  {
    name: 'timespent',
    label: 'Time spent on on system',
    field: 'timespent',
    align: 'left',
    sortable: true,
  },
  {
    name: 'unsuccessfullogout',
    label: 'Unsuccessful login reason',
    field: 'unsuccessfullogout',
    align: 'left',
    sortable: true,
  },
]);

const rows = ref<
  Array<{
    date: string;
    logintime: string | undefined;
    logouttime: string;
    timespent: string;
    unsuccessfullogout: string;
  }>
>([]);

onMounted(async () => {
  await authGet('/UserPanel/GetCurrentUserActivity').then((response) => {
    let tableArray = response.data.data;

    let totalMilliseconds = 0;
    let id = 0;
    for (let i = 0; i < tableArray.length; i++) {
      const currentTuple = tableArray[i];
      const nextTuple = tableArray[i + 1];

      const date = currentTuple.dateTime.substring(0, 10);
      const time = currentTuple.dateTime.substring(11, 19);

      if (currentTuple.description !== '') {
        rows.value.unshift({
          date: date,
          logintime: time,
          logouttime: '**',
          timespent: '**',
          unsuccessfullogout: currentTuple.description,
        });
        id += 1;
        numberOfCraches.value = id.toString();
        continue;
      }

      let logoutTime = '';

      if (nextTuple) {
        logoutTime = nextTuple.dateTime.substring(11, 19);
      } else {
        rows.value.unshift({
          date: date,
          logintime: time,
          logouttime: '**',
          timespent: '**',
          unsuccessfullogout: currentTuple.description,
        });
        continue;
      }

      const loginTime = time;
      const timespent = nextTuple
        ? calculateTimeDifference(nextTuple.dateTime, currentTuple.dateTime)
        : '';
      const description = currentTuple.description;

      if (timespent !== '**') {
        // Накапливать общее время в миллисекундах
        totalMilliseconds += calculateMilliseconds(timespent);
      }
      rows.value.unshift({
        date: date,
        logintime: loginTime,
        logouttime: logoutTime,
        timespent: timespent,
        unsuccessfullogout: description,
      });
    }
    const totalTimespent = formatTimeDifference(totalMilliseconds);
    time.value = totalTimespent;
  });
});

function formatTimeComponent(component: number): string {
  return component < 10 ? `0${component}` : `${component}`;
}

function calculateTimeDifference(endTime: string, startTime: string): string {
  const start = new Date(startTime);
  const end = new Date(endTime);
  const diff = end.getTime() - start.getTime();
  const hours = Math.floor(diff / (1000 * 60 * 60));
  const minutes = Math.floor((diff % (1000 * 60 * 60)) / (1000 * 60));
  const seconds = Math.floor((diff % (1000 * 60)) / 1000);

  return `${formatTimeComponent(hours)}:${formatTimeComponent(
    minutes
  )}:${formatTimeComponent(seconds)}`;
}

function calculateMilliseconds(timespent: string): number {
  const [hours, minutes, seconds] = timespent.split(':').map(Number);
  return hours * 3600 * 1000 + minutes * 60 * 1000 + seconds * 1000;
}

function formatTimeDifference(milliseconds: number): string {
  const hours = Math.floor(milliseconds / (1000 * 60 * 60));
  const minutes = Math.floor((milliseconds % (1000 * 60 * 60)) / (1000 * 60));
  const seconds = Math.floor((milliseconds % (1000 * 60)) / 1000);

  return `${formatTimeComponent(hours)}:${formatTimeComponent(minutes)}:${formatTimeComponent(seconds)}`;
}


</script>

<style lang="sass" scoped>
thead tr th
  position: sticky
  z-index: 1

thead tr th
  top: 0
</style>
