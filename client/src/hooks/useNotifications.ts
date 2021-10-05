import { toast } from 'react-toastify';
/* eslint-disable max-len */
import {
    fromEvent, map, merge, Observable, tap,
} from 'rxjs';
import ToastMessage from '../components/ToastMessage';
import NotificationService from '../services/Notification.service';

export default (): Observable<any> => {
    const conn = NotificationService.connection as any;
    const RequestForRide$ = fromEvent(conn, 'RequestForRide');
    const ResponseOnRideRequest$ = fromEvent(conn, 'ResponseOnRideRequest');
    const RideReview$ = fromEvent(conn, 'RideReview');

    const displayNotificationAsToast = (notification: Notification): void => {
        toast(ToastMessage, { data: notification });
    };
    return merge(
        RequestForRide$.pipe(map((res) => res as Notification), tap((res) => displayNotificationAsToast(res as Notification))),
        ResponseOnRideRequest$.pipe(map((res) => res as Notification), tap((res) => displayNotificationAsToast(res as Notification))),
        RideReview$.pipe(map((res) => res as Notification), tap((res) => displayNotificationAsToast(res as Notification))),
    ) as Observable<Notification>;
};
