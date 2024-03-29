/* eslint-disable max-len */
/* eslint-disable react/jsx-props-no-spreading */
import React, { useEffect, useState } from 'react';
import RideRequest from '../../components/RideRequest';
import RideResponse from '../../components/RideResponse';
import useNotifications from '../../hooks/useNotifications';
import ApiService from '../../services/Api.service';
import CredentialsService from '../../services/Credentials.service';
import { Notification } from '../../types';
import './NotificationPage.styles.scss';

const NotificationPage: React.FC = () => {
    const [notifications, setNotificaitons] = useState<Notification[]>([]);

    const newNotificationObserver$ = useNotifications(true);

    useEffect(() => {
        if (newNotificationObserver$) {
            newNotificationObserver$.subscribe((res) => setNotificaitons(((prev) => ([res, ...prev]))));
        }
    }, [newNotificationObserver$]);

    useEffect(() => {
        const subscription = ApiService.getNotifications$(CredentialsService.getUserId()).subscribe((res) => {
            setNotificaitons(res as any);
        });
        return () => {
            subscription.unsubscribe();
        };
    }, []);

    const baseClass = 'pm-notifications';
    return (
        <div className={baseClass}>
            {notifications.map((req: any) => {
                if (req.type === 'RequestForRide') {
                    return <RideRequest {...req} />;
                }

                return <RideResponse {...req} />;
            })}
        </div>
    );
};

export default NotificationPage;
