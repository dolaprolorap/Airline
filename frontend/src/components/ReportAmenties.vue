<template>
    <q-card>
      <q-card-section class="row justify-end text-h3"
        >Amenties Report
      </q-card-section>
      <q-splitter horizontal></q-splitter>
      <q-card-section class="column items-start" style="row-gap: 16px">
        <q-table
          style="max-height: 69vh"
          flat
          hide-pagination
          bordered
          dense
          separator="vertical"
          table-header-class="bg-primary"
          :rows="rows"
          :columns="columns"
        >
          <template v-slot:header-cell="props">
            <q-th
              v-if="props.col.name !== 'id'"
              :props="props"
              class="text-white tex-gyre-adventor-bold"
              style="font-size: large"
            >
              <div v-html="multiLineHeader(props.col.label)"></div>
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
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { authGet } from 'src/utils';

const columns = ref([
  {
    name: 'ExtraBlanket',
    label: 'Extra Blanket',
    field: 'ExtraBlanket',
    required: true,
    align: 'center',
  },
  {
    name: 'NextSeatFree',
    label: 'Next Seat Free',
    field: 'NextSeatFree',
    required: true,
    align: 'center',
  },
  {
    name: 'TwoNeighboringSeatsFree',
    label: 'Two Neighboring Seats Free',
    field: 'TwoNeighboringSeatsFree',
    required: true,
    align: 'center',
  },
  {
    name: 'TabletRental',
    label: 'Tablet Rental',
    field: 'TabletRental',
    required: true,
    align: 'center',
  },
  {
    name: 'LaptopRental',
    label: 'Laptop Rental',
    field: 'LaptopRental',
    required: true,
    align: 'center',
  },
  {
    name: 'LoungeAccess',
    label: 'Lounge Access',
    field: 'LoungeAccess',
    required: true,
    align: 'center',
  },
  {
    name: 'SoftDrinks',
    label: 'Soft Drinks',
    field: 'SoftDrinks',
    required: true,
    align: 'center',
  },
  {
    name: 'PremiumHeadphonesRental',
    label: 'Premium Headphones Rental',
    field: 'PremiumHeadphonesRental',
    required: true,
    align: 'center',
  },
  {
    name: 'ExtraBag',
    label: 'Extra Bag',
    field: 'ExtraBag',
    required: true,
    align: 'center',
  },
  {
    name: 'FastCheckinLane',
    label: 'Fast Checkin Lane',
    field: 'FastCheckinLane',
    required: true,
    align: 'center',
  },
  {
    name: 'WiFi50mb',
    label: 'Wi-Fi 50 mb',
    field: 'WiFi50mb',
    required: true,
    align: 'center',
  },
  {
    name: 'WiFi250mb',
    label: 'Wi-Fi 250 mb',
    field: 'WiFi250mb',
    required: true,
    align: 'center',
  },
]);

const multiLineHeader = (label: any) => {
  // Replace spaces with <br> to create line breaks
  return label.replace(/\s+/g, '<br>');
};

const visibleColumns = ref([
  'ExtraBlanket',
  'NextSeatFree',
  'TwoNeighboringSeatsFree',
  'TabletRental',
  'LaptopRental',
  'LoungeAccess',
  'SoftDrinks',
  'PremiumHeadphonesRental',
  'ExtraBag',
  'FastCheckinLane',
  'WiFi50mb',
  'WiFi250mb',
]);

const rows = ref<
  Array<{
    ExtraBlanket: string;
    NextSeatFree: string;
    TwoNeighboringSeatsFree: string;
    TabletRental: string;
    LaptopRental: string;
    LoungeAccess: string;
    SoftDrinks: string;
    PremiumHeadphonesRental: string;
    ExtraBag: string;
    FastCheckinLane: string;
    WiFi50mb: string;
    WiFi250mb: string;
  }>
>([]);

onMounted(async () => {
  await authGet('/Amenities/MakeReport').then((response) => {
    const tableArray = response.data.data;
    rows.value.push({
      ExtraBlanket: tableArray['Extra Blanket'],
      NextSeatFree: tableArray['Next Seat Free'],
      TwoNeighboringSeatsFree: tableArray['Two Neighboring Seats Free'],
      TabletRental: tableArray['Tablet Rental'],
      LaptopRental: tableArray['Laptop Rental'],
      LoungeAccess: tableArray['Lounge Access'],
      SoftDrinks: tableArray['Soft Drinks'],
      PremiumHeadphonesRental: tableArray['Premium Headphones Rental'],
      ExtraBag: tableArray['Extra Bag'],
      FastCheckinLane: tableArray['Fast Checkin Lane'],
      WiFi50mb: tableArray['Wi-Fi 50 mb'],
      WiFi250mb: tableArray['Wi-Fi 250 mb']
    });
  });
});
</script>
