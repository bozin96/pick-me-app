/* eslint-disable max-len */
/* eslint-disable react/no-unused-prop-types */
import React from 'react';
import { Card } from 'semantic-ui-react';
import { RequestType } from '../../types';

const RideResponse: React.FC<RequestType> = (props: RequestType) => {
    const {
        Body: body = '', Header: header = '', Id: id, UserFromName: userFromName = '',
    } = props;

    return (
        <Card>
            <Card.Content>
                <Card.Header>
                    {userFromName}
                    :
                    &nbsp;
                    {header}
                </Card.Header>
                <Card.Description>
                    {header}
                    {body}
                </Card.Description>
            </Card.Content>
        </Card>

    );
};

export default RideResponse;
