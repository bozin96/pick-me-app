/* eslint-disable max-len */
import { defer, map, Observable } from 'rxjs';
import httpClient from './httpClient';

const get = <T>(url: string, queryParams?: any): Observable<T> => defer(() => httpClient.get<T>(url, { params: queryParams }))
    .pipe(map((result) => result.data));

const post = <T>(url: string, body: any, queryParams?: any): Observable<T | void> => defer(() => httpClient.post<T>(url, body, { params: queryParams }))
    .pipe(map((result) => result.data));

const put = <T>(url: string, body: any, queryParams?: any): Observable<T | void> => defer(() => httpClient.put<T>(url, body, { params: queryParams }))
    .pipe(map((result) => result.data));

const patch = <T>(url: string, body: any, queryParams?: any): Observable<T | void> => defer(() => httpClient.patch<T>(url, body, { params: queryParams }))
    .pipe(map((result) => result.data));

const deleteR = <T>(url: string, id: number): Observable<T | void> => defer(() => (httpClient.delete(`${url}/${id}`)))
    .pipe(map((result) => result.data));

export default {
    get, post, put, patch, deleteR,
};
