import React from 'react';
import { toast } from 'react-toastify';
import {
    Card, Header, Icon, List, Rating,
} from 'semantic-ui-react';
import ApiService from '../../services/Api.service';
import { MyRideInterface } from '../../types';
import './MyRide.styles.scss';

const MyRide: React.FC<MyRideInterface> = (props: MyRideInterface) => {
    const {
        startWaypoint, endWaypoint, driverName, rideId, id, review,
    } = props;
    const handleRateDriver = (event: any, data: any): any => {
        const { rating } = data;

        ApiService.rateRide(rideId, rating, id).subscribe({
            next(x) {
                toast.success('Successfully Rated Ride');
            },
            error(err) {
                toast.error('Rating Failed');
            },
        });
    };
    return (
        <Card>
            <Card.Content>
                <Header as="h4">
                    <Header.Content as="h2">
                        Driver:
                        {driverName}
                        <br />
                        <Rating onRate={handleRateDriver} maxRating={5} size="huge" rating={review} />
                    </Header.Content>
                    <List>
                        <List.Item>
                            <Icon name="marker" />
                            <List.Content>
                                {startWaypoint}
                            </List.Content>
                        </List.Item>
                        <List.Item>
                            <Icon name="marker" />
                            <List.Content>
                                {endWaypoint}
                            </List.Content>
                        </List.Item>
                    </List>
                </Header>
            </Card.Content>
        </Card>
    );
};
export default MyRide;
