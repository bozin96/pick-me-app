/* eslint-disable max-len */
/* eslint-disable react/no-unescaped-entities */
import React, { useEffect, useLayoutEffect, useState } from 'react';
import {
    Button, Icon, Modal,
} from 'semantic-ui-react';
import MapService from '../../services/Map.service';
import { RouteLegInterface, Waypoint } from '../../types';

type MapModalProps = {
    waypoints: Waypoint[],
    routeLegs: RouteLegInterface[],
    routeIndex:number,
}
const MapModal: React.FC<Partial<MapModalProps>> = (props: Partial<MapModalProps>) => {
    const { waypoints = [], routeIndex } = props;
    const [map, setMap] = useState<any>();
    const [open, setOpen] = React.useState(false);
    useLayoutEffect(() => {
        const initMap = (): void => {
            MapService.loadBingApi('AsrEpVFLl4bpmG5EUAggr91gdwpbwDfzb74vFXC_bFmAeTWSQnINDulRiswel16H').then((res: any) => {
                const a = new Microsoft.Maps.Map('#map', {
                });
                setMap(a);
            });
        };
        if (open) initMap();
    }, [open]);
    useEffect(() => {
        const refreshMap = (): void => {
            Microsoft.Maps.loadModule('Microsoft.Maps.Directions', (): void => {
                const directionsManager = new Microsoft.Maps.Directions.DirectionsManager(map);

                directionsManager.clearDisplay();
                directionsManager.clearAll();

                waypoints
                    .map((dest: any): any => {
                        const { address, longitude, latitude } = dest;
                        return new Microsoft.Maps.Directions.Waypoint({
                            address,
                            location: new Microsoft.Maps.Location(latitude, longitude),
                        });
                    }).forEach(
                        (waypoint: Microsoft.Maps.Directions.Waypoint) => {
                            directionsManager.addWaypoint(waypoint);
                        },
                    );
                directionsManager.calculateDirections();

                directionsManager.setRequestOptions({
                    routeMode: Microsoft.Maps.Directions.RouteMode.driving,
                    routeDraggable: false,
                    routeIndex,
                    maxRoutes: 1,
                });
            });
        };
        if (waypoints && map) {
            refreshMap();
        }
    }, [waypoints, map, routeIndex]);
    return (
        <Modal
            onClose={() => setOpen(false)}
            onOpen={() => setOpen(true)}
            open={open}
            trigger={(
                <Button>
                    <Icon name="map" />
                    Show on Map
                </Button>
            )}
        >
            <Modal.Content image>
                <div
                    id="map"
                    style={{
                        minWidth: '500px',
                        width: '100%',
                        height: '500px',
                    }}
                />
            </Modal.Content>
            <Modal.Actions>
                <Button color="black" onClick={() => setOpen(false)}>
                    Close
                </Button>
            </Modal.Actions>
        </Modal>
    );
};

export default MapModal;
