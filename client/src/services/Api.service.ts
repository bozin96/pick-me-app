/* eslint-disable max-len */
import { AxiosResponse } from 'axios';
import {
    from, map, Observable,
} from 'rxjs';
import axiosInstance from '../api/axiosInstance';
import {
    AuthApiResponse, IChat, IChatMessage, MyDriveInterface, MyRideInterface, User, UserLoginInteface, UserRegisterInteface,
} from '../types/index';

const login$ = (data: UserLoginInteface): Observable<AuthApiResponse> => from((axiosInstance.post('/auth/login', data))).pipe(map((response) => response.data as AuthApiResponse));
const register$ = (data: UserRegisterInteface): Observable<AuthApiResponse> => from(axiosInstance.post<AuthApiResponse>('/auth/register', data)).pipe(map((res) => res.data as AuthApiResponse));

const getRides$ = (params: any = null): Observable<any> => from(axiosInstance.get('/rides', {
    params,
})).pipe(map((response): any => response.data));

const createRide$ = (data: any): Observable<any> => from(axiosInstance.post('/rides', data)).pipe(map((response): any => response.data));

const requestRide$ = (rideId: string, data: any): Observable<any> => from(axiosInstance.post(`rides/${rideId}/request`, data)).pipe(map((response): any => response.data));

const getNotifications$ = (userId: string): Observable<Notification[]> => from(axiosInstance.get(`/users/${userId}/notifications`)).pipe(map((response: AxiosResponse): Notification[] => response.data as Notification[]));

const reviewRideRequest$ = (rideId: any, data: any): Observable<any> => from(axiosInstance.post(`/rides/${rideId}/response`, data)).pipe();

const getUserRidesAsDriver$ = (userId: string, params: any): Observable<MyDriveInterface[]> => from(axiosInstance.get(`/users/${userId}/rides-as-driver`, { params })).pipe(map((res: AxiosResponse) => res.data as MyDriveInterface[]));

const getUserRidesAsPassenger$ = (userId: string, params: any = null): Observable<MyRideInterface[]> => from(axiosInstance.get(`/users/${userId}/rides-as-passenger`, { params })).pipe(map((res: AxiosResponse) => res.data as MyRideInterface[]));

const getUser$ = (userId: string): Observable<User> => from(axiosInstance.get(`/users/${userId}`)).pipe(map((res: AxiosResponse) => res.data as User));

const updateUser$ = (userId: string, data: Partial<User>): Observable<User> => from(axiosInstance.put(`/users/${userId}`, data)).pipe(map((res: AxiosResponse) => res.data as User));

const getChats$ = (searchQuery: string | undefined): Observable<IChat[]> => from(axiosInstance.get('/chats', { params: { searchQuery } })).pipe(map((res: AxiosResponse) => res.data as IChat[]));

const getChatMessages$ = (chatId: string, params: any): Observable<IChatMessage[]> => from(axiosInstance.get(`/chats/${chatId}/messages`, { params })).pipe(map((res: AxiosResponse) => res.data as IChatMessage[]));

const rateRide$ = (rideId: string, rate: number, id: string): Observable<any> => from(axiosInstance.post(`/rides/${rideId}/rate`, {
    rate,
    id,
}));

const getOrCreateChat$ = (userId: string): Observable<any> => from(axiosInstance.post('/chats', { userId })).pipe(map((res: AxiosResponse) => res.data));

export default {
    login$,
    getRides$,
    register$,
    createRide$,
    requestRide$,
    getNotifications$,
    reviewRideRequest$,
    getUserRidesAsDriver$,
    getUserRidesAsPassenger$,
    getUser$,
    updateUser$,
    getChats$,
    getChatMessages$,
    rateRide$,
    getOrCreateChat$,
};
