// const URL = `${process.env.REACT_APP_BE_NOT_URL}`;
import { HttpTransportType, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { useEffect } from 'react';
import { toast } from 'react-toastify';
import CredentialsService from '../services/Credentials.service';

export default (shoudStart: boolean | null): void => {
    useEffect(() => {
        const establishConnection = (): void => {
            const connection = new HubConnectionBuilder()
                .withUrl('http://localhost:51052/notifications', {
                    skipNegotiation: true,
                    transport: HttpTransportType.WebSockets,
                    accessTokenFactory: () => CredentialsService.getToken(),
                })
                .configureLogging(LogLevel.Information)
                .withAutomaticReconnect()
                .build();

            connection.on('RequestForRide', () => {
                toast('Wow so easy RequestForRide!');
            });
            connection.on('ResponseOnRideRequest', () => {
                toast('Wow so easy ResponseOnRideRequest!');
            });
            connection.on('RideReview', () => {
                toast('Wow so easy RideReview!');
            });
            connection
                .start()
                .then(() => {
                    console.log('Connection Started');
                })
                .catch((err: any) => console.log(`Unable to start Connection: ${err}`));
        };
        if (shoudStart) {
            establishConnection();
        }
    }, [shoudStart]);
};
