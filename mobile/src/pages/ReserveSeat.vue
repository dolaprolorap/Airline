<template>
  <div class="main">
    <div :class="['ticket-container', { 'center-input': !showImage, 'error-input': showError }]">
      <input
          type="text"
          v-model="ticketNumber"
          placeholder="Enter the ticket number"
      />
      <button class="button-next" @click="fetchTicketInfo">Next</button>
    </div>

    <div class="plane_img_container">
      <div v-if="showImage && showButtons" class="img_plane">
        <img :src="seatImage" alt="" class="plane_img_fc">
        <button
            v-if="showButtons && passengerClass === 'Business'"
            v-for="(seat, index) in seatConfig.businessClass"
            :key="index"
            @click="reserveSeatBusiness(seat.row, seat.column)"
            class="reserve-button__first"
            :style="{
              position: 'absolute',
              top: seat.top,
              left: seat.left,
              backgroundImage: getSeatBackgroundImage(seat.column),
              opacity: isSeatReserved(seat.row, seat.column) ? 0.5 : 1,
              pointerEvents: isSeatReserved(seat.row, seat.column) ? 'none' : 'auto',
            }"
            :disabled="isSeatReserved(seat.row, seat.column)"
        >
          <div class="label">{{ seat.label }}</div>
        </button>

        <button
            v-if="showButtons && passengerClass === 'First Class'"
            v-for="(seat, index) in seatConfig.firstClass"
            :key="index"
            @click="reserveSeatFirst(seat.row, seat.column)"
            class="reserve-button__first"
            :style="{
              position: 'absolute',
              top: seat.top,
              left: seat.left,
              backgroundImage: getSeatBackgroundImage(seat.column),
              opacity: isSeatReserved(seat.row, seat.column) ? 0.5 : 1,
              pointerEvents: isSeatReserved(seat.row, seat.column) ? 'none' : 'auto',
            }"
            :disabled="isSeatReserved(seat.row, seat.column)"
        >
          <div class="label">{{ seat.label }}</div>
        </button>
      </div>
      <div v-else>
        <p v-if="!showImage && passengerClass !== 'Economy'" class="message">
          Enter the ticket number to reserve a seat
        </p>
        <p v-if="passengerClass === 'Economy'" class="message">
          Seat reservations are not available for Economy class
        </p>
      </div>
    </div>
  </div>
  <div v-if="selectedSeat !== ''" class="message_reserve">
    Reserved seat: {{ selectedSeat }}
  </div>
  <div class="button-container">
    <router-link class="mainmenu-router__link" to="/">
      <div class="back-button-container">
        <button class="back-button" :class="{ 'wide-back-button': !isReserveSeatVisible }">Back</button>
      </div>
    </router-link>
    <button v-if="isReserveSeatVisible" class="reserve-seat__button">Reserve Seat</button>
  </div>
</template>

<script setup lang="ts">
import {ref} from 'vue';
import axios from 'axios';

const passengerClass = ref('');
const showImage = ref(false);
const showButtons = ref(false);
const seatImage = ref();
const selectedSeat = ref('');
const isReserveSeatVisible = ref(false);
const showError = ref(false);
const ticketNumber = ref('');

interface ReservedSeat {
  row: string;
  column: string;
}

const reservedSeats = ref<ReservedSeat[]>([]);

const seatConfig = {
  firstClass: [
    {row: '1', column: '1', top: '55.6%', left: '24.5%', label: 'A', reserved: false},
    {row: '2', column: '1', top: '63.6%', left: '24.4%', label: 'A', reserved: false},
    {row: '3', column: '1', top: '71.8%', left: '24.5%', label: 'A', reserved: false},

    {row: '1', column: '2', top: '55.6%', left: '57.3%', label: 'C', reserved: false},
    {row: '2', column: '2', top: '63.6%', left: '57%', label: 'C', reserved: false},
    {row: '3', column: '2', top: '71.8%', left: '57.3%', label: 'C', reserved: false},

    {row: '1', column: '3', top: '55.6%', left: '70%', label: 'D', reserved: false},
    {row: '2', column: '3', top: '63.6%', left: '69.8%', label: 'D', reserved: false},
    {row: '3', column: '3', top: '71.8%', left: '70%', label: 'D', reserved: false},
  ],
  businessClass: [
    {row: '4', column: '1', top: '48.7%', left: '20.8%', label: 'A', reserved: false},
    {row: '5', column: '1', top: '56.8%', left: '20.8%', label: 'A', reserved: false},
    {row: '6', column: '1', top: '65.3%', left: '20.8%', label: 'A', reserved: false},
    {row: '7', column: '1', top: '73.9%', left: '20.8%', label: 'A', reserved: false},

    {row: '4', column: '2', top: '48.7%', left: '33.5%', label: 'B', reserved: false},
    {row: '5', column: '2', top: '56.8%', left: '33.5%', label: 'B', reserved: false},
    {row: '6', column: '2', top: '65.3%', left: '33.5%', label: 'B', reserved: false},
    {row: '7', column: '2', top: '73.9%', left: '33.5%', label: 'B', reserved: false},

    {row: '4', column: '3', top: '48.7%', left: '56.3%', label: 'C', reserved: false},
    {row: '5', column: '3', top: '56.8%', left: '56.3%', label: 'C', reserved: false},
    {row: '6', column: '3', top: '65.3%', left: '56.3%', label: 'C', reserved: false},
    {row: '7', column: '3', top: '73.9%', left: '56.3%', label: 'C', reserved: false},

    {row: '4', column: '4', top: '48.7%', left: '69%', label: 'D', reserved: false},
    {row: '5', column: '4', top: '56.8%', left: '69%', label: 'D', reserved: false},
    {row: '6', column: '4', top: '65.3%', left: '69%', label: 'D', reserved: false},
    {row: '7', column: '4', top: '73.9%', left: '69%', label: 'D', reserved: false},
  ],
};

