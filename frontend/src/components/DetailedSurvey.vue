<script setup lang='ts'>
import { onMounted, ref } from 'vue';
import { authGet, authPost } from 'src/utils';

const columns = ref([
  {
    name: 'question',
    label: '',
    field: 'question',
    required: true,
    align: 'center'
  },
  {
    name: 'total',
    label: 'Total',
    field: 'total',
    required: true,
    align: 'center'
  },
  {
    name: 'male',
    label: 'Male',
    field: 'male',
    required: true,
    align: 'center'
  },
  {
    name: 'female',
    label: 'Female',
    field: 'female',
    required: true,
    align: 'center'
  },
  {
    name: 'a1824',
    label: '18-24',
    field: 'a1824',
    required: true,
    align: 'center'
  },
  {
    name: 'a2539',
    label: '25-39',
    field: 'a2539',
    required: true,
    align: 'center'
  },
  {
    name: 'a4059',
    label: '40-59',
    field: 'a4059',
    required: true,
    align: 'center'
  },
  {
    name: 'a60',
    label: '60+',
    field: 'a60',
    required: true,
    align: 'center'
  },
  {
    name: 'economy',
    label: 'Economy',
    field: 'economy',
    required: true,
    align: 'center'
  },
  {
    name: 'business',
    label: 'Business',
    field: 'business',
    required: true,
    align: 'center'
  },
  {
    name: 'first',
    label: 'First',
    field: 'first',
    required: true,
    align: 'center'
  },
  {
    name: 'AUH',
    label: 'AUH',
    field: 'AUH',
    required: true,
    align: 'center'
  },
  {
    name: 'ADE',
    label: 'ADE',
    field: 'ADE',
    required: true,
    align: 'center'
  },
  {
    name: 'BAH',
    label: 'BAH',
    field: 'BAH',
    required: true,
    align: 'center'
  },
  {
    name: 'DOH',
    label: 'DOH',
    field: 'DOH',
    required: true,
    align: 'center'
  },
  {
    name: 'RYU',
    label: 'RYU',
    field: 'RYU',
    required: true,
    align: 'center'
  },
  {
    name: 'CAI',
    label: 'CAI',
    field: 'CAI',
    required: true,
    align: 'center'
  }
]);
const visibleColumns = ref([
  'question',
  'total',
  'male',
  'female',
  'a1824',
  'a2539',
  'a4059',
  'a60',
  'economy',
  'business',
  'first',
  'AUH',
  'ADE',
  'BAH',
  'DOH',
  'RYU',
  'CAI'
]);
const rows = ref<
  Array<{
    question: string;
    total: string;
    male: string;
    female: string;
    a1824: string;
    a2539: string;
    a4059: string;
    a60: string;
    economy: string;
    business: string;
    first: string;
    AUH: string;
    ADE: string;
    BAH: string;
    DOH: string;
    RYU: string;
    CAI: string;
  }>
>([]);

const sample = ref('');

onMounted(async () => {
  await authGet('/Survey/GetDetailed').then((response) => {
    for (let i = 1; i < 5; i++) {
      const questArray = ['Please rate our aircraft flown on AMONIC Airlines:',
        'How would you rate our flight attendants:',
        'How would you rate our inflight entertainment:',
        'Please rate the ticket price for the trip you are taking:'
      ];
      rows.value.push({
        question: questArray[i - 1],
        total: '',
        male: '',
        female: '',
        a1824: '',
        a2539: '',
        a4059: '',
        a60: '',
        economy: '',
        business: '',
        first: '',
        AUH: '',
        ADE: '',
        BAH: '',
        DOH: '',
        RYU: '',
        CAI: ''
      });
      for (let j = 0; j < 7; j++) {
        const tableArray = response.data.data[`q${i}`][`${j}`];
        const rateArray = ['Outstanding',
          'Very Good',
          'Good',
          'Adequate',
          'Needs Improvement',
          'Poor',
          'Dont know'
        ];
        rows.value.push({
          question: rateArray[j],
          total: tableArray.total,
          male: tableArray.male,
          female: tableArray.female,
          a1824: tableArray.age18_24,
          a2539: tableArray.age25_39,
          a4059: tableArray.age40_59,
          a60: tableArray.age60,
          economy: tableArray.economy,
          business: tableArray.business,
          first: tableArray.first,
          AUH: tableArray.auh,
          ADE: tableArray.ade,
          BAH: tableArray.bah,
          DOH: tableArray.doh,
          RYU: tableArray.ruh,
          CAI: tableArray.cai
        });
      }
    }
  });
});

