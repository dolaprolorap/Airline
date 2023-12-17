import { api } from 'boot/axios';

import { LocalStorage } from 'quasar';

import { Router } from 'src/router';

export const getNewTokens = async () => {
  if (!LocalStorage.has('accessToken') || !LocalStorage.has('refreshToken')) {

    LocalStorage.set('accessToken', '');
    LocalStorage.set('refreshToken', '');

    await Router.push('/');

    throw Error('Not authorized');
  }

  return api
    .post('/Auth/Refresh', {
      accessToken: LocalStorage.getItem('accessToken'),
      refreshToken: LocalStorage.getItem('refreshToken')
    })
    .then(response => {
      if (response.status !== 200) {
        LocalStorage.set('accessToken', '');
        LocalStorage.set('refreshToken', '');
        Router.push('/').then(() => {
          throw Error('Not authorized');
        });
      }
      const data = response.data;

      LocalStorage.set('accessToken', data.data.accessToken);
      LocalStorage.set('refreshToken', data.data.refreshToken);
    });
};
export const authPost = async (url: string, data: object) => {
  await getNewTokens();

  return api.post(url, data, {
    headers: {
      Authorization: 'Bearer ' + LocalStorage.getItem('accessToken')
    }
  });
};

export const authGet = async (url: string) => {
  await getNewTokens();

  return api.get(url, {
    headers: {
      Authorization: 'Bearer ' + LocalStorage.getItem('accessToken')
    }
  });
};