const fetchTicketInfo = async () => {
  if (ticketNumber.value !== '') {
    try {
      const response = await axios.get(`http://localhost:5033/api/Ticket/Ticket?ticketId=${ticketNumber.value}`);
      const responseSeats = await axios.get(`http://localhost:5033/api/Seats/Seats?ticketId=${ticketNumber.value}`);
      const ticketData = response.data.data;
      const seatNumber = ticketData.seatNumber || '';
      console.log(seatNumber);
      reservedSeats.value = responseSeats.data.data.map((seat: number) => {
        const column = (seat % 10).toString();
        const row = Math.floor(seat / 10).toString();
        return {row, column};
      });
      passengerClass.value = ticketData.cabinType;
      switch (ticketData.cabinType) {
        case 'Business':
          seatImage.value = '/src/assets/img/BusinessClassSeats.png';
          showImage.value = true;
          showButtons.value = true;
          isReserveSeatVisible.value = true;
          showError.value = false;
          break;
        case 'First Class':
          seatImage.value = '/src/assets/img/FirstClassSeats.png';
          showImage.value = true;
          showButtons.value = true;
          isReserveSeatVisible.value = true;
          showError.value = false;
          break;
        case 'Economy':
          showImage.value = false;
          showButtons.value = false;
          isReserveSeatVisible.value = false;
          showError.value = false;
          break;
        default:
          showImage.value = false;
          showButtons.value = false;
          isReserveSeatVisible.value = false;
          break;
      }

      if (seatNumber != 0) {
        selectedSeat.value = `${passengerClass.value}: Row: ${Math.floor(seatNumber / 10).toString()} Seat: ${(seatNumber % 10).toString()}`;
        console.log(selectedSeat.value);
        isReserveSeatVisible.value = false;
      } else {
        isReserveSeatVisible.value = true;
      }
      showError.value = false;
    } catch (error) {
      showError.value = true;
    }
  }
};

const getSeatBackgroundImage = (column: string) => {

  if ((column === '3' && passengerClass.value === 'First Class') ||
      ((column === "2" || column === "4") && passengerClass.value === 'Business')) {
    return "url('/src/assets/img/right_seat.png')";
  } else {
    return "url('/src/assets/img/left_seat.png')";
  }
};

reservedSeats.value.forEach(reservedSeat => {
  const {row, column} = reservedSeat;
  const seatKey = `${row}${column}`;

  const seatObj = passengerClass.value === 'Business' ?
      seatConfig.businessClass.find(seat => seatKey === `${seat.row}${seat.column}`) :
      seatConfig.firstClass.find(seat => seatKey === `${seat.row}${seat.column}`);

  if (seatObj) {
    seatObj.reserved = true;
  }
});

const reserveSeat = async (row: string, seat: string) => {
  if (selectedSeat.value !== '') {
    alert(`You have already selected seat ${selectedSeat.value}.`);
    return;
  }

  const isReserved = reservedSeats.value.some(reservedSeat => (
      reservedSeat.row === row && reservedSeat.column === seat
  ));

  if (!isReserved) {
    try {
      const response = await axios.post('http://localhost:5033/api/Seats/BookSeat', {
        ticketId: parseInt(ticketNumber.value),
        seatNumber: parseInt(row + seat),
      });

      if (response.status === 200) {
        selectedSeat.value = `${passengerClass.value}: Row: ${row} Seat: ${seat}`;
        reservedSeats.value.push({row, column: seat});
        alert(`Row: ${row} Seat: ${seat} reserved successfully!`);
      } else {
        alert('Failed to reserve seat. Please try again.');
      }
    } catch (error) {
      alert('Failed to reserve seat. Please try again.');
    }
  } else {
    alert(`Row: ${row} Seat: ${seat} is already reserved.`);
  }
};

const reserveSeatBusiness = (row: string, seat: string) => {
  reserveSeat(row, seat);
};

const reserveSeatFirst = (row: string, seat: string) => {
  reserveSeat(row, seat);
};

const isSeatReserved = (row: string, column: string) => {
  return reservedSeats.value.some(
      (reservedSeat) => reservedSeat.row === row && reservedSeat.column === column
  );
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
  margin-left: 30px;
}

.img_plane {
  position: relative;
}

.error-input input {
  border: 2px solid rgb(200, 0, 0);
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
  border-radius: 5px;
}

.mainmenu-router__link {
  text-decoration: none;
  display: block;
  margin: auto;
  width: 200px;
}

.center-input {
  display: flex;
  align-items: center;
  position: relative;
  top: 300px;
}

.center-input input {
  font-size: 16px;
  padding: 10px;
  border-radius: 10px;
}

.back-button-container .wide-back-button {
  padding: 15px 90px;
  width: 350px;
  position: relative;
  right: 305px;
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

.reserve-button__first[disabled] {
  opacity: 0.5;
  cursor: not-allowed;
}

.message {
  font-weight: 900;
  position: relative;
  font-size: 18px;
  text-align: center;
}

.button-container {
  display: flex;
  flex-direction: row;
  align-items: center;
  width: 99%;
  position: absolute;
  bottom: 70px;
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

.message_reserve {
  position: relative;
  bottom: 120px;
  display: flex;
  justify-content: center;
  font-size: 15px;
  font-weight: 700;
}
</style>