/* eslint-disable @typescript-eslint/no-empty-function */
/* eslint-disable jsx-a11y/click-events-have-key-events */
/* eslint-disable jsx-a11y/alt-text */
/* eslint-disable jsx-a11y/no-static-element-interactions */
/* eslint-disable max-len */
/* eslint-disable @typescript-eslint/no-shadow */
import React, { ChangeEvent, useEffect, useState } from 'react';
import { toast } from 'react-toastify';
import { Icon } from 'semantic-ui-react';
import useChat from '../../hooks/useChat';
import useDebounce from '../../hooks/useDebounce';
import { usePrevious } from '../../hooks/usePrevious';
import ApiService from '../../services/Api.service';
import CredentialsService from '../../services/Credentials.service';
import {
    IChat,
    OtherChatMessageReceive,
} from '../../types';
import './ChatPage.styles.scss';
import Chat from './components/Chat/Chat.component';

const ChatPage: React.FC = () => {
    const [chatState, setChatState] = useState<IChat[]>([]);
    const [selectedChat, setSelectedChat] = useState<string>('');
    const [receiverId, setReceiverId] = useState<string>('');
    const [searchValue, setSearchValue] = useState<string>('');

    const {
        openChat$, newUnreadedMessage$, newChatRequest$, closeChat$,
    } = useChat();
    const [value, setValue] = useDebounce<string>(1000, searchValue);
    const previousChatId = usePrevious<string>(selectedChat);

    const handleChatSelect = (chatId: string, receiverId: string): void => {
        const selectedChatState = chatState.find((state: IChat) => state.chatId === chatId) as IChat;

        openChat$(chatId, selectedChatState.numberOfUnreadedMessages > 0, previousChatId).subscribe((): void => {
            setSelectedChat(chatId);
            setReceiverId(receiverId);
            setChatState((prev: IChat[]) => prev.map((chatState: IChat) => (chatState.chatId === chatId ? ({ ...chatState, numberOfUnreadedMessages: 0 }) : chatState)));
        });
    };

    const handleSearchInputChange = (e: ChangeEvent<HTMLInputElement>): void => {
        const { target: { value } } = e;
        setSearchValue(value);
        setValue(value);
    };

    useEffect(() => () => {
        closeChat$();
    }, [closeChat$]);

    useEffect(() => {
        const subscription = newChatRequest$.subscribe((newChat: IChat) => {
            toast('New Chat Added');
            setChatState((prev) => ([newChat, ...prev]));
        });
        return () => {
            subscription.unsubscribe();
        };
    }, [newChatRequest$]);

    useEffect(() => {
        ApiService.getChats(value || undefined).subscribe((res: IChat[]): void => setChatState(res));
    }, [value]);

    useEffect(() => {
        const subscription = newUnreadedMessage$.subscribe((chatInfo:OtherChatMessageReceive) => {
            toast.success('New Message Received');
            const { chatId: chatIdToUpdate } = chatInfo;
            setChatState((prev: IChat[]) => prev.map((chatState: IChat) => (chatState.chatId === chatIdToUpdate
                ? ({ ...chatState, numberOfUnreadedMessages: chatState.numberOfUnreadedMessages + 1 })
                : chatState)));
        });
        return () => {
            subscription.unsubscribe();
        };
    }, [newUnreadedMessage$]);

    return (
        <div className="pm-chats">
            <div>
                <div className="pm-chats--sidebar">
                    <div className="pm-chats__search">
                        <input type="text" placeholder="search" value={searchValue} onChange={handleSearchInputChange} />
                        <Icon name="search" />
                    </div>
                    {chatState.map(({
                        chatId, firstUserId, firstUserName, firstUserPhoto, secondUserId, secondUserName,
                        secondUserPhoto,
                        numberOfUnreadedMessages, lastMessageSenderId,
                    }: any) => {
                        const isTargetUserFirst = firstUserId !== CredentialsService.getUserId();
                        return (
                            <div className={chatId === selectedChat ? 'active' : ''} onClick={() => handleChatSelect(chatId, isTargetUserFirst ? firstUserId : secondUserId)}>
                                <img src={`data:image/png;base64,${isTargetUserFirst ? firstUserPhoto : secondUserPhoto}`} />
                                <div>
                                    <span>
                                        {isTargetUserFirst ? firstUserName : secondUserName}
                                    </span>
                                    {numberOfUnreadedMessages > 0 && lastMessageSenderId !== CredentialsService.getUserId() && <Icon name="mail" />}
                                </div>
                            </div>
                        );
                    })}
                </div>
                <Chat chatId={selectedChat} receiverId={receiverId} />
            </div>
        </div>

    );
};

export default ChatPage;
