/* eslint-disable @typescript-eslint/no-empty-function */
/* eslint-disable jsx-a11y/click-events-have-key-events */
/* eslint-disable jsx-a11y/alt-text */
/* eslint-disable jsx-a11y/no-static-element-interactions */
/* eslint-disable max-len */
/* eslint-disable @typescript-eslint/no-shadow */
import React, { ChangeEvent, useEffect, useState } from 'react';
import { Icon } from 'semantic-ui-react';
import { newUnreadedMessage } from '../../common/observers';
import useChat from '../../hooks/useChat';
import useDebounce from '../../hooks/useDebounce';
import ApiService from '../../services/Api.service';
import CredentialsService from '../../services/Credentials.service';
import {
    ChatInteface,
} from '../../types';
import './ChatPage.styles.scss';
import Chat from './components/Chat/Chat.component';

const ChatPage: React.FC = () => {
    const [chatState, setChatState] = useState<ChatInteface[]>([]);
    const [selectedChat, setSelectedChat] = useState<string>('');
    const [receiverId, setReceiverId] = useState<string>('');
    const { chatInitialize } = useChat();

    const [searchValue, setSearchValue] = useState<string>('');
    const [value, setValue] = useDebounce<string>(1000, searchValue);

    useEffect(() => {
    }, [value]);

    useEffect(() => {
        ApiService.getChats(value || undefined).subscribe((res: ChatInteface[]): void => setChatState(res));
    }, [value]);

    const handleChatSelect = (chatId: string, receiverId: string): void => {
        const selectedChatState = chatState.find((state: ChatInteface) => state.chatId === chatId) as ChatInteface;
        chatInitialize(chatId, selectedChatState.numberOfUnreadedMessages > 0).subscribe((): void => {
            setSelectedChat(chatId);
            setReceiverId(receiverId);
            setChatState((prev: ChatInteface[]) => prev.map((chatState: ChatInteface) => (chatState.chatId === chatId ? ({ ...chatState, numberOfUnreadedMessages: 0 }) : chatState)));
        });
    };

    useEffect(() => {
        newUnreadedMessage.subscribe((chatId: string) => setChatState((prev: ChatInteface[]) => prev.map((chatState: ChatInteface) => (chatState.chatId === chatId ? ({ ...chatState, numberOfUnreadedMessages: chatState.numberOfUnreadedMessages + 1 }) : chatState))));
    }, []);

    const handleSearchInputChange = (e:ChangeEvent<HTMLInputElement>):void => {
        const { target: { value } } = e;
        setSearchValue(value);
        setValue(value);
    };

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
                                    {(numberOfUnreadedMessages > 0 && lastMessageSenderId !== CredentialsService.getUserId()) && <Icon name="mail" />}
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
