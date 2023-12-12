<template>
  <div class="main">
    <div class="button-column">
      <router-link class="about-router__link" to="/search-flights">
          <button>Search Flights</button>
      </router-link>
      <router-link class="about-router__link" to="/reserve-seat">
        <button>Reserve Seat</button>
      </router-link>
      <router-link class="about-router__link" to="/amenities">
          <button>Amenities</button>
      </router-link>
      <router-link class="about-router__link" to="/about-airline">
          <button>About Our Airline</button>
      </router-link>
    </div>
  </div>
</template>

<script setup lang="ts">
</script>

<style scoped>
.main {
  background-image: url('/src/assets/img/logo_1.png');
  background-repeat: no-repeat;
  background-position: center top;
  width: 100%;
  min-height: 500px;
  margin-top: 40px;
}

.button-column {
  display: flex;
  flex-direction: column;
  align-items: center;
  position: relative;
  top: 200px;
  width: 100%;
}

.button-column button {
  margin-bottom: 50px;
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
</style>