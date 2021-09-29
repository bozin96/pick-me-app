import React, { useLayoutEffect, useState } from 'react';
import { Button, Form, Header } from 'semantic-ui-react';
import { RideSearchDataSubject, RidesSearchResultsSubject } from '../../../../../common/observers';
import useMap from '../../../../../hooks/useMap';
import ApiService from '../../../../../services/Api.service';
import './RideSearchForm.styles.scss';

const RideSearchForm: React.FC = () => {
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
            destinationFrom, destinationTo, dateTime, numberOfPlaces,
        } = formState;

        const startLongitude = destinationFrom.pin.location.longitude;
        const startLatitude = destinationFrom.pin.location.latitude;
        const endLongitude = destinationTo.pin.location.longitude;
        const endLatitude = destinationTo.pin.location.latitude;

        RideSearchDataSubject.next({
            startWaypoint: destinationFrom.tag,
            endWaypoint: destinationTo.tag,
            startDate: dateTime,
            numberOfPlaces: Number(numberOfPlaces),
            startLongitude,
            startLatitude,
            endLongitude,
            endLatitude,
            dateTime,
        });
        ApiService.getRides({
            startLongitude,
            startLatitude,
            endLongitude,
            endLatitude,
            dateTime,
            numberOfPlaces: Number(numberOfPlaces),
        }).subscribe((res) => RidesSearchResultsSubject.next(res));
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
                    label="From"
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
                name="numberOfPlaces"
                onChange={handleOnChange}
            />
            <Button type="submit">Search</Button>
        </Form>
    );
};
export default RideSearchForm;
