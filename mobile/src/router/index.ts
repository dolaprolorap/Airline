import {createRouter, createWebHistory} from "vue-router";
import MainMenu from '../pages/MainMenu.vue'
import AboutAirline from '../pages/AboutAirline.vue'
import Amenities from '../pages/Amenities.vue'
import SearchFlights from '../pages/SearchFlights.vue'

const routes = [
    {
        path: '/',
        component: MainMenu
    },
    {
        path: '/about-airline',
        component: AboutAirline
    },
    {
        path: '/amenities',
        component: Amenities
    },
    {
        path: '/search-flights',
        component: SearchFlights
    }
]

const router = createRouter({
    routes,
    history: createWebHistory('/subdirectory/'),
});

export default router;