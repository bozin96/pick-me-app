/* eslint-disable @typescript-eslint/explicit-function-return-type */
/* eslint-disable @typescript-eslint/no-explicit-any */
// const URL = `${process.env.REACT_APP_BE_NOT_URL}`;
import {
    HttpTransportType, HubConnectionBuilder, LogLevel,
} from '@microsoft/signalr';
import { useEffect, useState } from 'react';
import { defer, Observable } from 'rxjs';
import { newChatMessageSubject } from '../common/observers';
import CredentialsService from '../services/Credentials.service';
import { ChatMessageSend } from '../types';

export default (): any => {
    const [connection, setConnection] = useState<any>();

    useEffect(() => {
        const establishConnection = async (): Promise<any> => {
            const newConnection = new HubConnectionBuilder()
                .withUrl('http://localhost:51052/chats', {
                    skipNegotiation: true,
                    transport: HttpTransportType.WebSockets,
                    accessTokenFactory: () => CredentialsService.getToken(),
                })
                .configureLogging(LogLevel.Information)
                .withAutomaticReconnect()
                .build();

            setConnection(newConnection);
        };

        establishConnection();
        // add dc
    }, []);

    useEffect(() => {
        if (connection) {
            connection.start()
                .then((result: any): void => {
                    connection.on('ReceiveMessage', (message: any): void => {
                        newChatMessageSubject.next(message);
                    });
                })
                .catch((e: any) => console.log('Connection failed: ', e));
        }
    }, [connection]);

    const sendMessage = (message: ChatMessageSend): Observable<any> | null => {
        if (connection.connectionStarted) {
            return defer(() => connection.send('SendMessage', message));
        }
        alert('No connection to server yet.');
        return null;
    };
    return {
        sendMessage,
    };
};
