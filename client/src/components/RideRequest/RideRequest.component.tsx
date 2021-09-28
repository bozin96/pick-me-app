/* eslint-disable max-len */
/* eslint-disable react/no-unused-prop-types */
import React, { useCallback } from 'react';
import { toast } from 'react-toastify';
import { Button, Card } from 'semantic-ui-react';
import ApiService from '../../services/Api.service';

interface RequestType {
    body: string, // "Body content"
    header: string, // "Header"
    rideId: string, // "7ff2c780-a493-4908-8460-d665966fc3ff"
    type: number, // 0
    userFromName: string,
    userFromId: string, // "6384b6be-7fb7-46a1-80df-8c8ba4c63835"
    userFromImage: any,
    userToId: string // "d3015522-d131-49dd-9220-4a50c0da3cd1
    id: string
}
const RideRequest: React.FC<RequestType> = (props: RequestType) => {
    console.log(props);
    const {
        rideId,
        body = '', header = '', id, userFromName = '',
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
