/* eslint-disable max-len */
import { AxiosResponse } from 'axios';
import { defer, map, Observable } from 'rxjs';
import axiosInstace from '../api/axiosInstance';
import {
    AuthApiResponse, ChatInteface, ChatMessageInteface, MyDriveInterface, MyRideInterface, User, UserLoginInteface, UserRegisterInteface,
} from '../types/index';

const login = (data: UserLoginInteface): Observable<AuthApiResponse> => defer(() => axiosInstace.post('/auth/login', data)).pipe(map((res) => res.data as AuthApiResponse));

const register = (data: UserRegisterInteface): Observable<AuthApiResponse> => defer(() => axiosInstace.post<AuthApiResponse>('/auth/register', data)).pipe(map((res) => res.data as AuthApiResponse));

const getRides = (params: any = null): Observable<any> => defer(() => axiosInstace.get('/rides', {
    params,
})).pipe(map((response): any => response.data));

const createRide = (data: any): Observable<any> => defer(() => axiosInstace.post('/rides', data)).pipe(map((response): any => response.data));

const requestRide = (rideId: string, data: any): Observable<any> => defer(() => axiosInstace.post(`rides/${rideId}/request`, data)).pipe(map((response): any => response.data));

const getNotifications = (userId: string): Observable<Notification[]> => defer(() => axiosInstace.get(`/users/${userId}/notifications`)).pipe(map((response:AxiosResponse):Notification[] => response.data as Notification[]));

const reviewRideRequest = (rideId: any, data: any): Observable<any> => defer(() => axiosInstace.post(`/rides/${rideId}/response`, data)).pipe();

const getUserRidesAsDriver = (userId: string, params: any): Observable<MyDriveInterface[]> => defer(() => axiosInstace.get(`/users/${userId}/rides-as-driver`, { params })).pipe(map((res: AxiosResponse) => res.data as MyDriveInterface[]));

const getUserRidesAsPassenger = (userId: string, params: any = null): Observable<MyRideInterface[]> => defer(() => axiosInstace.get(`/users/${userId}/rides-as-passenger`, { params })).pipe(map((res: AxiosResponse) => res.data as MyRideInterface[]));

const getUser = (userId: string): Observable<User> => defer(() => axiosInstace.get(`/users/${userId}`)).pipe(map((res: AxiosResponse) => res.data as User));

const updateUser = (userId: string, data: Partial<User>): Observable<User> => defer(() => axiosInstace.put(`/users/${userId}`, data)).pipe(map((res: AxiosResponse) => res.data as User));

const getChats = (searchQuery :string | undefined): Observable<ChatInteface[]> => defer(() => axiosInstace.get('/chats', { params: { searchQuery } })).pipe(map((res: AxiosResponse) => res.data as ChatInteface[]));

const getChatMessages = (chatId: string, params:any): Observable<ChatMessageInteface[]> => defer(() => axiosInstace.get(`/chats/${chatId}/messages`, { params })).pipe(map((res: AxiosResponse) => res.data as ChatMessageInteface[]));

const rateRide = (rideId: string, rate: number, id: string): Observable<any> => defer(() => axiosInstace.post(`/rides/${rideId}/rate`, {
    rate,
    id,
}));

const getOrCreateChat = (userId:string):Observable<any> => defer(() => axiosInstace.post('/chats', { userId })).pipe(map((res:AxiosResponse) => res.data));

const openChat = (chatId:string, hasnumberOfUnreadedMessages:boolean):Observable<any> => defer(() => axiosInstace.post('/openChat', { chatId, hasnumberOfUnreadedMessages })).pipe(map((res:AxiosResponse) => res.data));

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
