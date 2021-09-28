/* eslint-disable max-len */
/* eslint-disable @typescript-eslint/no-empty-function */
/* eslint-disable jsx-a11y/anchor-is-valid */
/* eslint-disable no-lone-blocks */
import React, {
    useCallback, useMemo,
} from 'react';
import {
    Button,
    Card, CardContent, Icon, Label, List, Rating,
} from 'semantic-ui-react';
import { MyDriveInterface } from '../../types';
import MapModal from '../MapModal';
import './Ride.styles.scss';

interface RideProps {
    onApply: (rideId: string) => void,
    onChatClick: (userId: string) => void,
    ride: MyDriveInterface
}

const Ride: React.FC<RideProps> = (props: RideProps) => {
    const {
        ride: {
            waypoints = [], routeLegs = [],
            startDate, id: rideId = '', numberOfPassengers,
            driverName,
            driverRate,
            driverId,
            routeIndex,
        },
        onChatClick,
        onApply,
    } = props;

    const startLocation = useMemo(() => waypoints[0].address, [waypoints]);
    const endLocation = useMemo(() => waypoints[waypoints.length - 1].address, [waypoints]);

    const handleRideApply = useCallback(() => {
        onApply(rideId);
    }, [onApply, rideId]);

    return (
        <Card>
            <CardContent>
                <Card.Header>
                    <a href="#">
                        <h3>{driverName}</h3>
                    </a>
                    <span>
                        {startDate}
                    </span>
                    <Rating icon="star" size="large" defaultRating={driverRate} maxRating={5} disabled />
                </Card.Header>
                <Card.Meta>
                    <List>
                        <List.Item>
                            <Icon name="marker" />

                            <List.Content>
                                {startLocation}
                            </List.Content>
                        </List.Item>
                        <List.Item>
                            <List>
                                {waypoints.slice(1, waypoints.length - 1).map((waypoint: any) => (
                                    <List.Item>
                                        <Icon name="marker" />

                                        <List.Content>
                                            {waypoint.address}
                                        </List.Content>
                                    </List.Item>
                                ))}
                            </List>
                        </List.Item>
                        <List.Item>
                            <Icon name="marker" />
                            <List.Content>
                                {endLocation}

                            </List.Content>
                        </List.Item>
                    </List>
                    <MapModal waypoints={waypoints} routeLegs={routeLegs} routeIndex={routeIndex} />

                </Card.Meta>
                <Card.Description>
                    <Label as="a" size="large" color="teal">
                        <Icon name="dollar sign" />
                        500.00
                    </Label>
                    <Label as="a" size="large" color="yellow">
                        <Icon name="time" />
                        60 min
                    </Label>
                    <Label as="a" size="large" color="teal">
                        Free Seats:
                        {numberOfPassengers}
                    </Label>
                </Card.Description>
                <Card.Description>
                    <Button color="orange" inverted onClick={() => onChatClick(driverId)} icon="chat">Chat</Button>
                    <Button positive onClick={handleRideApply}>Apply</Button>
                </Card.Description>
            </CardContent>
        </Card>
    );
};
export default Ride;
