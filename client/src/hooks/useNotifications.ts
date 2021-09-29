/* eslint-disable max-len */
// const URL = `${process.env.REACT_APP_BE_NOT_URL}`;
import { HttpTransportType, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { useEffect, useState } from 'react';
import { toast } from 'react-toastify';
import { NotificationsFetchSubject, NotificationsSubject } from '../common/observers';
import ToastMessage from '../components/ToastMessage';
import ApiService from '../services/Api.service';
import CredentialsService from '../services/Credentials.service';
import { Notification } from '../types';

const url = process.env.REACT_APP_BE_URL;

export default (shoudStart: boolean | null):void => {
    const [notifications, setNotificaitons] = useState<Notification[]>([]);
    const [connection, setConenection] = useState<any>();

    useEffect(() => {
        ApiService.getNotifications(CredentialsService.getUserId()).subscribe((res) => {
            setNotificaitons(res as any);
        });
    }, []);

    useEffect(() => NotificationsSubject.next(notifications), [notifications]);

    useEffect(() => {
        NotificationsFetchSubject.subscribe(() => NotificationsSubject.next(notifications));
    },
    [notifications]);

    useEffect(() => {
        const establishConnection = (): void => {
            const conn = new HubConnectionBuilder().withUrl(`${url}/notifications`, {
                skipNegotiation: true,
                transport: HttpTransportType.WebSockets,
                accessTokenFactory: () => CredentialsService.getToken(),
            })
                .configureLogging(LogLevel.Information)
                .withAutomaticReconnect()
                .build();

            conn.on('RequestForRide', (notification: Notification): void => {
                setNotificaitons((prev: Notification[]): Notification[] => ([notification, ...prev]));
                toast(ToastMessage, { data: notification });
            });
            conn.on('ResponseOnRideRequest', (notification: Notification): void => {
                setNotificaitons((prev: Notification[]): Notification[] => ([notification, ...prev]));
                toast(ToastMessage, { data: notification });
            });
            conn.on('RideReview', (notification: Notification): void => {
                toast(ToastMessage, { data: notification });
                setNotificaitons((prev: Notification[]): Notification[] => ([notification, ...prev]));
                toast('Wow so easy RideReview!');
            });
            conn
                .start()
                .then(() => {
                    setConenection(conn);
                })
                .catch((err: any) => console.log(`Unable to start Connection: ${err}`));
        };

        if (!connection?.connectionStarted && shoudStart) {
            console.log('asd da li');

            establishConnection();
        }
    }, [connection, shoudStart]);
};
