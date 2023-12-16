<script setup lang='ts'>
import { computed, ComputedRef, Ref, ref } from 'vue';
import { QTableColumn } from 'quasar';

import { authGet, authPost } from 'src/utils';

const office = ref('All offices');

const officeOptions = ref([
  'All offices',
  'Abu Dhabi',
  'Bahrain',
  'Doha'
]);

interface User {
  id: number,
  roleId: number,
  email: string,
  password: string,
  firstName: string,
  lastName: string,
  birthdate: string,
  active: boolean,
  office: string,
  role: string,
  tickets: string[],
  trackerrecords: string[],
  token: string
}

interface Row {
  id: number,
  firstName: string,
  lastName: string,
  age: number,
  role: string,
  email: string,
  office: string
}

const selectedUsers: Ref<User[]> = ref([]);

const changeRoleDialog = ref(false);

const newRole = ref('');

const columns: QTableColumn[] = [
  {
    name: 'id',
    required: true,
    label: 'id',
    field: 'id',
    align: 'left',
    sortable: true
  },
  {
    name: 'firstName',
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

const users: Ref<User[]> = ref([])

authGet('/AdminPanel/Users').then(
  response => {
    users.value = response.data.data;
    console.log(rows.value);
  }
);

const rows: ComputedRef<Row[]> = computed(() => {
  return users.value.map(user => {return {...user, age: calculateAge(user.birthdate)}})
});


const getSelectedRowsText = (amountOfRows: number) => `${amountOfRows} users selected.`;

const submitChange = () => {
  if (selectedUsers.value[0].role === newRole.value) {
    return;
  }

  authPost(
    '/AdminPanel/ChangeRole',
    {
      userEmail: selectedUsers.value[0].email,
      roleName: newRole.value
    });
  selectedUsers.value[0].role = newRole.value;
};

const calculateAge = (birthday: string): number => {
  const birthday_date = new Date(birthday)
  const ageDifMs = Date.now() - birthday_date.getTime();
  const ageDate = new Date(ageDifMs); // miliseconds from epoch
  return Math.abs(ageDate.getUTCFullYear() - 1970);
};

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
          :filter='office === "All offices" ? "" : office'
          row-key='id'
          selection='multiple'
          v-model:selected='selectedUsers'
          :selected-rows-label='getSelectedRowsText'
          :rows-per-page-options='[10, 15, 20, 25, 50, 0]'
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
          <q-btn
            class='tex-gyre-adventor-bold col-2'
            style='font-size: medium'
            color='primary'
            :disable='selectedUsers.length !== 1'
            @click='changeRoleDialog = true; newRole = selectedUsers[0].role'
          >Change role
          </q-btn>
          <q-btn class='tex-gyre-adventor-bold col-3 offset-1' style='font-size: medium' color='primary'>Enable/Disable
            Login
          </q-btn>
          <q-btn class='tex-gyre-adventor-bold col-2 offset-4' style='font-size: medium' color='primary'>Add user
          </q-btn>
        </div>
      </q-card-section>
    </q-card>
    <q-dialog v-model='changeRoleDialog'>
      <q-card style='min-width: 350px'>
        <q-card-section>
          <div class='text-h6'>Change role</div>
        </q-card-section>

        <q-card-section class='q-pt-none'>
          <q-input dense v-model='selectedUsers[0].email' readonly label='Email' type='email' />
          <q-input dense v-model='selectedUsers[0].firstName' readonly label='Name' />
          <q-input dense v-model='selectedUsers[0].lastName' readonly label='Last name' />
          <q-input dense v-model='selectedUsers[0].office' readonly label='Office' />
          <div class='column q-mt-md'>
            Role:
            <q-radio v-model='newRole' val='Administrator' label='Administrator' />
            <q-radio v-model='newRole' val='User' label='User' />
          </div>
        </q-card-section>

        <q-card-actions align='right' class='text-secondary'>
          <q-btn class='bg-primary text-white tex-gyre-adventor-bold' label='Cancel' v-close-popup />
          <q-btn class='bg-primary text-white tex-gyre-adventor-bold' label='Submit' v-close-popup
                 @click='submitChange' />
        </q-card-actions>
      </q-card>
    </q-dialog>
  </q-page>
</template>

<style lang='sass'>
thead tr th
  position: sticky
  z-index: 1

thead tr th
  top: 0
</style>
