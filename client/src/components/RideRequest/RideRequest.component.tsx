/* eslint-disable max-len */
/* eslint-disable react/no-unused-prop-types */
import React, { useCallback } from 'react';
import { toast } from 'react-toastify';
import { Button, Card } from 'semantic-ui-react';
import ApiService from '../../services/Api.service';

interface RequestType {
    Body: string, // "Body content"
    Header: string, // "Header"
    RideId: string, // "7ff2c780-a493-4908-8460-d665966fc3ff"
    Type: number, // 0
    UserFromName: string,
    UserFromId: string, // "6384b6be-7fb7-46a1-80df-8c8ba4c63835"
    UserFromImage: any,
    UserToId: string // "d3015522-d131-49dd-9220-4a50c0da3cd1
    Id: string
}
const RideRequest: React.FC<RequestType> = (props: RequestType) => {
    const {
        RideId: rideId,
        Body: body = '', Header: header = '', Id: id, UserFromName: userFromName = '',
    } = props;

    const declineRideRequest = useCallback(() => {
        ApiService.reviewRideRequest(rideId, { notificationId: id, accepted: false }).subscribe(() => {
            toast('Succesfully Declined Ride');
        });
    }, [id, rideId]);

    const approveRideRequest = useCallback(() => {
        ApiService.reviewRideRequest(rideId, { notificationId: id, accepted: true }).subscribe(() => {
            toast('Succesfully Approved Ride');
        });
    }, [id, rideId]);

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
            <Card.Content extra>
                <div className="ui two buttons">
                    <Button color="green" onClick={approveRideRequest}>
                        Approve
                    </Button>
                    <Button color="red" onClick={declineRideRequest}>
                        Decline
                    </Button>
                </div>
            </Card.Content>
        </Card>

    );
};

export default RideRequest;
