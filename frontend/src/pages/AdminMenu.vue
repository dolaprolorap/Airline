<script setup lang='ts'>
import { ref } from 'vue';

const office = ref('All offices');

const officeOptions = ref([
  'All offices',
  'Abu Dhabi',
  'Bahrain',
  'Doha'
]);

const selectedUsers = ref([]);

const columns = [
  {
    name: 'id',
    required: true,
    label: 'id',
    field: 'id',
    align: 'left',
    sortable: true
  },
  {
    name: 'name',
    required: true,
    label: 'Name',
    field: 'name',
    align: 'left',
    sortable: true
  },
  {
    name: 'lastName',
    label: 'Last Name',
    field: 'lastName',
    align: 'left',
    sortable: true
  },
  {
    name: 'age',
    label: 'Age',
    field: 'age',
    align: 'center',
    sortable: true,
    sort: (a: string, b: string) => parseInt(a, 10) - parseInt(b, 10)
  },
  {
    name: 'userRole',
    label: 'User Role',
    field: 'userRole',
    align: 'left',
    sortable: true
  },
  {
    name: 'email',
    label: 'Email Address',
    field: 'email',
    align: 'left',
    sortable: true
  },
  {
    name: 'office',
    label: 'Office',
    field: 'office',
    align: 'left',
    sortable: true
  }
];

const visibleColumns = columns.filter(column => column.name !== 'id').map(column => column.name);

const rows = Array(6).fill([
  {
    id: 0,
    name: 'Vasya',
    lastName: 'Huev',
    age: '54',
    userRole: 'administrator',
    email: 'vasyanBoGG@mail.sru',
    office: officeOptions.value[1]
  },
  {
    id: 1,
    name: 'Iluha',
    lastName: 'Zhopich',
    age: '35',
    userRole: 'office user',
    email: 'gemoroyKolbASS@mail.sru',
    office: officeOptions.value[2]
  },
  {
    id: 2,
    name: 'Tyoma',
    lastName: 'Super',
    age: '19',
    userRole: 'administrator',
    email: 'coolC0D3R@mail.sru',
    office: officeOptions.value[3]
  }
]).flat().reduce((acc: { arr: [], id: number }, elem) => {
  return {
    arr: acc.arr.concat({ ...elem, id: acc.id }),
    id: acc.id + 1
  };
}, { arr: [], id: 0 }).arr;

const getSelectedRowsText = (amountOfRows: number) => `${amountOfRows} users selected.`;

</script>
<template>
  <q-page class='row items-center justify-center' style=''
  >
    <q-card>
      <q-card-section class='column items-start' style='row-gap: 16px'>
        <q-select v-model='office' :options='officeOptions' label='Office' dense outlined />
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
          v-model:selected='selectedUsers'
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
                <q-checkbox v-model='props.selected' color='secondary' />
              </q-td>
              <q-td v-for='col in visibleColumns' :key='col' :props='props' style='font-size: medium'>
                {{ props.row[col] }}
              </q-td>
            </q-tr>
          </template>
        </q-table>
        <div class='row fit justify-start'>
          <q-btn class='tex-gyre-adventor-bold col-2' style='font-size: medium' color='primary'>Change role</q-btn>
          <q-btn class='tex-gyre-adventor-bold col-3 offset-1' style='font-size: medium' color='primary'>Enable/Disable
            Login
          </q-btn>
          <q-btn class='tex-gyre-adventor-bold col-2 offset-4' style='font-size: medium' color='primary'>Add user</q-btn>
        </div>
      </q-card-section>
    </q-card>
  </q-page>
</template>

<style  lang='sass'>
thead tr th
  position: sticky
  z-index: 1

thead tr th
  top: 0
</style>
