/* eslint-disable no-param-reassign */
import Axios from 'axios';
import CredentialsService from '../services/Credentials.service';

const httpClient = Axios.create({
  baseURL: 'http://e09c-178-17-18-13.ngrok.io/api/',

});

httpClient.interceptors.request.use(
  (config: any) => {
    // Do something before request is sent
    // If the header does not contain the token and the url not public, redirect to login

    const token = CredentialsService.getToken();
    if (token && config.method !== 'OPTIONS') {
      config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
  },
  (error) => Promise.reject(error),
);

httpClient.interceptors.response.use(
  (result: any) => result,
  (error: any) => {
    if (error.response && [401, 403].includes(error.response.status)) {
      if (error.response.data?.error === 'Wrong username or password') {
        console.error(error.response.data);
      } else {
        // CredentialsService.setToken('');
        // history.push('/auth');
      }
    }
    if (error.response) {
      //   toast.error('Request failed. Please try again or contact administrator.');
    }
    return Promise.reject(error);
  },
);

export default httpClient;
