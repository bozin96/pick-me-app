import axios from 'axios';

const httpClient = axios.create({
    baseURL: 'http://localhost:3000',
});

httpClient.interceptors.request.use(
    (config: any) =>
        // Do something before request is sent
        // If the header does not contain the token and the url not public, redirect to login

        // const { token } = credentialsService;
        // if (token && config.method !== 'OPTIONS') {
        //     config.headers.Authorization = `Bearer ${token}`;
        // }

        config,
    (error) => Promise.reject(error),
);

httpClient.interceptors.response.use(null, (error) => {
    if (error.response && [401, 403].includes(error.response.status)) {
        if (error.response.data?.error === 'Wrong username or password') {
            console.error(error.response.data);
        } else {
            // credentialsService.removeAuthBody();
            // history.push('/login');
        }
    }
    return Promise.reject(error);
});

export default httpClient;
