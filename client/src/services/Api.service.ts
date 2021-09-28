/* eslint-disable max-len */
import { AxiosResponse } from 'axios';
import { defer, map, Observable } from 'rxjs';
import httpClient from '../api/httpClient';
import {
    AuthApiResponse, ChatInteface, ChatMessageInteface, MyDriveInterface, MyRideInterface, User, UserLoginInteface, UserRegisterInteface,
} from '../types/index';

const login = (data: UserLoginInteface): Observable<AuthApiResponse> => defer(() => httpClient.post('/auth/login', data)).pipe(map((res) => res.data as AuthApiResponse));

const register = (data: UserRegisterInteface): Observable<AuthApiResponse> => defer(() => httpClient.post<AuthApiResponse>('/auth/register', data)).pipe(map((res) => res.data as AuthApiResponse));

const getRides = (params: any = null): Observable<any> => defer(() => httpClient.get('/rides', {
    params,
})).pipe(map((response): any => response.data));

const createRide = (data: any): Observable<any> => defer(() => httpClient.post('/rides', data)).pipe(map((response): any => response.data));

const requestRide = (rideId: string, data: any): Observable<any> => defer(() => httpClient.post(`rides/${rideId}/request`, data)).pipe(map((response): any => response.data));

const getNotifications = (userId: string): Observable<any> => defer(() => httpClient.get(`/users/${userId}/notifications`)).pipe(map((response): any => response.data));

const reviewRideRequest = (rideId: any, data: any): Observable<any> => defer(() => httpClient.post(`/rides/${rideId}/response`, data)).pipe();

const getUserRidesAsDriver = (userId: string, params: any): Observable<MyDriveInterface[]> => defer(() => httpClient.get(`/users/${userId}/rides-as-driver`, { params })).pipe(map((res: AxiosResponse) => res.data as MyDriveInterface[]));

const getUserRidesAsPassenger = (userId: string, params: any = null): Observable<MyRideInterface[]> => defer(() => httpClient.get(`/users/${userId}/rides-as-passenger`, { params })).pipe(map((res: AxiosResponse) => res.data as MyRideInterface[]));

const getUser = (userId: string): Observable<User> => defer(() => httpClient.get(`/users/${userId}`)).pipe(map((res: AxiosResponse) => res.data as User));

const updateUser = (userId: string, data: Partial<User>): Observable<User> => defer(() => httpClient.put(`/users/${userId}`, data)).pipe(map((res: AxiosResponse) => res.data as User));

const getChats = (): Observable<ChatInteface[]> => defer(() => httpClient.get('/chats')).pipe(map((res: AxiosResponse) => res.data as ChatInteface[]));

const getChatMessages = (chatId: string): Observable<ChatMessageInteface[]> => defer(() => httpClient.get(`/chats/${chatId}/messages`)).pipe(map((res: AxiosResponse) => res.data as ChatMessageInteface[]));

const rateRide = (rideId: string, rate: number, id: string): Observable<any> => defer(() => httpClient.post(`/rides/${rideId}/rate`, {
    rate,
    id,
}));

const getOrCreateChat = (userId:string):Observable<any> => defer(() => httpClient.post('/chats', { userId })).pipe(map((res:AxiosResponse) => res.data));

const openChat = (chatId:string, hasnumberOfUnreadedMessages:boolean):Observable<any> => defer(() => httpClient.post('/openChat', { chatId, hasnumberOfUnreadedMessages })).pipe(map((res:AxiosResponse) => res.data));

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
    getChats,
    getChatMessages,
    rateRide,
    getOrCreateChat,
    openChat,
};
