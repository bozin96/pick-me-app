/* eslint-disable max-len */
import { AxiosResponse } from 'axios';
import { defer, map, Observable } from 'rxjs';
import httpClient from '../api/httpClient';
import {
    AuthApiResponse, MyRideInterface, User, UserLoginInteface, UserRegisterInteface,
} from '../types/index';

const login = (data: UserLoginInteface): Observable<AuthApiResponse> => defer(() => httpClient.post('/auth/login', data)).pipe(map((res) => res.data as AuthApiResponse));

const register = (data: UserRegisterInteface): Observable<AuthApiResponse> => defer(() => httpClient.post<AuthApiResponse>('/auth/register', data)).pipe(map((res) => res.data as AuthApiResponse));

const getRides = (params: any = null): Observable<any> => defer(() => httpClient.get('/rides', {
    params,
})).pipe(map((response): any => response.data));

const createRide = (data: any): Observable<any> => defer(() => httpClient.post('/rides', data)).pipe(map((response): any => response.data));

const requestRide = (rideId: string, data: any): Observable<any> => defer(() => httpClient.post(`rides/${rideId}/request`, data)).pipe(map((response): any => response.data));

const getNotifications = (userId:string): Observable<any> => defer(() => httpClient.get(`/users/${userId}/notifications`)).pipe(map((response): any => response.data));

const reviewRideRequest = (rideId:any, data:any): Observable<any> => defer(() => httpClient.post(`/rides/${rideId}/response`, data)).pipe();
// api/rides/:rideId/request

const getUserRidesAsDriver = (userId:string, params:any):Observable<MyRideInterface[]> => defer(() => httpClient.get(`/users/${userId}/rides-as-driver`, { params })).pipe(map((res:AxiosResponse) => res.data as MyRideInterface[]));
const getUserRidesAsPassenger = (userId:string, params:any = null):Observable<MyRideInterface[]> => defer(() => httpClient.get(`/users/${userId}/rides-as-passenger`, { params })).pipe(map((res:AxiosResponse) => res.data as MyRideInterface[]));

const getUser = (userId:string):Observable<User> => defer(() => httpClient.get(`/users/${userId}`)).pipe(map((res:AxiosResponse) => res.data as User));

const updateUser = (userId:string, data:Partial<User>):Observable<User> => defer(() => httpClient.put(`/users/${userId}`, data)).pipe(map((res:AxiosResponse) => res.data as User));
export default {
    login,
    getRides,
    register,
    createRide,
    requestRide,
    getNotifications,
    reviewRideRequest,
    getUserRidesAsDriver,
    getUserRidesAsPassenger,
    getUser,
    updateUser,
};
