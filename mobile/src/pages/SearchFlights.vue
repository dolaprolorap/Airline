<template>
<div class="logo"></div>
<div class="form-container">
    
    <div class="form-content">
        <select v-model="departurePoint">
            <option value="">From</option>
            <option value="City1">City 1</option>
            <option value="City2">City 2</option>
        </select>

        <select v-model="arrivalPoint">
            <option value="">To</option>
            <option value="CityA">City A</option>
            <option value="CityB">City B</option>
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
  import { ref, computed } from 'vue';

  const departurePoint = ref('');
  const arrivalPoint = ref('');
  const departureDate = ref('');
  const shouldSearch = ref(false);
  const selectedDate = ref('');
  const flights = ref([
    {
      id: 1,
      flightNumber: '001',
      price: 100,
      time: '12:00',
      aircraft: 'XXXXXXX',
      date: '2023-12-15'
    },
    {
      id: 2,
      flightNumber: '002',
      price: 120,
      time: '14:30',
      aircraft: 'XXXXXXX',
      date: '2023-12-16'
    },
    {
      id: 3,
      flightNumber: '003',
      price: 90,
      time: '08:45',
      aircraft: 'XXXXXXX',
      date: '2023-12-16'
    },
    {
      id: 4,
      flightNumber: '004',
      price: 110,
      time: '10:15',
      aircraft: 'XXXXXXX',
      date: '2023-12-17'
    },

  ]);

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

.search{
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

.list_of_flights li:last-child{
    border-bottom: none;
}

.flight{
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