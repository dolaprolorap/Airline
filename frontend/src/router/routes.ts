import { RouteRecordRaw } from 'vue-router';

const routes: RouteRecordRaw[] = [
  {
    path: '/',
    component: () => import('layouts/LoginLayout.vue'),
    children: [
      {
        path: '',
        component: () => import('pages/LoginPage.vue'),
      },
    ],
  },
  {
    path: '/',
    component: () => import('layouts/AuthedLayout.vue'),
    children: [
      {
        path: 'admin',
        component: () => import('pages/AdminPage.vue'),
      },
      {
        path: 'user',
        component: () => import('pages/UserPage.vue'),
      },
      {
        path: 'flight-manager',
        component: () => import('pages/FlightManagerPage.vue'),
      },
      {
        path: 'book-flights',
        component: () => import('pages/BookFlightsPage.vue'),
      },
      {
        path: 'survey',
        component: () => import('pages/SurveyPage.vue'),
      },
      {
        path: 'amenties',
        component: () => import('pages/AmentiesPage.vue'),
      }
    ],
  },
  {
    path: '/:catchAll(.*)*',
    component: () => import('pages/ErrorNotFound.vue'),
  },
];

export default routes;
