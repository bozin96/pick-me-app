import React, { useMemo } from 'react';
import {
    Card, Header, Icon, Label, List,
} from 'semantic-ui-react';
import { MyRideInterface } from '../../types';
import MapModal from '../MapModal';
import './MyRide.styles.scss';

const MyRide: React.FC<MyRideInterface> = (props: MyRideInterface) => {
    const { waypoints = [], routeLegs } = props;

    const startLocation = useMemo(() => waypoints[0]?.address, [waypoints]);
    const endLocation = useMemo(() => waypoints[waypoints.length - 1]?.address, [waypoints]);
    return (
        <Card>
            <Card.Content>
                <Header as="h4">
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

                    <MapModal waypoints={waypoints} routeLegs={routeLegs} index={0} />
                </Header>
            </Card.Content>
            <Card.Content extra>

                <Label as="a" size="large" color="teal">
                    <Icon name="dollar sign" />
                    500.00
                </Label>
                <Label as="a" size="large" color="yellow">
                    <Icon name="time" />
                    60 min
                </Label>
            </Card.Content>
        </Card>
    );
};
export default MyRide;
