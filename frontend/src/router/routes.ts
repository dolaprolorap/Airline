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
        path: 'flightmanager',
        component: () => import('pages/FlightEditor.vue'),
      },
      {
        path: 'book-flights',
        component: () => import('pages/SearchFlightsPage.vue'),
      },
      {
        path: 'summary-survey',
        component: () => import('pages/FlightSurvey.vue'),
      },
      {
        path: 'detailed-survey',
        component: () => import('pages/DetailedSurvey.vue'),
      },
    ],
  },
  {
    path: '/:catchAll(.*)*',
    component: () => import('pages/ErrorNotFound.vue'),
  },
];

export default routes;
