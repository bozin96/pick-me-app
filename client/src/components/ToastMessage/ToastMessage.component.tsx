import React from 'react';

interface ToastMessage {
    header: string,
    body: string
}

const ToastMessage: React.FC<any> = (props: any) => {
    const { data: { header = '', body = '' } } = props;
    return (

        <div>
        <h3>
            {header}
        </h3>
        <p>
            {body}
        </p>
        </div>
);
};

export default ToastMessage;
