<template>
  <div class="main">
    <div class="ticket-container">
      Ticket number:&emsp;
      <input type="text"
             v-model="passengerClass"
             placeholder="Enter the ticket number">
      <button class="button-next"
              @click="updateImage">Next</button>
    </div>
    <div class="plane_img_container">
      <div v-if="showImage">
        <img :src="seatImage" alt="" class="plane_img_fc" usemap="#seatMap">
      </div>
      <div v-else>
        <p v-if="passengerClass === ''"
           class="message">Enter the ticket number to reserve a seat
        </p>
        <p v-else-if="passengerClass === '1'"
           class="message">Seat reservations are not available for Economy class
        </p>
      </div>
    </div>
    <div class="button-container">
      <button class="reserve-seat__button">Reserve Seat</button>
      <router-link class="mainmenu-router__link" to="/">
        <button class="back-button">Back</button>
      </router-link>
    </div>
  </div>
</template>

<script setup lang="ts">
import {ref} from 'vue';

const passengerClass = ref('');
const showImage = ref(false);
const seatImage = ref();

const updateImage = () => {
  switch (passengerClass.value) {
    case '1':
      showImage.value = false;
      break;
    case '2':
      seatImage.value = '/src/assets/img/BusinessClassSeats.png';
      showImage.value = true;
      break;
    case '3':
      seatImage.value = '/src/assets/img/FirstClassSeats.png';
      showImage.value = true;
      break;
    default:
      showImage.value = false;
      break;
  }
};

</script>

<style scoped>
* {
  margin: 0;
  padding: 0;
}

.main {
  background-repeat: no-repeat;
  background-position: center top;
  width: 100%;
  min-height: 600px;
  margin-top: 40px;
  position: relative;
}

.ticket-container {
  display: flex;
  flex-direction: row;
  justify-content: center;
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
  width: 200px;
  right: 50%;
  transform: translateX(60%);
}

.reserve-seat__button {
  padding: 15px;
  border-radius: 30px;
  font-size: 17px;
  font-weight: 800;
  cursor: pointer;
  width: 200px;
  position: absolute;
  left: 50%;
  transform: translateX(-110%);
}

.reserved-seat {
  background-color: #FF0000;
  opacity: 0.5;
}

.message{
  font-weight: 900;
}

.button-container {
  display: flex;
  flex-direction: row;
  align-items: center;
  width: 100%;
  position: absolute;
  bottom: 0;
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
}

.plane_img_fc {
  margin-top: 50px;
  margin-bottom: 120px;
}
</style>