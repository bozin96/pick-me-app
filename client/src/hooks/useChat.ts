import {
    from, fromEvent, Observable,
} from 'rxjs';
import ChatService from '../services/Chat.service';
import { ChatMessageSend } from '../types';

export default (): any => {
    const conn = ChatService.connection as any;

    const newChatMessage$ = fromEvent(conn, 'ReceiveMessage');
    const newChatRequest$ = fromEvent(conn, 'NewChatRequest');
    const newUnreadedMessage$ = fromEvent(conn, 'ReceiveOtherChatMessage');

    const sendMessage$ = (message: ChatMessageSend): Observable<any> => from(conn.send('SendMessage', message));
    const openChat$ = (chatId: string, HasUnreadedMessages: boolean, prevChatId: string): Observable<any> => from(conn.send('OpenChat', { chatId, prevChatId, HasUnreadedMessages }));
    const closeChat$ = (chatId: string): Observable<any> => from(conn.send('CloseChat', { chatId }));

    return {
        sendMessage$,
        newChatMessage$,
        newUnreadedMessage$,
        openChat$,
        closeChat$,
        newChatRequest$,
    };
};
