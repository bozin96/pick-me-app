import { Subject } from 'rxjs';
import { Notification } from '../types/index';

export const RidesSearchResultsSubject = new Subject<any>();
export const RideSearchDataSubject = new Subject<any>();
export const waypointsSubject = new Subject<any>();
export const userInfoSubject = new Subject<any>();
export const notificationSubject$ = new Subject<Notification>();
