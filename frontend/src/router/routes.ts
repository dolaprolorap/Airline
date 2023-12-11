import { RouteRecordRaw } from 'vue-router';
import LoginPage from 'pages/Login-Page.vue';
import AdminMenu from 'pages/Admin-Menu.vue';

const routes: RouteRecordRaw[] = [
  {
    path: '/',
    component: LoginPage,
  },
  {
    path: '/Admin-Menu/',
    component: AdminMenu,
  },
];

export default routes;
