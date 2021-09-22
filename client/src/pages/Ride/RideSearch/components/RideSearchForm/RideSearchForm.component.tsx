import React, { useLayoutEffect, useState } from 'react';
import { Button, Form, Header } from 'semantic-ui-react';
import { RideSearchDataSubject, RidesSearchResultsSubject } from '../../../../../common/observers';
import useMap from '../../../../../hooks/useMap';
import ApiService from '../../../../../services/Api.service';
import './RideSearchForm.styles.scss';

type RideSearchFormTypes = {
}
const RideSearchForm: React.FC<any> = (props: any) => {
    const baseClass = 'pm-ride-search-form';
    const map = useMap();
    const [formState, setFormState] = useState<any>();

    useLayoutEffect(() => {
        const destinationFromSuggestionHandler = (result: any): void => {
            const { formattedSuggestion } = result;
            setFormState((prev: any) => ({
                ...prev,
                destinationFrom: {
                    tag: formattedSuggestion,
                    pin: result,
                },
            }));
        };
        const destinationToSuggestionHandler = (result: any): void => {
            const { formattedSuggestion } = result;
            setFormState((prev: any) => ({
                ...prev,
                destinationTo: {
                    tag: formattedSuggestion,
                    pin: result,
                },
            }));
        };
        const linkInputsWithAutoSuggest = (): void => {
            Microsoft.Maps.loadModule('Microsoft.Maps.AutoSuggest', (): void => {
                const manager = new Microsoft.Maps.AutosuggestManager({ map });
                manager.attachAutosuggest('#destinationFrom', '#searchBoxContainerDestinationFrom', destinationFromSuggestionHandler);
            });
            Microsoft.Maps.loadModule('Microsoft.Maps.AutoSuggest', (): void => {
                const manager = new Microsoft.Maps.AutosuggestManager({ map });
                manager.attachAutosuggest('#destinationTo', '#searchBoxContainerDestinationTo', destinationToSuggestionHandler);
            });
        };

        if (map) linkInputsWithAutoSuggest();
    }, [map]);

    const handleOnSubmit = (): void => {
        const {
            destinationFrom, destinationTo, dateTime, numberOfPassengers,
        } = formState;

        const startLongitude = destinationFrom.pin.location.longitude;
        const startLatitude = destinationFrom.pin.location.latitude;
        const endLongitude = destinationTo.pin.location.longitude;
        const endLatitude = destinationTo.pin.location.latitude;

        RideSearchDataSubject.next({
            startWaypoint: destinationFrom.tag,
            endWaypoint: destinationTo.tag,
            startDate: dateTime,
            numberOfPassengers: Number(numberOfPassengers),
            startLongitude,
            startLatitude,
            endLongitude,
            endLatitude,
            dateTime,
        });
        // "startWaypoint": "Niš, Serbia",
        // "endWaypoint": "Prokuplje, Serbia",
        // "numberOfPassengers": 2,
        // "startDate": "9/14/2021 12:00:00 AM"
        ApiService.getRides({
            startLongitude,
            startLatitude,
            endLongitude,
            endLatitude,
            dateTime,
            numberOfPassengers: Number(numberOfPassengers),
        }).subscribe((res) => RidesSearchResultsSubject.next(res));

        // startLongitude=21.894499&startLatitude=43.316868&endLo
        // ngitude=21.330828&endLatitude=43.579903&numberOfPassengers=2
    };

    const handleOnChange = (e: any): void => {
        setFormState((prev: any) => ({
            ...prev,
            [e.target.name]: e.target.value,
        }));
    };
    return (
        <Form onSubmit={handleOnSubmit} className={baseClass}>
            <Header> Search For the Ride</Header>
            <div id="searchBoxContainerDestinationFrom">
                <Form.Input
                    label="Form"
                    id="destinationFrom"
                    name="tag"
                />
            </div>
            <div id="searchBoxContainerDestinationTo">
                <Form.Input
                    label="To"
                    id="destinationTo"
                    name="tag"
                />
            </div>
            <Form.Input
                label="Date"
                type="date"
                name="dateTime"
                onChange={handleOnChange}
            />
            <Form.Input
                label="Number of Seats"
                type="number"
                max={8}
                min={0}
                name="numberOfPassengers"
                onChange={handleOnChange}
            />
            <Button type="submit">Search</Button>
        </Form>
    );
};
export default RideSearchForm;
