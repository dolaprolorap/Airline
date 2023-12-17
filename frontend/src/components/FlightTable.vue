<script setup lang='ts'>


import { QTableColumn } from 'quasar';
import BookFlightDialog from 'components/BookFlightDialog.vue';

export interface IRow {
  from: string,
  to: string,
  date: string,
  time: string,
  flightIDs: string,
  basePrice: number,
  numberOfStops: number,
  id: number
}

const props = defineProps<{
  cabinType: string,
  rows: IRow[],
  selected: IRow[],
  isLoading: boolean,
  filter: string
}>();

defineEmits(['update:selected']);

const columns: QTableColumn[] = [
  {
    name: 'from',
    label: 'From',
    field: 'from',
    align: 'left',
    sortable: true
  },
  {
    name: 'to',
    label: 'To',
    field: 'to',
    align: 'left',
    sortable: true
  },
  {
    name: 'date',
    label: 'Date',
    field: 'date',
    align: 'center',
    sortable: true
  },
  {
    name: 'time',
    label: 'Time',
    field: 'time',
    align: 'center',
    sortable: true
  },
  {
    name: 'flightIDs',
    label: 'Flight numbers',
    field: 'flightIDs',
    align: 'left',
    sortable: true
  },
  {
    name: 'basePrice',
    label: 'Price',
    field: 'basePrice',
    format: (basePrice: number) => `${
      Math.floor(
        basePrice *
        (props.cabinType === 'Business' ? 1.3 : 1) *
        (props.cabinType === 'First Class' ? 1.755 : 1)
      )} $`,
    align: 'left',
    sortable: true
  },
  {
    name: 'numberOfStops',
    label: 'Stops',
    field: 'numberOfStops',
    align: 'left',
    sortable: true
  },
  {
    name: 'id',
    required: true,
    label: 'ID',
    field: 'id',
    align: 'left',
    sortable: true
  }
];

</script>

<template>
  <q-table
    dense
    style='max-height: 30vh'
    flat
    bordered
    separator='vertical'
    table-header-class='bg-primary'
    :rows='rows'
    :columns='columns'
    :loading='isLoading'
    row-key='id'
    :filter='filter'
    selection='single'
    :selected='selected'
    @selection='$emit("update:selected", $event.rows)'
    hide-selected-banner
    :rows-per-page-options='[0, 10, 15, 20, 25, 50]'
  >
    <template v-slot:header-cell='props'>
      <q-th :props='props' class='text-white tex-gyre-adventor-bold'
            style='font-size: large;'>
        {{ props.col.label }}
      </q-th>
    </template>
    <template v-slot:body='props'>
      <q-tr :props='props' @click='props.selected = !props.selected'>
        <q-td>
          <q-checkbox v-model='props.selected' color='secondary' />
        </q-td>
        <q-td v-for='col in props.cols' :key='col.name' :props='props' style='font-size: medium'>
          {{ col.value }}
        </q-td>
      </q-tr>
    </template>
    <template v-slot:loading>
      <q-inner-loading showing color='primary' />
    </template>
  </q-table>
</template>

<style scoped lang='sass'>

</style>