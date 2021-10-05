/* eslint-disable no-console */
/* eslint-disable class-methods-use-this */
import { HttpTransportType, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import CredentialsService from './Credentials.service';

const url = process.env.REACT_APP_BE_URL;

class ChatService {
    static connection: any = null

    get connection(): HubConnectionBuilder { return ChatService.connection; }

    set connection(val: HubConnectionBuilder) { ChatService.connection = val; }

    constructor() {
        ChatService.connection = new HubConnectionBuilder().withUrl(`${url}/chats`, {
            skipNegotiation: true,
            transport: HttpTransportType.WebSockets,
            accessTokenFactory: () => CredentialsService.getToken(),
        })
            .configureLogging(LogLevel.Information)
            .withAutomaticReconnect()
            .build();

        // await ChatService.connection.StartAsync();

        ChatService.connection
            .start()
            .then(() => {
                console.log('Connection started');
            })
            .catch((err: any) => console.log(`Unable to start Connection: ${err}`));
    }
}

export default new ChatService();
