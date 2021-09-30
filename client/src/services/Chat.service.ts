/* eslint-disable class-methods-use-this */
import { HttpTransportType, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import CredentialsService from './Credentials.service';

const url = process.env.REACT_APP_BE_URL;

class SingnalRService {
    static connection: any = null

    get connection(): HubConnectionBuilder { return SingnalRService.connection; }

    set connection(val: HubConnectionBuilder) { SingnalRService.connection = val; }

    constructor() {
        SingnalRService.connection = new HubConnectionBuilder().withUrl(`${url}/chats`, {
            skipNegotiation: true,
            transport: HttpTransportType.WebSockets,
            accessTokenFactory: () => CredentialsService.getToken(),
        })
            .configureLogging(LogLevel.Information)
            .withAutomaticReconnect()
            .build();

        SingnalRService.connection
            .start()
            .then(() => {
                console.log('Connection started');
            })
            .catch((err: any) => console.log(`Unable to start Connection: ${err}`));
    }
}

export default new SingnalRService();
