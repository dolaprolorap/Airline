<script setup lang='ts'>
import { computed, ComputedRef, reactive, Ref, ref } from 'vue';
import { QTableColumn } from 'quasar';

import { authGet, authPost } from 'src/utils';
import { Router } from 'src/router';

authGet('/Auth/GetMyself')
  .then(response => {
      if (response.data.data.user.roleName === 'User')
        Router.replace('/user');
    }
  );

interface Office {
  country: string,
  title: string,
  phone: string,
  contact: string
}

const selectedOffice = ref('All offices');

const officeOptions: Ref<string[]> = ref(['All offices']);

authGet('/Office/Get').then(
  response => {
    officeOptions.value = [
      'All offices',
      ...response.data.data.map((office: Office) => office.title)
    ];
  }
);

interface User {
  officeName: string,
  email: string,
  firstName: string,
  lastName: string,
  roleName: string,
  birthdate: string,
  active: boolean,
}


const selectedUsers: Ref<User[]> = ref([]);

const changeRoleDialog = ref(false);

const newRole = ref('');

const addUserDialog = ref(false);

const columns: QTableColumn[] = [
  {
    name: 'firstName',
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
    name: 'roleName',
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
    name: 'officeName',
    label: 'Office',
    field: 'office',
    align: 'left',
    sortable: true
  }
];

const visibleColumns = columns.filter(column => column.name !== 'id').map(column => column.name);

const users: Ref<User[]> = ref([]);

const isLoading = ref(true);

authGet('/AdminPanel/Users').then(
  response => {
    users.value = response.data.data;
  }
).then(() => {
  isLoading.value = false;
});


const rows: ComputedRef<User[]> = computed(() => {
  return users.value.map(user => {
    return { ...user, age: calculateAge(user.birthdate) };
  });
});


const getSelectedRowsText = (amountOfRows: number) => `${amountOfRows} users selected.`;

const submitChangeRole = () => {
  if (selectedUsers.value[0].roleName === newRole.value) {
    return;
  }

  authPost(
    '/AdminPanel/ChangeRole',
    {
      userEmail: selectedUsers.value[0].email,
      roleName: newRole.value
    });
  selectedUsers.value[0].roleName = newRole.value;
};

const calculateAge = (birthday: string): number => {
  const birthday_date = new Date(birthday);
  const ageDifMs = Date.now() - birthday_date.getTime();
  const ageDate = new Date(ageDifMs); // miliseconds from epoch
  return Math.abs(ageDate.getUTCFullYear() - 1970);
};

const determineUserColor = (user: User) => {
  if (!user.active)
    return 'bg-red text-white';

  if (user.roleName === 'Administrator')
    return 'bg-green text-white';
  else
    return 'bg-white';
};

const filterRows = (
  rows: User[],
  terms: string
) => {
  if (terms === 'All offices')
    return rows;

  return rows.filter(
    row => row.officeName === terms
  );
};

const changeLogin = () => {
  for (let selectedUser of selectedUsers.value) {
    const user = users.value.find(user => user.email === selectedUser.email) || selectedUser;
    user.active = !user.active;
    authPost('/AdminPanel/SetActiveUser', {
      email: user.email,
      activeState: user.active
    });
  }
};

const newUser = reactive({
  email: '',
  firstName: '',
  lastName: '',
  officeName: '',
  birthdate: '',
  password: ''
});

const clearNewUser = () => {
  for (const field in newUser)
    newUser[field as keyof typeof newUser] = '';
};

const submitAddUser = () => {
  users.value = [
    ...users.value,
    {
      ...newUser,
      roleName: 'User',
      active: true
    }
  ];
  authPost('/AdminPanel/AddUser',
    {
      ...newUser,
      roleName: 'User'
    });
};


</script>

