/* eslint-disable max-len */
import React, { useState } from 'react';
import { toast } from 'react-toastify';
import {
    Card, Header, Icon, List, Rating, RatingProps,
} from 'semantic-ui-react';
import ApiService from '../../services/Api.service';
import { MyRideInterface } from '../../types';
import './MyRide.styles.scss';

const MyRide: React.FC<MyRideInterface> = (props: MyRideInterface) => {
    const {
        startWaypoint, endWaypoint, driverName, rideId, id, review: initialReview,
    } = props;

    const [review, setReview] = useState<number>(initialReview);

    const handleRateDriver = (event: any, data: RatingProps): void => {
        const { rating } = data;
        ApiService.rateRide(rideId, rating as number, id).subscribe({
            next(x) {
                setReview(rating as number);
                toast.success('Successfully Rated Ride');
            },
            error() {
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
