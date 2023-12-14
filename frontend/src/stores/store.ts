import {defineStore} from 'pinia';
import {api} from 'boot/axios';

export const useAuthStore = defineStore({
    id: 'auth',
    state: () => ({
        accessToken: null as string | null,
        refreshToken: null as string | null,
        isAuthenticated: false,
        username: null as string | null
    }),
    actions: ({
        SetTokens({accessToken, refreshToken}: { accessToken: string; refreshToken: string }) {
            this.accessToken = accessToken;
            this.refreshToken = refreshToken;
            this.isAuthenticated = true;
        },
        SetUsername(username: string | null) {
            this.username = username;
        },
        ResfreshTokens() {
            const authStore = useAuthStore();
            try {
                api.post('/Auth/Refresh', {
                    accessToken: this.accessToken,
                    refreshToken: this.refreshToken
                })
                    .then(response => {
                        const newAccessToken = response.data.data.accessToken;
                        const newRefreshToken = response.data.data.refreshToken;

                        authStore.SetTokens({
                            accessToken: newAccessToken,
                            refreshToken: newRefreshToken
                        });
                    })
            } catch (error) {
                console.log('Error with refreshing', error);
            }
        }
    }),
});
