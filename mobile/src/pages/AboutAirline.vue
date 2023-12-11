<template>
    <div class="main">
        <div class="text-window">
            <p class="about">
                AMONIC Airlines is a new airline based in Abu Dhabi, UAE and is trying to bring a flying option to those with fun and active lifestyles, with brand new planes, attractive fares, top-notch service, and a host of fun, innovative amenities. We plan to reinvent regional air travel in our image. 
                The AMONIC Airlines mission statement is to connect people to what is important in their lives through friendly, reliable, and low-cost air travel. While keeping our status as the region’s youngest and most vibrant, AMONIC Airlines makes flying more fun. 
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
*{
    margin: 0;
    padding: 0;
}
p{
    margin: 0;
    padding: 0;
}
.main {
    background-image: url('/src/assets/img/plane_1.png');
    background-repeat: no-repeat;
    background-position: center top;
    width: 100%;
    min-height: 500px;
    margin-top: 40px;
    background-size: 90%;
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
    border: 2px solid  black;
    overflow-x: hidden;
    overflow-y: scroll;
}

.about{
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
</style>