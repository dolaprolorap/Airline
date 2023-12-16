<script setup lang='ts'>
import { Ref, ref } from 'vue';
import { QTableColumn } from 'quasar';
import { api } from 'boot/axios'
import { authPost } from 'src/utils';

const office = ref('All offices');

const officeOptions = ref([
  'All offices',
  'Abu Dhabi',
  'Bahrain',
  'Doha'
]);

interface User {
  id: number,
  name: string,
  lastName: string,
  age: number,
  userRole: string,
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

const rows: User[] = Array(6).fill([
  {
    id: 0,
    name: 'Vasya',
    lastName: 'Huev',
    age: '54',
    userRole: 'Administrator',
    email: 'vasyanBoGG@mail.sru',
    office: officeOptions.value[1]
  },
  {
    id: 1,
    name: 'Iluha',
    lastName: 'Zhopich',
    age: '35',
    userRole: 'User',
    email: 'gemoroyKolbASS@mail.sru',
    office: officeOptions.value[2]
  },
  {
    id: 2,
    name: 'Tyoma',
    lastName: 'Super',
    age: '19',
    userRole: 'Administrator',
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

const submitChange = () => {
  if (selectedUsers.value[0].userRole === newRole.value) {
    return;
  }

  authPost(
    '/AdminPanel/ChangeRole',
    {
      userEmail: selectedUsers.value[0].email,
      roleName: newRole
    });
  selectedUsers.value[0].userRole = newRole.value;
}

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
            @click='changeRoleDialog = true; newRole = selectedUsers[0].userRole'
          >Change role
          </q-btn>
          <q-btn class='tex-gyre-adventor-bold col-3 offset-1' style='font-size: medium' color='primary'>Enable/Disable
            Login
          </q-btn>
          <q-btn class='tex-gyre-adventor-bold col-2 offset-4' style='font-size: medium' color='primary'>Add user
          </q-btn>
        </div>
        <div>
          {{ selectedUsers }}
        </div>
      </q-card-section>
    </q-card>
    <q-dialog v-model='changeRoleDialog'>
      <q-card style='min-width: 350px'>
        <q-card-section>
          <div class='text-h6'>Change role</div>
        </q-card-section>

        <q-card-section class='q-pt-none'>
          <q-input dense v-model='selectedUsers[0].email' readonly label='Email' type='email'/>
          <q-input dense v-model='selectedUsers[0].name' readonly label='Name' />
          <q-input dense v-model='selectedUsers[0].lastName' readonly label='Last name' />
          <q-input dense v-model='selectedUsers[0].office' readonly label='Office' />
          <div class='column q-mt-md'>
            Role:
            <q-radio v-model="newRole" val="Administrator" label="Administrator" />
            <q-radio v-model="newRole" val="User" label="User" />
          </div>
        </q-card-section>

        <q-card-actions align='right' class='text-secondary'>
          <q-btn class='bg-primary text-white tex-gyre-adventor-bold' label='Cancel' v-close-popup />
          <q-btn class='bg-primary text-white tex-gyre-adventor-bold' label='Submit' v-close-popup @click='submitChange'/>
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
