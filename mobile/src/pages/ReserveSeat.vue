<template>
  <div class="main">
    <div class="ticket-container">
      Ticket number:
      <input
          type="text"
          v-model="passengerClass"
          placeholder="Enter the ticket number"
      />
      <button class="button-next" @click="updateImage">Next</button>
    </div>

    <div class="plane_img_container">
      <div v-if="showImage && showButtons" class="img_plane">
        <img :src="seatImage" alt="" class="plane_img_fc">
        <button
            v-if="showButtons && passengerClass === '3'"
            v-for="(seat, index) in seatConfig.firstClass"
            :key="index"
            @click="reserveSeatFirst(seat.row, seat.column)"
            class="reserve-button__first"
            :style="{
              position: 'absolute',
              top: seat.top,
              left: seat.left,
              backgroundImage: getSeatBackgroundImage(seat.column)
            }"
        >
          <div class="label">{{ seat.label }}</div>
        </button>
        <button
            v-if="showButtons && passengerClass === '2'"
            v-for="(seat, index) in seatConfig.businessClass"
            :key="index"
            @click="reserveSeatBusiness(seat.row, seat.column)"
            class="reserve-button__first"
            :style="{
              position: 'absolute',
              top: seat.top,
              left: seat.left,
              backgroundImage: getSeatBackgroundImage(seat.column)
            }"
        >
          <div class="label">{{ seat.label }}</div>
        </button>
      </div>
      <div v-else>
        <p v-if="passengerClass === ''" class="message">
          Enter the ticket number to reserve a seat
        </p>
        <p v-else-if="passengerClass === '1'" class="message">
          Seat reservations are not available for Economy class
        </p>
      </div>
    </div>
  </div>

  <div class="button-container">
    <router-link class="mainmenu-router__link" to="/">
      <button class="back-button">Back</button>
    </router-link>
    <button class="reserve-seat__button">Reserve Seat</button>
  </div>
</template>

<script setup lang="ts">
import {ref} from 'vue';

const passengerClass = ref('');
const showImage = ref(false);
const showButtons = ref(false);
const seatImage = ref();

const seatConfig = {
  firstClass: [
    { row: '1', column: '1', top: '55.6%', left: '24.5%', label: 'A' },
    { row: '2', column: '1', top: '63.6%', left: '24.4%', label: 'A' },
    { row: '3', column: '1', top: '71.8%', left: '24.5%', label: 'A' },

    { row: '1', column: '2', top: '55.6%', left: '57.3%', label: 'C' },
    { row: '2', column: '2', top: '63.6%', left: '57%', label: 'C' },
    { row: '3', column: '2', top: '71.8%', left: '57.3%', label: 'C' },

    { row: '1', column: '3', top: '55.6%', left: '70%', label: 'D' },
    { row: '2', column: '3', top: '63.6%', left: '69.8%', label: 'D' },
    { row: '3', column: '3', top: '71.8%', left: '70%', label: 'D' },
  ],
  businessClass: [
    { row: '1', column: '1', top: '48.7%', left: '20.8%', label: 'A' },
    { row: '2', column: '1', top: '56.8%', left: '20.8%', label: 'A' },
    { row: '3', column: '1', top: '65.3%', left: '20.8%', label: 'A' },
    { row: '4', column: '1', top: '73.9%', left: '20.8%', label: 'A' },

    { row: '1', column: '2', top: '48.7%', left: '33.5%', label: 'B' },
    { row: '2', column: '2', top: '56.8%', left: '33.5%', label: 'B' },
    { row: '3', column: '2', top: '65.3%', left: '33.5%', label: 'B' },
    { row: '4', column: '2', top: '73.9%', left: '33.5%', label: 'B' },

    { row: '1', column: '3', top: '48.7%', left: '56.3%', label: 'C' },
    { row: '2', column: '3', top: '56.8%', left: '56.3%', label: 'C' },
    { row: '3', column: '3', top: '65.3%', left: '56.3%', label: 'C' },
    { row: '4', column: '3', top: '73.9%', left: '56.3%', label: 'C' },

    { row: '1', column: '4', top: '48.7%', left: '69%', label: 'D' },
    { row: '2', column: '4', top: '56.8%', left: '69%', label: 'D' },
    { row: '3', column: '4', top: '65.3%', left: '69%', label: 'D' },
    { row: '4', column: '4', top: '73.9%', left: '69%', label: 'D' },
  ],
};

const updateImage = () => {
  if (passengerClass.value !== '') {
    switch (passengerClass.value) {
      case '1':
        showImage.value = false;
        showButtons.value = false;
        break;
      case '2':
        seatImage.value = '/src/assets/img/BusinessClassSeats.png';
        showImage.value = true;
        showButtons.value = true;
        break;
      case '3':
        seatImage.value = '/src/assets/img/FirstClassSeats.png';
        showImage.value = true;
        showButtons.value = true;
        break;
      default:
        showImage.value = false;
        showButtons.value = false;
        break;
    }
  }
};

const getSeatBackgroundImage = (column: string) => {

  if ((column === '3' && passengerClass.value === '3') ||
      ((column === "2" || column === "4") && passengerClass.value === '2')) {
    return "url('/src/assets/img/right_seat.png')";
  } else {
    return "url('/src/assets/img/left_seat.png')";
  }
};
const reserveSeatBusiness = (row: string, seat: string) => {
  console.log(`Reserving seat BusinessClass ${row}${seat}`);
};

const reserveSeatFirst = (row: string, seat: string) => {
  console.log(`Reserving seat FirstClass ${row}${seat}`);
};
</script>

<style scoped>
* {
  margin: 0;
  padding: 0;
}

.main {
  background-repeat: no-repeat;
  min-height: 600px;
  position: relative;
}

.ticket-container {
  display: flex;
  flex-direction: row;
  justify-content: center;
  margin-top: 20px;
  z-index: 1200;
}

.img_plane {
  position: relative;
}

.label {
  position: relative;
  top: 5px;
  right: 1px;
}

.button-next {
  margin-left: 10px;
  padding: 5px 10px;
  cursor: pointer;
  background-color: white;
}

.mainmenu-router__link {
  text-decoration: none;
  display: block;
  margin: auto;
  width: 200px;
}

.back-button {
  padding: 15px;
  border-radius: 30px;
  font-size: 17px;
  font-weight: 800;
  background-color: white;
  display: block;
  cursor: pointer;
  width: 150px;
  transform: translateX(60%);
  margin-left: 15px;
}

.reserve-seat__button {
  padding: 15px;
  border-radius: 30px;
  font-size: 17px;
  font-weight: 800;
  cursor: pointer;
  width: 150px;
  position: absolute;
  left: 50%;
  transform: translateX(-110%);
  background-color: white;
}

.reserve-button__first {
  background-color: inherit;
  font-weight: 900;
  border: none;
  padding: 13px;
  border-radius: 5px;
  cursor: pointer;
  background-repeat: no-repeat;
}

.reserve-button__first:hover {
  opacity: 0.8;
}

.message {
  font-weight: 900;
  position: relative;
}

.button-container {
  display: flex;
  flex-direction: row;
  align-items: center;
  width: 99%;
  position: absolute;
  bottom: 20px;
  z-index: 1000;
}

.button-next {
  margin-left: 10px;
  padding: 5px 10px;
  cursor: pointer;
  background-color: white;
}

.plane_img_container {
  display: flex;
  justify-content: center;
  bottom: 200px;
  overflow: hidden;
}

.plane_img_fc {
  margin-bottom: 120px;
  margin-top: 5px;
  margin-left: 8%;
}
</style>