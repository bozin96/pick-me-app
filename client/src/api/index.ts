/* eslint-disable max-len */
import { defer, map, Observable } from 'rxjs';
import axiosInstace from './axiosInstance';

const get = <T>(url: string, queryParams?: any): Observable<T> => defer(() => axiosInstace.get<T>(url, { params: queryParams }))
    .pipe(map((result) => result.data));

const post = <T>(url: string, body: any, queryParams?: any): Observable<T | void> => defer(() => axiosInstace.post<T>(url, body, { params: queryParams }))
    .pipe(map((result) => result.data));

const put = <T>(url: string, body: any, queryParams?: any): Observable<T | void> => defer(() => axiosInstace.put<T>(url, body, { params: queryParams }))
    .pipe(map((result) => result.data));

const patch = <T>(url: string, body: any, queryParams?: any): Observable<T | void> => defer(() => axiosInstace.patch<T>(url, body, { params: queryParams }))
    .pipe(map((result) => result.data));

const deleteR = <T>(url: string, id: number): Observable<T | void> => defer(() => (axiosInstace.delete(`${url}/${id}`)))
    .pipe(map((result) => result.data));

export default {
    get, post, put, patch, deleteR,
};
