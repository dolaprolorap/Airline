<template>
  <q-page class="row items-center justify-center" style="">
    <q-card>
      <div class="row justify-end">
        <q-card-section class="text-h3">User Panel</q-card-section>
      </div>
      <q-splitter horizontal></q-splitter>
      <q-card-section class="row">
        <q-card-section class="col">
          Hi, {{ email }}, Welcome to AMONIC Airlines.
        </q-card-section>
        <q-card-section class="col">
          Time spent on system: {{ time }}
        </q-card-section>
        <q-card-section class="offset-1">
          Number of crashes: {{ numberOfCraches }}
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
            <q-tr :props="props">
              <q-td
                v-for="col in visibleColumns"
                :key="col"
                :props="props"
                style="font-size: medium"
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
import {ref, onMounted} from 'vue';
import {api} from 'src/boot/axios';
import {LocalStorage} from 'quasar';
import {authGet} from "src/utils";

const email = LocalStorage.getItem('email');

const time = 'DOHUYA';
const numberOfCraches = '100';

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
    label: 'Unsuccessful logout reason',
    field: 'unsuccessfullogout',
    align: 'left',
    sortable: true,
  },
]);

const rows = ref<
    Array<{
      date: string;
      logintime: string|undefined;
      logouttime: string;
      timespent: string;
      unsuccessfullogout: string;
    }>
>([]);

onMounted(async () => {
  await authGet('/UserPanel/GetCurrentUserActivity').then((response) => {
    let tableArray = response.data.data;

    // tableArray.forEach((tuple: any) => {
    //   const date = tuple.dateTime.substring(0, 10);
    //   const time = tuple.dateTime.substring(11, 19);
    //
    //   rows.value.push({
    //     date: date,
    //     logintime: time,
    //     logouttime: '19:00',
    //     timespent: 'mnogo',
    //     unsuccessfullogout: '',
    //   });
    // });

    for (let i = 0; i < tableArray.length - 1; i++) {
      let tuple = tableArray[i];
      let tupleNext = tableArray[i + 1];
      if (tuple.warnType === 'err') {
        rows.value.push({
          date: tuple.date,
          logintime: '**',
          logouttime: '**',
          timespent: '**',
          unsuccessfullogout: tuple.description
        });
      } else {

        let timee:string|undefined;
        let totalTime;
        if (tupleNext.time[0] !== 0) {
          const hoursNext = Number(tupleNext.time.substring(0, 2)) * 60 * 60;
          const minutesNext = Number(tupleNext.time.substring(3, 5)) * 60;
          const secondsNext = Number(tupleNext.time.substring(5));
          const totalNext = hoursNext + minutesNext + secondsNext;

          const hours = Number(tuple.time.substring(0, 2)) * 60 * 60;
          const minutes = Number(tuple.time.substring(3, 5)) * 60;
          const seconds = Number(tuple.time.substring(5));
          const total = hours + minutes + seconds;

          totalTime = (totalNext-total);

          const totalHours = Math.floor(totalTime/3600);
          totalTime -= totalHours*3600;

          const totalMinutes = Math.floor(totalTime/60);
          totalTime -= totalMinutes*60;

          totalTime = totalHours + ':' + totalMinutes + ':' + totalTime;
        }

        timee = totalTime;

        rows.value.push({
          date: tuple.date,
          logintime: timee,
          logouttime: '**',
          timespent: '**',
          unsuccessfullogout: tuple.description
        });
      }
    }
  });
});
</script>

<style lang="sass" scoped>
thead tr th
  position: sticky
  z-index: 1

thead tr th
  top: 0
</style>
