<template>
  <q-page class='row items-center justify-center' style=''
  >
    <q-card>
      <q-card-section class="row">
        <q-card-section class="col">
          Hi, {{ username }}, Welcome to AMONIC Airlines.
        </q-card-section>
        <q-card-section class="col">
          Time spent on system: {{ time }}
        </q-card-section>
        <q-card-section class="offset-1">
          Number of crashes: {{ numberOfCraches }}
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
        >
          <template v-slot:header-cell='props'>
            <q-th v-if='props.col.name !== "id"' :props='props' class='text-white tex-gyre-adventor-bold'
                  style='font-size: large;'>
              {{ props.col.label }}
            </q-th>
          </template>
        </q-table>
      </q-card-section>
    </q-card>
  </q-page>
</template>

<script setup lang="ts">
import {ref, onMounted} from 'vue';
import {api} from 'src/boot/axios'
import {useAuthStore} from "../stores/store";

const User = useAuthStore();

const username = User.username;
const time = 'DOHUYA';
const numberOfCraches = '100';

const columns = [
  {
    name: 'date',
    required: true,
    label: 'Date',
    field: 'date',
    align: 'left',
    sortable: true
  },
  {
    name: 'login-time',
    label: 'Login time',
    field: 'logintime',
    align: 'left',
    sortable: true
  },
  {
    name: 'logout-time',
    label: 'Logout time',
    field: 'logouttime',
    align: 'left',
    sortable: true,
  },
  {
    name: 'time-spent',
    label: 'Time spent on on system',
    field: 'timespent',
    align: 'left',
    sortable: true
  },
  {
    name: 'unsuccessful-logout',
    label: 'Unsuccessful logout reason',
    field: 'unsuccessfullogout',
    align: 'left',
    sortable: true
  },
];

const rows = ref<Array<{ date: string; logintime: string; logouttime: string; timespent: string; unsuccessfullogout: string }>>([]);

onMounted(async () => {
  await api.get('/UserPanel/GetCurrentUserActivity').then(response => {
    let tableArray = response.data.data;

    tableArray.forEach((tuple: any) => {

      const date = tuple.dateTime.substring(0,10);
      const time = tuple.dateTime.substring(11,19);

      rows.value.unshift({
        date: date,
        logintime: time,
        logouttime: '19:00',
        timespent: 'mnogo',
        unsuccessfullogout: ''
      })
    })
  })
});
</script>

<style lang="sass" scoped>
thead tr th
  position: sticky
  z-index: 1

thead tr th
  top: 0
</style>