</script>

<template>
  <q-card>
    <q-card-section class='row justify-end text-h3'
    >Detailed Report
    </q-card-section>
    <q-splitter horizontal></q-splitter>

    <q-card-section class='column items-start' style='row-gap: 16px'>
      <q-table
        style='max-height: 57vh; max-width: 95vw'
        flat
        dense
        hide-pagination
        bordered
        separator='vertical'
        table-header-class='bg-primary'
        :rows='rows'
        :columns='columns'
        :rows-per-page-options='[0, 10, 15, 20, 25, 50]'
      >
        <template v-slot:header='props'>
          <q-tr class='text-white tex-gyre-adventor-bold bg-primary'>
            <q-th colspan='0' style='text-align: center; font-size: x-large'>
            </q-th>
            <q-th colspan='0' style='text-align: center; font-size: x-large'>
            </q-th>
            <q-th colspan='2' style='text-align: center; font-size: x-large'
            >Gender
            </q-th>
            <q-th colspan='4' style='text-align: center; font-size: x-large'
            >Age
            </q-th>
            <q-th colspan='3' style='text-align: center; font-size: x-large'
            >Cabin Type
            </q-th>
            <q-th colspan='6' style='font-size: x-large'
            >Destination Airport
            </q-th>
          </q-tr>
          <q-tr
            class='text-white tex-gyre-adventor-bold bg-primary'
            style='font-size: large'
          >
            <q-th
              :props='props'
              key='question'
              style='text-align: center; font-size: large; padding: 8px 0; max-width: 25vw'
            >question
            </q-th>
            <q-th
              :props='props'
              key='total'
              style='text-align: center; font-size: large'
            >total
            </q-th>
            <q-th
              :props='props'
              key='male'
              style='text-align: center; font-size: large'
            >male
            </q-th>
            <q-th
              :props='props'
              key='female'
              style='text-align: center; font-size: large'
            >female
            </q-th>
            <q-th
              :props='props'
              key='a1824'
              style='text-align: center; font-size: large'
            >18-24
            </q-th>
            <q-th
              :props='props'
              key='a2539'
              style='text-align: center; font-size: large'
            >25-39
            </q-th>
            <q-th
              :props='props'
              key='a4059'
              style='text-align: center; font-size: large'
            >40-59
            </q-th>
            <q-th
              :props='props'
              key='a60'
              style='text-align: center; font-size: large'
            >60+
            </q-th>
            <q-th
              :props='props'
              key='economy'
              style='text-align: center; font-size: large'
            >economy
            </q-th>
            <q-th
              :props='props'
              key='business'
              style='text-align: center; font-size: large'
            >business
            </q-th>
            <q-th
              :props='props'
              key='economy'
              style='text-align: center; font-size: large'
            >first class
            </q-th>
            <q-th
              :props='props'
              key='economy'
              style='text-align: center; font-size: large'
            >AUH
            </q-th>
            <q-th
              :props='props'
              key='economy'
              style='text-align: center; font-size: large'
            >ADE
            </q-th>
            <q-th
              :props='props'
              key='economy'
              style='text-align: center; font-size: large'
            >BAH
            </q-th>
            <q-th
              :props='props'
              key='economy'
              style='text-align: center; font-size: large'
            >DOH
            </q-th>
            <q-th
              :props='props'
              key='economy'
              style='text-align: center; font-size: large'
            >RYU
            </q-th>
            <q-th
              :props='props'
              key='economy'
              style='text-align: center; font-size: large'
            >CAI
            </q-th>
          </q-tr>
        </template>
        <template v-slot:body='props'>
          <q-tr :props='props' v-if="props.row.total !== ''">
            <q-td
              v-for='col in visibleColumns'
              :key='col'
              :props='props'
              style='font-size: medium'
            >
              {{ props.row[col] }}
            </q-td>
          </q-tr>
          <q-tr v-else>
            <q-td class='bg-secondary text-white'
                  v-for='col in visibleColumns'
                  :key='col'
                  :props='props'
                  style='font-size: medium'
            >
              {{ props.row[col] }}
            </q-td>
          </q-tr>
        </template>
      </q-table>
    </q-card-section>
  </q-card>
</template>

<style scoped lang='sass'>

thead tr th
  position: sticky
  z-index: 1

thead tr th
  top: 0
</style>
