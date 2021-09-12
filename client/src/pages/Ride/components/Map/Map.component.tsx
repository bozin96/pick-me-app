/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable max-len */
/* eslint-disable react/require-default-props */
import React, { useEffect, useRef, useState } from 'react';
import MapService, { Microsoft } from '../../../../services/Map.service';

type IMapProps = {
    mapOptions?: any;
}

const Map: React.FC<IMapProps> = (props: IMapProps) => {
    const mapRef = useRef<HTMLDivElement>(null);
    const { mapOptions = {} } = props;

    useEffect(() => {
        let directionsManager: any = null;
        console.log('ae bre');
        const directionsUpdated = (e: any): void => {
            const currRoute = directionsManager.getCurrentRoute();

            const { routeLegs } = currRoute;
            console.log('legs', routeLegs);
            const newArray = [];

            const waypoints = directionsManager.getAllWaypoints();
            console.log(waypoints);

            // for preview
            const waypointsInfoArray = waypoints.map((waypoint: any, index: number) => {
                const { _waypointOptions: { address, location: { longitude, latitude } } } = waypoint;
                return {
                    index, address, longitude, latitude,
                };
            });
            console.log(waypointsInfoArray);

            // routeLegs.forEach((current: any, index: number) => {
            //     if (index>0index < routeLegs.length) {
            //         const next = routeLegs[index + 1];
            //         const {startWaypointLocation,summary}= current;
            //         const {}
            //     }
            // });
            // index,long,lat,address => getAllWaysPoints()
            //
            const currList = routeLegs.map((route: any, index: number) => ({
                summary: route.summary, startWaypointLocation: route.startWaypointLocation, endWaypointLocation: route.endWaypointLocation, index,
            }));

            const routeIndex = e.route.findIndex((route: any) => route.routePath.length === currRoute.routePath.length);

            const finalObject = {
                waypointsInfoArray,
                routeIndex,

            };
        };

        const directionsError = (e: any): void => {
            alert(`Error: ${e.message}\r\nResponse Code: ${e.responseCode}`);
        };
        const initMap = (): void => {
            MapService.loadBingApi('AsrEpVFLl4bpmG5EUAggr91gdwpbwDfzb74vFXC_bFmAeTWSQnINDulRiswel16H').then((res: any) => {
                const map = new Microsoft.Maps.Map(mapRef.current);
                if (mapOptions) {
                    map.setOptions(mapOptions);
                }
                Microsoft.Maps.loadModule('Microsoft.Maps.Directions', () => {
                    directionsManager = new Microsoft.Maps.Directions.DirectionsManager(map);

                    directionsManager.showInputPanel('directionsPanel');

                    Microsoft.Maps.Events.addHandler(directionsManager, 'directionsError', directionsError);

                    Microsoft.Maps.Events.addHandler(directionsManager, 'directionsUpdated', directionsUpdated);
                });
                return map;
            });
        };
        if (mapRef.current) {
            initMap();
        }
    }, [mapRef, mapOptions]);

    return (
        <>
            <div ref={mapRef} style={{ width: 500, height: 500 }} />
            <div className="directionsContainer" style={{ backgroundColor: 'white' }}>
                <div id="directionsPanel" />
                <div id="routeInfoPanel" />
            </div>
        </>
    );
};

export default Map;
