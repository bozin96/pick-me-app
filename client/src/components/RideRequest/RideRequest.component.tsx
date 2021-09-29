/* eslint-disable max-len */
/* eslint-disable react/no-unused-prop-types */
import React, { useCallback } from 'react';
import { toast } from 'react-toastify';
import { Button, Card } from 'semantic-ui-react';
import ApiService from '../../services/Api.service';
import { RideRequestNotificationInterface } from '../../types';

const RideRequest: React.FC<RideRequestNotificationInterface> = (props: RideRequestNotificationInterface) => {
    const {
        rideId,
        body = '', header = '', id, userFromName = '',
        isVisible = true,
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
        <Card className={!isVisible ? 'disabled' : ''}>

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
