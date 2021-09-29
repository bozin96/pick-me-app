import { Subject } from 'rxjs';
import { Notification } from '../types';

export const RidesSearchResultsSubject = new Subject<any>();
export const RideSearchDataSubject = new Subject<any>();

export const waypointsSubject = new Subject<any>();
export const userInfoSubject = new Subject<any>();
export const newChatMessageSubject = new Subject<any>();
export const newUnreadedMessage = new Subject<any>();

export const NotificationsSubject = new Subject<Notification[]>();
export const NotificationsFetchSubject = new Subject();
