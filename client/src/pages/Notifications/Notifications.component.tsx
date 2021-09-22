/* eslint-disable react/jsx-props-no-spreading */
import React, { useEffect, useState } from 'react';
import { Dimmer, Segment } from 'semantic-ui-react';
import RideRequest from '../../components/RideRequest';
import RideResponse from '../../components/RideResponse';
import ApiService from '../../services/Api.service';
import CredentialsService from '../../services/Credentials.service';
import './Notifications.styles.scss';

const Notifications: React.FC = () => {
    const baseClass = 'pm-notifications';
    const [notifications, setNotifications] = useState<any[]>([]);
    const [isFetching, setIsFetching] = useState<boolean>(false);
    useEffect(() => {
        // setIsFetching(true);
        ApiService.getNotifications(CredentialsService.getUserId()).subscribe({

            next(x) {
                setNotifications(x);
                setIsFetching(false);
            },
            error(err) {
                setIsFetching(false);
            },
            complete() {
                console.log('done');
            },
        });
    }, []);
    return (
        <div className={baseClass}>
            <Dimmer.Dimmable as={Segment} dimmed={isFetching}>
                {notifications.map((req: any) => {
                    if (req.Type === 'RequestForRide') {
                       return <RideRequest {...req} />;
                    }
                    if (req.Type === 'ResponseForRide') {
                        return <RideResponse {...req} />;
                    }
                    return undefined;
                })}
            </Dimmer.Dimmable>
        </div>
    );
};

export default Notifications;
