/* eslint-disable import/no-extraneous-dependencies */
import React from 'react';
import DateService from '../../services/Date.service';
import './ChatMessage.styles.scss';

interface ChatMessageProps {
    right: boolean,
    text: string,
    timestamp: string
}
const ChatMessage: React.FC<ChatMessageProps> = (props: ChatMessageProps) => {
    const { right = true, text, timestamp } = props;
    return (
        <li
            className={`pm-message pm-message--${right ? 'right' : 'left'}`}
        >
            <div className="pm-message__header">
                {DateService.getFullLocalChatTime(timestamp)}
            </div>
            <div className="pm-message__content">
                {text}

            </div>
        </li>
    );
};

export default ChatMessage;
