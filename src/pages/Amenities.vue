<template>
    <div class="main">
        <div class="text-window">
            <ul>
                <li v-for="(item, index) in itemList" :key="index">
                    <span class="service-name"> 
                      <strong class="service">Service:</strong>
                      {{ item.service }}
                    </span>
                    <span class="service-cost">{{ item.cost === 0 ? 'Free' : '$' + item.cost }}</span>
                </li>
            </ul>
        </div>
    </div>
    <router-link class="mainmenu-router__link" to="/">
      <button class="back-button">Back</button>
    </router-link>
</template>
  
<script setup lang="ts">
  import { ref } from 'vue';
  
  const initialList = [
    { service: 'Extra Blanket', cost: 10 },
    { service: 'Next Seat Free', cost: 30 },
    { service: 'Two Neighboring Seats Free', cost: 50 },
    { service: 'Tablet Rental', cost: 12 },
    { service: 'Laptop Rental', cost: 15 },
    { service: 'Lounge Access', cost: 25 },
    { service: 'Soft Drinks', cost: 0 },
    { service: 'Premium Headphones Rental', cost: 5 },
    { service: 'Extra Bag', cost: 15 },
    { service: 'Fast Checkin Lane', cost: 10 },
    { service: 'Wi-Fi 50 mb', cost: 0 },
    { service: 'Wi-Fi 250 mb', cost: 25 },
  ];
  
  const itemList = ref(initialList);
</script>

<style scoped>
* {
  margin: 0;
  padding: 0;
}

p {
  margin: 0;
  padding: 0;
}

.main {
  background-image: url('/src/assets/img/logo_1.png');
  background-repeat: no-repeat;
  background-position: center top;
  width: 100%;
  min-height: 500px;
  margin-top: 40px;
  position: relative;
}

.text-window {
  height: 70%;
  padding: 5px;
  background-color: white;
  position: absolute;
  bottom: 0;
  left: 50%;
  transform: translateX(-50%);
  width: 95%;
  border: 2px solid black;
  overflow-x: hidden;
  overflow-y: scroll;
}

.text-window ul {
  list-style-type: none;
}

.text-window li {
  border-bottom: 2px solid black;
  padding: 10px;
  overflow: hidden;
}

.text-window li:last-child{
    border-bottom: none;
}

.service-name {
  float: left;
  font-weight: bold;
}

.service-cost {
  float: right;
  font-weight: bold;
}

.about {
  font-size: 15px;
}

.back-button {
  margin-top: 30px;
  margin-left: 12px;
  padding: 15px 90px;
  border-radius: 30px;
  font-size: 17px;
  font-weight: 800;
  background-color: white;
  width: 350px;
  display: inline-block;
  text-align: center;
  cursor: pointer;
}

.service{
  color: grey;
}
</style>