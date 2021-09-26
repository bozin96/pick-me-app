/* eslint-disable jsx-a11y/click-events-have-key-events */
/* eslint-disable jsx-a11y/alt-text */
/* eslint-disable jsx-a11y/no-static-element-interactions */
/* eslint-disable max-len */
/* eslint-disable @typescript-eslint/no-shadow */
import React, { useEffect, useState } from 'react';
import { Button, Input } from 'semantic-ui-react';
import { newChatMessageSubject } from '../../common/observers';
import ChatMessage from '../../components/ChatMessage';
import useChat from '../../hooks/useChat';
import ApiService from '../../services/Api.service';
import CredentialsService from '../../services/Credentials.service';
import { ChatMessageSend } from '../../types';
import './ChatPage.styles.scss';

const ChatPage: React.FC = () => {
    const [chatState, setChatState] = useState<any>([]);
    const [chatMessages, setChatMessages] = useState<any>([]);
    const [selectedChat, setSelectedChat] = useState<string>('');
    const [receiverId, setReceiverId] = useState<string>('');
    const [currentMessage, setCurrentMessage] = useState<string>('');
    const chatService = useChat();

    useEffect(() => {
        newChatMessageSubject.subscribe((res:any):any => setChatMessages((prev:any):any => ([...prev, res])));
    }, []);

    useEffect(() => {
        ApiService.getChats().subscribe((res: any): void => setChatState(res));
    }, []);

    const handleChatSelect = (chatId: string, receiverId: string): void => {
        setSelectedChat(chatId);
        setReceiverId(receiverId);
        ApiService.getChatMessages(chatId).subscribe((res: any): void => setChatMessages(res));
    };

    const handleOnChange = (e: any): void => {
        setCurrentMessage(e.target.value);
    };

    const handleSendMessage = (): void => {
        if (!selectedChat) {
            alert('no chat selected');
        }
        chatService.sendMessage({
            chatId: selectedChat,
            receiverId,
            text: currentMessage,
        } as ChatMessageSend).pipe().subscribe(
            {
                next() {
                    setChatMessages((prev: any): any => [...prev, {
                        chatId: selectedChat,
                        sendUserId: CredentialsService.getUserId(),
                        text: currentMessage,
                    }]);
                    setCurrentMessage('');
                },
            },
        );
    };

    return (
        <div className="pm-chats">
            <div>
                <div className="pm-chats--sidebar">
                    {chatState.map(({
                        chatId, firstUserId, firstUserName, firstUserPhoto, secondUserId, secondUserName,
                        secondUserPhoto,
                    }: any) => {
                        const isTargetUserFirst = firstUserId !== CredentialsService.getUserId();
                        return (
                            <div onClick={() => handleChatSelect(chatId, isTargetUserFirst ? firstUserId : secondUserId)}>
                                <img src={isTargetUserFirst ? firstUserPhoto : secondUserPhoto} />
                                <div>
                                    <span>
                                        {isTargetUserFirst ? firstUserName : secondUserName}
                                    </span>
                                </div>
                            </div>
                        );
                    })}
                </div>
                <div className="pm-chats__container">
                    {chatMessages.map((msg: any) => (
                            <ChatMessage right={CredentialsService.getUserId() === msg.sendUserId} text={msg.text} />
                        ))}
                    <div className="pm-chats__container__input-container">

                        <Input placeholder="Enter Message" value={currentMessage} name="message" onChange={handleOnChange} />
                        <Button type="click" onClick={handleSendMessage}>Send</Button>
                    </div>
                </div>
            </div>
        </div>

    );
};

export default ChatPage;
