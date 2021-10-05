/* eslint-disable class-methods-use-this */
import { HttpTransportType, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import CredentialsService from './Credentials.service';

const url = process.env.REACT_APP_BE_URL;

class NotificationService {
    static connection: any = null

    get connection(): HubConnectionBuilder { return NotificationService.connection; }

    set connection(val: HubConnectionBuilder) { NotificationService.connection = val; }

    constructor() {
        NotificationService.connection = new HubConnectionBuilder().withUrl(`${url}/notifications`, {
            skipNegotiation: true,
            transport: HttpTransportType.WebSockets,
            accessTokenFactory: () => CredentialsService.getToken(),
        })
            .configureLogging(LogLevel.Information)
            .withAutomaticReconnect()
            .build();

        NotificationService.connection
            .start()
            .then(() => {
                console.log('Connection started');
            })
            .catch((err: any) => console.log(`Unable to start Connection: ${err}`));
    }
}

export default new NotificationService();
