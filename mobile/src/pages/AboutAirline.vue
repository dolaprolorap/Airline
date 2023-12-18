<template>
  <div class="main">
    <div class="text-window">
      <p class="about">
        AMONIC Airlines is a new airline based in Abu Dhabi, UAE and is trying to bring a flying option to those with
        fun and active lifestyles, with brand new planes, attractive fares, top-notch service, and a host of fun,
        innovative amenities. We plan to reinvent regional air travel in our image.
        The AMONIC Airlines mission statement is to connect people to what is important in their lives through friendly,
        reliable, and low-cost air travel. While keeping our status as the region’s youngest and most vibrant, AMONIC
        Airlines makes flying more fun.
      </p>
    </div>
  </div>
  <router-link class="mainmenu-router__link" to="/">
    <button class="back-button">Back</button>
  </router-link>
</template>

<script setup lang="ts">
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
  background-image: url('/src/assets/img/plane_1.png');
  background-repeat: no-repeat;
  background-position: center top;
  background-size: 400px;
  width: 100%;
  min-height: 620px;
  position: relative;
}

.text-window {
  height: 50%;
  padding: 5px;
  width: 350px;
  background-color: white;
  position: absolute;
  bottom: 100px;
  left: 50%;
  transform: translateX(-50%);
  border: 2px solid rgb(006, 075, 102);
  overflow-x: hidden;
  overflow-y: scroll;
  margin-top: 50px;
}

.about {
  font-size: 15px;
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
  margin-top: 80px;
  background-color: white;
  width: 350px;
  display: block;
  cursor: pointer;
  position: relative;
  bottom: 100px;
}
</style>