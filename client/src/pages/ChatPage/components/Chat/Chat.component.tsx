/* eslint-disable max-len */
import React, { useEffect, useRef, useState } from 'react';
import { Button, Input } from 'semantic-ui-react';
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

    const chatContainer = useRef<any>();

    const { sendMessage } = useChat();
    const handleOnChange = (e: any): void => {
        setCurrentMessage(e.target.value);
    };
    const updateScroll = (): void => {
        if (!chatContainer.current) return;
        chatContainer.current.scrollTop = chatContainer.current.scrollHeight;
    };

    useEffect(() => {
        if (chatId) {
            ApiService.getChatMessages(chatId).subscribe((res: ChatMessageInteface[]): void => {
                setChatMessages(res);
                setTimeout(() => {
                    updateScroll();
                }, 1000);
            });
        }
    }, [chatId]);

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
                    }]);
                    setCurrentMessage('');
                    updateScroll();
                },
            },
        );
    };

    return (
        <div className="pm-single-chat">
            <div ref={chatContainer} className="pm-single-chat__container">
                {chatMessages.map((msg: ChatMessageReceive) => (
                    <ChatMessage right={CredentialsService.getUserId() === msg.sendUserId} text={msg.text} />
                ))}
            </div>
            <div className="pm-single-chat__input-container">
                <Input placeholder="Enter Message" value={currentMessage} name="message" onChange={handleOnChange} />
                <Button type="click" onClick={handleSendMessage}>Send</Button>
            </div>
        </div>
    );
};

export default Chat;
