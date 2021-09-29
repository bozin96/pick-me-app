/* eslint-disable react/jsx-props-no-spreading */
import React, { useEffect, useState } from 'react';
import { NotificationsSubject } from '../../common/observers';
import RideRequest from '../../components/RideRequest';
import RideResponse from '../../components/RideResponse';
import './Notifications.styles.scss';

const Notifications: React.FC = () => {
    const baseClass = 'pm-notifications';
    const [notifications, setNotificaitons] = useState<Notification[]>([]);

    useEffect(() => {
        NotificationsSubject.subscribe({
 next(res) {
            console.log(res);
            setNotificaitons(res as any);
},
});
    }, []);

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

export default Notifications;
