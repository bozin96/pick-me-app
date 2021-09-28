/* eslint-disable @typescript-eslint/no-shadow */
/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable max-len */
import React, {
    useEffect, useLayoutEffect, useRef, useState,
} from 'react';
import { toast } from 'react-toastify';
import { map as mapRxjs } from 'rxjs';
import { Button, Form } from 'semantic-ui-react';
import history from '../../../common/history';
import { waypointsSubject } from '../../../common/observers';
import useMap from '../../../hooks/useMap';
import ApiService from '../../../services/Api.service';
import { Microsoft } from '../../../services/Map.service';
import './RideForm.styles.scss';

const RideForm: React.FC<any> = () => {
    const [formSubmitting, setFormSubmitting] = useState(false);
    const [formState, setFormState] = useState<any>({
        destinations: [{
        }, {}],
        options: [{}],
    });
    const [mapWaypoints, setMapWaypoints] = useState<any[]>([]);
    const { destinations, options } = formState;

    const handleAddDestination = (e: any): void => {
        setFormState((prev: any) => ({
            ...prev,
            destinations: [...prev.destinations, {}],
            options: [...prev.options, {}],
        }));
    };
    // const { onSubmit } = props;
    const mapRef = useRef(null);
    const map = useMap(mapRef.current);

    useEffect(() => {
        waypointsSubject.pipe(mapRxjs((pin: any): any => {
            const {
                location: { longitude, latitude }, address: {
                    formattedAddress,
                },
            } = pin;
            return {
                address: formattedAddress,
                location: new Microsoft.Maps.Location(latitude, longitude),
            };
        })).subscribe(((res: any) => setMapWaypoints((prev: any): any => ([...prev, res]))));
    }, []);

    useEffect(() => {
        let directionsManager: Microsoft.Maps.Directions.DirectionsManager;
        const handleUpdate = (): void => {
            const waypoints = directionsManager.getAllWaypoints();
            const currentRoute = directionsManager.getCurrentRoute();
            setFormState((prev: any) => ({
                ...prev,
                waypoints,
                routeLegs: currentRoute.routeLegs,
                originalRouteIndex: currentRoute.routeLegs[0].originalRouteIndex,
            }));
        };
        const refreshMap = (): void => {
            Microsoft.Maps.loadModule('Microsoft.Maps.Directions', (): void => {
                directionsManager = new Microsoft.Maps.Directions.DirectionsManager(map);
                Microsoft.Maps.Events.addHandler(directionsManager, 'directionsUpdated', handleUpdate);

                directionsManager.clearDisplay();
                directionsManager.clearAll();
                directionsManager.setRequestOptions({
                    routeMode: Microsoft.Maps.Directions.RouteMode.driving,
                });

                mapWaypoints
                    .map((waypointInfo: any) => new Microsoft.Maps.Directions.Waypoint(waypointInfo))
                    .forEach(
                        (waypoint: Microsoft.Maps.Directions.Waypoint) => {
                            directionsManager.addWaypoint(waypoint);
                        },
                    );

                directionsManager.calculateDirections();
            });
        };

        if (map && mapWaypoints.length) {
            refreshMap();
        }

        return () => {
            if (map) {
                Microsoft.Maps.Events.removeHandler(directionsManager, 'directionsUpdated', handleUpdate);
            }
        };
    }, [map, mapWaypoints]);

    useLayoutEffect(() => {
        const suggestionHandler = (result: any, index: number): void => {
            const { formattedSuggestion } = result;
            waypointsSubject.next(result);
            setFormState((prev: any) => {
                const { destinations: prevDestinations } = prev;
                prevDestinations[index] = {
                    pin: result,
                    tag: formattedSuggestion,
                };
                return { ...prev, destinations: prevDestinations };
            });
        };
        const setAutoSuggest = (): void => {
            destinations.forEach((element: any, index: number) => {
                Microsoft.Maps.loadModule('Microsoft.Maps.AutoSuggest', (): void => {
                    const manager = new Microsoft.Maps.AutosuggestManager({ map });
                    manager.attachAutosuggest(`#destination${index + 1}`, `#searchBoxContainer${index + 1}`, (result: any) => suggestionHandler(result, index));
                });
            });
        };
        if (map && destinations.length) {
            setAutoSuggest();
        }
    }, [destinations.length, map]);

    const handleOptionsChange = (e: any, index: number): void => {
        setFormState((prev: any) => {
            const { options: prevOptions } = formState;
            prevOptions[index][e.target.name] = e.target.value;
            return {
                ...prev,
                options: prevOptions,
            };
        });
    };
    const handleDateChange = (e: any): void => {
        setFormState((prev: any) => ({ ...prev, [e.target.name]: e.target.value }));
    };
    const handleOnSubmit = (): void => {
        const {
            waypoints = [], startDate, routeLegs = [], options, numberOfPassengers,
        } = formState;
        const submitObject = {
            startDate,
            numberOfPassengers: Number(numberOfPassengers),
            waypoints: waypoints.map((waypoint: any) => {
                const {
                    _waypointOptions: {
                        address,
                        location,
                    },
                } = waypoint;
                return {
                    address,
                    latitude: location.latitude,
                    longitude: location.longitude,
                };
            }),
            routeLegs: routeLegs.map((leg: any, index: number) => {
                const {
                    endWaypointLocation: { latitude: endLatitude, longitude: endLongitude }, summary: { distance, time }, startWaypointLocation: {
                        latitude: startLatitude, longitude: startLongitude,
                    },
                } = leg;
                return {
                    startLatitude,
                    startLongitude,
                    endLatitude,
                    endLongitude,
                    distance,
                    time,
                    price: Number(options[index].price),

                };
            }),
        };
        setFormSubmitting(true);
        ApiService.createRide(submitObject).subscribe({
            next(x) {
                toast('Ride Created Succesfully');
                setFormSubmitting(false);
                setTimeout(() => {
                    history.push('/dashboard');
                }, 1000);
            },
            error(err) {
                toast('Error Occured');
                setFormSubmitting(false);
            },
        });
    };
    const baseClass = 'pm-ride-form';

    return (
        <div className={baseClass}>
            <Form onSubmit={handleOnSubmit}>
                <div>
                    <h2>Create Ride</h2>
                    {destinations.map((dest: any, index: number) => (
                        <div id={`searchBoxContainer${index + 1}`}>
                            <Form.Input
                                label={`Destination ${index + 1}`}
                                id={`destination${index + 1}`}
                                name="tag"
                            />
                        </div>
                    ))}
                    <Button type="button" onClick={handleAddDestination}>Add Destination</Button>
                    <Form.Input
                        label="Start Date"
                        type="date"
                        name="startDate"
                        onChange={handleDateChange}
                    />
                    <Form.Input label="Number of Seats" placeholder="Seats" name="numberOfPassengers" icon="dollar sign" iconPosition="left" onChange={handleDateChange} />

                    <Button type="submit" loading={formSubmitting}>Create Ride </Button>

                </div>
                <div className={`${baseClass}__options`}>
                    {options.map((opt: any, index: number) => (
                        <div className={`${baseClass}__options__item`}>
                            <Form.Input placeholder="Price" name="price" icon="dollar sign" iconPosition="left" onChange={(e: any) => handleOptionsChange(e, index)} />

                        </div>
                    ))}
                </div>

            </Form>
            <div ref={mapRef} className="pm-map" />
        </div>
    );
};

export default RideForm;
