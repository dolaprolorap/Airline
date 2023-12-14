<template>
  <div class="logo"></div>
  <div class="form-container">
    <div class="form-content">
      <select>
        <option value="">From</option>
        <option v-for="airport in airports" :key="airport.departurePoint" :value="airport.departurePoint">{{ airport.departurePoint }}</option>
      </select>

      <select>
        <option value="">To</option>
        <option v-for="airport in airports" :key="airport.arrivalPoint" :value="airport.arrivalPoint">{{ airport.arrivalPoint }}</option>
      </select>

      <div class="date-input-container">
        <label for="departureDate" :class="{ 'label-hidden': departureDate }">XX.XX.XXXX</label>
        <input
            id="departureDate"
            type="date"
            :value="departureDate"
            @input="updateDate"
        />
      </div>

      <button class="search" @click="searchFlights">Search Flights</button>

      <div class="list_of_flights">
        <ul>
          <li v-for="flight in filteredFlights" :key="flight.id" class="flight">
            <div class="flight-details">
              <div class="flight-info">✈︎ flight number : {{ flight.flightNumber }}</div>
              <div class="flight-info">Price : {{ flight.price }}$</div>
              <div class="flight-info">Time : {{ flight.time }} Aircraft : {{ flight.aircraft }}</div>
            </div>
            <div class="flight-date">📅 {{ flight.date }}</div>
          </li>
        </ul>
      </div>
    </div>
  </div>
  <router-link class="mainmenu-router__link" to="/">
    <button class="back-button">Back</button>
  </router-link>
</template>

<script setup lang="ts">
import {ref, computed, onMounted} from 'vue';
import axios from 'axios';

const airports = ref([] as any[]);
const departureDate = ref('');
const shouldSearch = ref(false);
const selectedDate = ref('');
const flights = ref([] as any[]);

const updateDate = (event: Event) => {
  const target = event.target as HTMLInputElement;
  departureDate.value = target.value;
};

const searchFlights = () => {
  shouldSearch.value = true;
  if (shouldSearch.value) {
    selectedDate.value = departureDate.value;
  }
};

const filteredFlights = computed(() => {
  return shouldSearch.value && selectedDate.value
      ? flights.value.filter((flight) => flight.date === selectedDate.value)
      : flights.value;
});

const fetchFlights = async () => {
  try {
    const response = await axios.get('http://localhost:5033/api/Schedule/GetSchedule');
    flights.value = response.data.data.map((apiFlight: {
      flightNumber: string;
      economyPrice: number;
      time: string;
      aircraft: string;
      date: string;
    }) => ({
      id: apiFlight.flightNumber,
      flightNumber: apiFlight.flightNumber,
      price: apiFlight.economyPrice,
      time: apiFlight.time,
      aircraft: apiFlight.aircraft,
      date: apiFlight.date,
    }));

    airports.value = response.data.data.map((apiAirports: {
      fromAirportName: string,
      toAirportName: string,
    }) => ({
      departurePoint: apiAirports.fromAirportName,
      arrivalPoint: apiAirports.toAirportName,
    }));
  } catch
      (error) {
    console.error('Failed to load flights:', error);
  }
}

onMounted(fetchFlights);
</script>

<style scoped>
.logo {
  background-image: url('/src/assets/img/logo_1.png');
  background-repeat: no-repeat;
  background-position: center top;
  width: 100%;
  min-height: 500px;
  margin-top: 40px;
}

.form-container {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 50%;
  position: relative;
  bottom: 350px;
}

.form-content {
  display: flex;
  flex-direction: column;
  align-items: center;
}

select, input {
  margin-bottom: 10px;
  width: 350px;
  height: 30px;
  border: none;
  border-bottom: 1.5px solid black;
  background-color: white;
}

.date-input-container {
  position: relative;
  font-size: 13px;
}

.date-input-container label {
  position: absolute;
  top: 7px;
  left: 5px;
  pointer-events: none;
  color: black;
}

.date-input-container input {
  width: 350px;
  height: 30px;
  border: none;
  border-bottom: 1.5px solid black;
  background-color: white;

}

.label-hidden {
  display: none;
}

.search {
  padding: 10px 100px;
  font-size: 17px;
  font-weight: 800;
  background-color: white;
  width: 350px;
  display: inline-block;
  text-align: center;
  cursor: pointer;
}

.mainmenu-router__link {
  text-decoration: none;
  display: block;
  width: 350px;
  margin: auto;
}

.back-button {
  padding: 15px 90px;
  border-radius: 30px;
  font-size: 17px;
  font-weight: 800;
  background-color: white;
  width: 350px;
  display: block;
  margin: auto;
  cursor: pointer;
}

.list_of_flights {
  max-height: 200%;
  display: block;
  position: absolute;
  top: 170px;
  overflow-y: scroll;
  left: 50%;
  transform: translateX(-53%);
  width: 410px;
  right: 5px;
}

.list_of_flights ul {
  list-style-type: none;
}

.list_of_flights li {
  border-bottom: 2px solid black;
  padding: 10px;
  overflow: hidden;
}

.list_of_flights li:last-child {
  border-bottom: none;
}

.flight {
  font-size: 15px;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.flight-info {
  margin-bottom: 5px;
}

.flight-date {
  position: relative;
  white-space: nowrap;
  margin-bottom: 60px;
}
</style>