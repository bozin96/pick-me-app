/* eslint-disable import/no-extraneous-dependencies */
import React from 'react';
import './ChatMessage.styles.scss';

interface ChatMessageProps {
    right: boolean,
    text: string
}
const ChatMessage: React.FC<ChatMessageProps> = (props: ChatMessageProps) => {
    const { right = true, text } = props;
    return (
        <li
            className={`pm-message pm-message--${right ? 'right' : 'left'}`}
        >
            <span>
                {text}
            </span>
        </li>
    );
};

export default ChatMessage;
