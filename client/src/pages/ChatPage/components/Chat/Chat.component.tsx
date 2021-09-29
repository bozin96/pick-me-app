/* eslint-disable max-len */
import React, {
    ChangeEvent, useEffect, useRef, useState,
} from 'react';
import { Button, TextArea } from 'semantic-ui-react';
import { newChatMessageSubject } from '../../../../common/observers';
import ChatMessage from '../../../../components/ChatMessage';
import useChat from '../../../../hooks/useChat';
import ApiService from '../../../../services/Api.service';
import CredentialsService from '../../../../services/Credentials.service';
import { ChatMessageInteface, ChatMessageReceive, ChatMessageSend } from '../../../../types';
import './Chat.styles.scss';

type ChatProps = {
    chatId: string,
    receiverId: string
}

const Chat: React.FC<ChatProps> = (props: ChatProps) => {
    const { chatId, receiverId } = props;
    const [chatMessages, setChatMessages] = useState<ChatMessageInteface[]>([]);
    const [currentMessage, setCurrentMessage] = useState<string>('');
    const [params, setParams] = useState({
        pageSize: 10,
        pageNumber: 1,
    });
    const chatContainer = useRef<any>();

    const { sendMessage } = useChat();

    const handleOnChange = (e: ChangeEvent<HTMLTextAreaElement>): void => {
        setCurrentMessage(e.target.value);
    };
    const updateScroll = (): void => {
        if (!chatContainer.current) return;
        chatContainer.current.scrollTop = chatContainer.current.scrollHeight;
    };

    useEffect(() => {
        const fetchMessages = (): void => {
            ApiService.getChatMessages(chatId, params).subscribe((res: ChatMessageInteface[]): void => {
                setChatMessages((prev: any) => ([...res, ...prev]
                ));
            });
        };
        if (chatId) {
            fetchMessages();
        }
    }, [chatId, params]);

    const handleSendMessage = (): void => {
        if (!chatId) {
            alert('no chat selected');
            return;
        }
        sendMessage({
            chatId,
            receiverId,
            text: currentMessage,
        } as ChatMessageSend).pipe().subscribe(
            {
                next() {
                    setChatMessages((prev: any): any => [...prev, {
                        chatId,
                        sendUserId: CredentialsService.getUserId(),
                        text: currentMessage,
                        timestamp: new Date(),
                    }].sort((a: any, b: any) => {
                        const dateA = new Date(a.timestamp);
                        const dateB = new Date(b.timestamp);
                        return dateA > dateB ? 1 : -1;
                    }));
                    setCurrentMessage('');
                    updateScroll();
                },
            },
        );
    };

    useEffect(() => {
        newChatMessageSubject.pipe().subscribe(
            {
                next(x) {
                    setChatMessages((prev: any): any => [...prev, {
                        chatId,
                        ...x,
                    }].sort((a: any, b: any) => {
                        const dateA = new Date(a.timestamp);
                        const dateB = new Date(b.timestamp);
                        return dateA > dateB ? 1 : -1;
                    }));
                    setCurrentMessage('');
                    updateScroll();
                },
            },
        );
    }, []);

    return (
        <div className="pm-single-chat">
            <div id="chatHistory" className="pm-single-chat__history" ref={chatContainer}>
                {chatId && (
                    <Button onClick={() => setParams((prev: any) => ({ ...prev, pageNumber: prev.pageNumber + 1 }))}>Load More</Button>
                )}
                {chatMessages.map((msg: ChatMessageReceive) => (
                    <ChatMessage right={CredentialsService.getUserId() === msg.sendUserId} text={msg.text} timestamp={msg.timestamp} />
                ))}
            </div>
            <div className="pm-single-chat__input-container">
                <TextArea placeholder="Enter Message" value={currentMessage} name="message" onChange={handleOnChange} />
                <Button type="click" onClick={handleSendMessage}>Send</Button>
            </div>
        </div>
    );
};

export default Chat;
