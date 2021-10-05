import React, { useMemo } from 'react';
import {
    Card, Header, Icon, List,
} from 'semantic-ui-react';
import { MyDriveInterface } from '../../types';
import MapModal from '../MapModal';
import './MyDrive.styles.scss';

const MyDrive: React.FC<MyDriveInterface> = (props: MyDriveInterface) => {
    const { waypoints = [], routeLegs, routeIndex } = props;

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

                    <MapModal waypoints={waypoints} routeLegs={routeLegs} routeIndex={routeIndex} />
                </Header>
            </Card.Content>
        </Card>
    );
};
export default MyDrive;
