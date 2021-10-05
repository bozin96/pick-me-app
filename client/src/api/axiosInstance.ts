/* eslint-disable no-param-reassign */
import Axios, { AxiosError, AxiosRequestConfig } from 'axios';
import { toast } from 'react-toastify';
import CredentialsService from '../services/Credentials.service';

const axiosInstance = Axios.create({
  baseURL: `${process.env.REACT_APP_BE_URL}/api`,

});

axiosInstance.interceptors.request.use(
  (config: AxiosRequestConfig) => {
    // Do something before request is sent

    const token = CredentialsService.getToken();
    if (token && config.method !== 'OPTIONS') {
      config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
  },
  (error) => Promise.reject(error),
);

axiosInstance.interceptors.response.use(
  (result: AxiosRequestConfig) => result,
  (error: AxiosError) => {
    if (error.response && [401, 403].includes(error.response.status)) {
      if (error.response.data?.error === 'Wrong username or password') {
        // eslint-disable-next-line no-console
        console.error(error.response.data);
      } else {
        toast('Unathorized');
        CredentialsService.clearLocalStorage();
      }
    }
    if (error.response) {
      //   toast.error('Request failed. Please try again or contact administrator.');
    }
    return Promise.reject(error);
  },
);

export default axiosInstance;