<template>
  <q-page class='row items-center justify-center' style=''
  >
    <q-card>
      <q-card-section class='q-pb-xs row items-end justify-between text-h3'>
        <q-select v-model='selectedOffice' :options='officeOptions' label='Office' dense outlined />
        Admin panel
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
          :filter='selectedOffice'
          :filter-method='filterRows'
          :loading='isLoading'
          row-key='email'
          selection='multiple'
          v-model:selected='selectedUsers'
          :selected-rows-label='getSelectedRowsText'
          :rows-per-page-options='[0, 10, 15, 20, 25, 50]'
        >
          <template v-slot:header-cell='props'>
            <q-th v-if='props.col.name !== "id"' :props='props' class='text-white tex-gyre-adventor-bold'
                  style='font-size: large;'>
              {{ props.col.label }}
            </q-th>
          </template>
          <template v-slot:body='props'>
            <q-tr :props='props' @click='props.selected = !props.selected' :class='determineUserColor(props.row)'>
              <q-td>
                <q-checkbox v-model='props.selected' color='secondary' />
              </q-td>
              <q-td v-for='col in visibleColumns' :key='col' :props='props' style='font-size: medium'>
                {{ props.row[col] }}
              </q-td>
            </q-tr>
          </template>
          <template v-slot:loading>
            <q-inner-loading showing color='primary' />
          </template>
        </q-table>
        <div class='row fit justify-start'>
          <q-btn
            class='tex-gyre-adventor-bold col-2'
            style='font-size: medium'
            color='primary'
            :disable='selectedUsers.length !== 1'
            @click='changeRoleDialog = true; newRole = selectedUsers[0].roleName'
          >Change role
          </q-btn>
          <q-btn
            class='tex-gyre-adventor-bold col-3 offset-1'
            style='font-size: medium'
            color='primary'
            @click='changeLogin'
            :disable='selectedUsers.length === 0'
          >
            Enable/Disable Login
          </q-btn>
          <q-btn
            class='tex-gyre-adventor-bold col-2 offset-4'
            style='font-size: medium'
            color='primary'
            @click='addUserDialog = true'
          >
            Add user
          </q-btn>
        </div>
      </q-card-section>
    </q-card>
    <q-dialog v-model='changeRoleDialog'>
      <q-card class='q-pa-sm' style='min-width: 350px'>
        <q-card-section>
          <div class='text-h6'>Change role</div>
        </q-card-section>

        <q-card-section class='q-pt-none'>
          <q-input dense v-model='selectedUsers[0].email' readonly label='Email' type='email' />
          <q-input dense v-model='selectedUsers[0].firstName' readonly label='Name' />
          <q-input dense v-model='selectedUsers[0].lastName' readonly label='Last name' />
          <q-input dense v-model='selectedUsers[0].officeName' readonly label='Office' />
          <div class='column q-mt-md'>
            Role:
            <q-radio color='secondary' v-model='newRole' val='Administrator' label='Administrator' />
            <q-radio color='secondary' v-model='newRole' val='User' label='User' />
          </div>
        </q-card-section>

        <q-card-actions align='between' class='text-secondary'>
          <q-btn class='bg-primary text-white tex-gyre-adventor-bold' label='Cancel' v-close-popup />
          <q-btn class='bg-primary text-white tex-gyre-adventor-bold' label='Submit' v-close-popup
                 @click='submitChangeRole' />
        </q-card-actions>
      </q-card>
    </q-dialog>

    <q-dialog v-model='addUserDialog' @hide='clearNewUser'>
      <q-card class='q-pa-sm' style='min-width: 350px'>
        <q-card-section>
          <div class='text-h6'>Add user</div>
        </q-card-section>

        <q-card-section class='q-pt-none'>
          <q-form
            class='column'
            style='row-gap:8px'
            autocorrect='off'
            autocapitalize='off'
            autocomplete='new-password'
            spellcheck='false'
          >
            <q-input
              autocomplete='new-password'
              dense v-model='newUser.email' label='Email' type='email' />
            <q-input dense v-model='newUser.firstName' label='Name' />
            <q-input dense v-model='newUser.lastName' label='Last name' />
            <q-select dense v-model='newUser.officeName' :options='officeOptions.slice(1)' label='Office' />
            <q-input dense v-model='newUser.birthdate' label='Birthdate' type='date' />
            <q-input dense v-model='newUser.password'
                     autocomplete='new-password'
                     label='Password' type='password' />
          </q-form>
        </q-card-section>

        <q-card-actions align='right' class='text-secondary'>
          <q-btn class='bg-primary text-white tex-gyre-adventor-bold' label='Cancel' v-close-popup />
          <q-btn class='bg-primary text-white tex-gyre-adventor-bold' label='Submit' v-close-popup
                 @click='submitAddUser' />
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
