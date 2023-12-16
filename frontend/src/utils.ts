import { api } from 'boot/axios';
import { LocalStorage } from 'quasar';
import { useRouter } from 'vue-router';

const getNewTokens = () => {
  if (!LocalStorage.has('accessToken') || !LocalStorage.has('refreshToken')) {
    LocalStorage.set('accessToken', '');
    LocalStorage.set('refreshToken', '');

    useRouter().push('/');

    throw Error('Not authorized');
  }
  console.log({
    accessToken: LocalStorage.getItem('accessToken'),
    refreshToken: LocalStorage.getItem('refreshToken')
  })

  api
    .post('/Auth/Refresh', {
      accessToken: LocalStorage.getItem('accessToken'),
      refreshToken: LocalStorage.getItem('refreshToken')
    })
    .then(response => {
      const data = response.data;
      LocalStorage.set('accessToken', data.data.accessToken);
      LocalStorage.set('refreshToken', data.data.refreshToken);
    });
};
export const authPost = (url: string, data: object) => {
  getNewTokens();

  return api.post(url, data, {
    headers: {
      Authorization: 'Bearer ' + LocalStorage.getItem('accessToken')
    }
  });
};

export const authGet = (url: string) => {
  getNewTokens();

  return api.get(url, {
    headers: {
      Authorization: 'Bearer ' + LocalStorage.getItem('accessToken')
    }
  });
};