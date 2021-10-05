/* eslint-disable max-len */
import React, {
    ChangeEvent, useEffect, useRef, useState,
} from 'react';
import { map } from 'rxjs';
import {
    Button, Dimmer, Loader, Segment, TextArea,
} from 'semantic-ui-react';
import ChatMessage from '../../../../components/ChatMessage';
import useChat from '../../../../hooks/useChat';
import ApiService from '../../../../services/Api.service';
import CredentialsService from '../../../../services/Credentials.service';
import { ChatMessageReceive, ChatMessageSend, IChatMessage } from '../../../../types';
import './Chat.styles.scss';

type ChatProps = {
    chatId: string,
    receiverId: string,

}

const Chat: React.FC<ChatProps> = (props: ChatProps) => {
    const { chatId, receiverId } = props;
    const [fetchingMessages, setFetchingMessages] = useState(false);
    const [chatMessages, setChatMessages] = useState<IChatMessage[]>([]);
    const [currentMessage, setCurrentMessage] = useState<string>('');
    const [params, setParams] = useState({
        pageSize: 10,
        pageNumber: 1,
    });
    const chatContainer = useRef<any>();
    const { sendMessage$, newChatMessage$ } = useChat();

    const handleOnChange = (e: ChangeEvent<HTMLTextAreaElement>): void => {
        // const { tag };
        setCurrentMessage(e.target.value);
    };

    const updateScroll = (): void => {
        if (!chatContainer.current) return;
        chatContainer.current.scrollTop = chatContainer.current.scrollHeight;
    };

    const handleSendMessage = (): void => {
        if (!chatId) {
            alert('no chat selected');
            return;
        }
        sendMessage$({
            chatId,
            receiverId,
            text: currentMessage,
        } as ChatMessageSend).pipe().subscribe(() => {
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
        });
    };

    useEffect(() => {
        const fetchMessages = (): void => {
            setFetchingMessages(true);
            ApiService.getChatMessages$(chatId, params).subscribe((res: IChatMessage[]): void => {
                setChatMessages((prev: any) => ([...res, ...prev]));
                setFetchingMessages(false);
            });
        };
        if (chatId) {
            fetchMessages();
        }
    }, [chatId, params]);

    useEffect(() => {
        if (chatId) {
            setParams({
                pageSize: 10,
                pageNumber: 1,
            });
            setChatMessages([]);
        }
    }, [chatId]);

    useEffect(() => {
        if (params.pageSize === 10 && params.pageNumber === 1 && chatMessages.length) {
            updateScroll();
        }
    }, [params, chatMessages]);

    useEffect(() => {
        const subscruption = newChatMessage$.pipe(map((res) => res as IChatMessage)).subscribe(
            (res: IChatMessage) => {
                setChatMessages((prev: any): any => [...prev, res].sort((a: any, b: any) => {
                    const dateA = new Date(a.timestamp);
                    const dateB = new Date(b.timestamp);
                    return dateA > dateB ? 1 : -1;
                }));
                setCurrentMessage('');
                updateScroll();
            },
        );
        return () => {
            subscruption.unsubscribe();
        };
    }, [chatId, newChatMessage$]);

    return (
        <div className="pm-single-chat">
            <Segment>
                <div id="chatHistory" className="pm-single-chat__history" ref={chatContainer}>
                    <Dimmer active={fetchingMessages} inverted>
                        <Loader inverted>Loading</Loader>
                    </Dimmer>
                    {chatId && (
                        <Button onClick={() => setParams((prev: any) => ({ ...prev, pageNumber: prev.pageNumber + 1 }))}>Load More</Button>
                    )}
                    {chatMessages.map((msg: ChatMessageReceive) => (
                        <ChatMessage right={CredentialsService.getUserId() === msg.sendUserId} text={msg.text} timestamp={msg.timestamp} />
                    ))}
                </div>
            </Segment>
            <div className="pm-single-chat__input-container">
                <TextArea placeholder="Enter Message" value={currentMessage} name="message" onChange={handleOnChange} />
                <Button type="click" onClick={handleSendMessage}>Send</Button>
            </div>
        </div>
    );
};

export default Chat;
