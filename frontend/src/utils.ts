import { api } from 'boot/axios';

import { LocalStorage } from 'quasar';

import { Router } from 'src/router';

export const exit = () => {
  LocalStorage.remove('accessToken');
  LocalStorage.remove('refreshToken');
  LocalStorage.remove('email');

  Router.push('/');
};

export const getNewTokens = async () => {
  if (!LocalStorage.has('accessToken') || !LocalStorage.has('refreshToken')) {
    exit();

    throw Error('Not authorized');
  }
  return await api
    .post('/Auth/Refresh', {
      accessToken: LocalStorage.getItem('accessToken'),
      refreshToken: LocalStorage.getItem('refreshToken')
    })
    .then(response => {
      const data = response.data;

      LocalStorage.set('accessToken', data.data.accessToken);
      LocalStorage.set('refreshToken', data.data.refreshToken);
    })
    .catch((error) => {
      if (error.response.status !== 200) {
        exit();
        throw Error('Not authorized');
      }
    });
};
export const authPost = async (url: string, data: object) => {
  return api.post(url, data, {
    headers: {
      Authorization: 'Bearer ' + LocalStorage.getItem('accessToken')
    }
  })
    .catch(async error => {
      if (error.response.status === 401) {
        await getNewTokens();
        return await api.post(url, data, {
          headers: {
            Authorization: 'Bearer ' + LocalStorage.getItem('accessToken')
          }
        });
      } else throw `Unhandled error ${error}`;
    });
};

export const authGet = async (url: string) => {
  return api.get(url, {
    headers: {
      Authorization: 'Bearer ' + LocalStorage.getItem('accessToken')
    }
  })
    .catch(async error => {
      if (error.response.status === 401) {
        await getNewTokens();
        return await api.get(url, {
          headers: {
            Authorization: 'Bearer ' + LocalStorage.getItem('accessToken')
          }
        });
      } else throw `Unhandled error ${error}`;
    });
};