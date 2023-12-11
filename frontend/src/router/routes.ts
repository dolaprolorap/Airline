import {RouteRecordRaw} from 'vue-router';

const routes: RouteRecordRaw[] = [
  {
    path: '/',
    component: () => import('layouts/LoginLayout.vue'),
    children: [
      {
        path: '',
        component: () => import('pages/LoginPage.vue')
      },
    ]
  },
  {
    path: '/',
    component: () => import('layouts/AuthedLayout.vue'),
    children: [
      {
        path: 'admin',
        component: () => import('pages/AdminMenu.vue')
      }
    ]
  },
  {
    path: '/:catchAll(.*)*',
    component: () => import('pages/ErrorNotFound.vue')
  }
];

export default routes;