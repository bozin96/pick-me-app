import { useEffect, useState } from 'react';
/* eslint-disable max-len */
import {
    fromEvent, map, merge, Observable,
} from 'rxjs';
import NotificationService from '../services/Notification.service';
import { Notification } from '../types/index';

export default (shouldStart = false): Observable<any> | undefined => {
    const conn = NotificationService.connection as any;
    const [RequestForRide$, setRequestForRide] = useState<Observable<Notification>>(new Observable());
    const [ResponseOnRideRequest$, setResponseOnRideRequest] = useState<Observable<Notification>>(new Observable());
    const [RideReview$, setRideReview] = useState<Observable<Notification>>(new Observable());

    useEffect(() => {
        const setUpListeners = (): void => {
            setRequestForRide(fromEvent(conn, 'RequestForRide'));
            setResponseOnRideRequest(fromEvent(conn, 'ResponseOnRideRequest'));
            setRideReview(fromEvent(conn, 'RideReview'));
        };
        if (shouldStart && conn.connectionState === 'Connected') {
            setUpListeners();
        }
    }, [conn, shouldStart]);

    return merge(
        RequestForRide$,
        ResponseOnRideRequest$,
        RideReview$,
    ).pipe(map((res) => res as Notification)) as Observable<Notification>;
};
