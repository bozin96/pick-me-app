/* eslint-disable react/jsx-props-no-spreading */
/* eslint-disable react/jsx-no-undef */
import React, { useCallback, useEffect, useState } from 'react';
import { toast } from 'react-toastify';
import { List } from 'semantic-ui-react';
import { RideSearchDataSubject, RidesSearchResultsSubject } from '../../../common/observers';
import Ride from '../../../components/Ride';
import ApiService from '../../../services/Api.service';
import { MyRideInterface } from '../../../types';
import RideSearchForm from './components/RideSearchForm';
import './RideSearch.styles.scss';

const RideSearch: React.FC = () => {
  const [ridesList, setRidesList] = useState<any>([]);
  const [searchRideInfo, setSearchRideInfo] = useState({});

  useEffect(() => {
    RidesSearchResultsSubject.subscribe((res) => setRidesList(res));
  }, []);

  useEffect(() => {
    RideSearchDataSubject.subscribe((res: any) => setSearchRideInfo(res));
  }, []);

  const handleRideApply = useCallback((rideId: string): void => {
    ApiService.requestRide(rideId, searchRideInfo).subscribe((res) => toast('Ride Applied Successfully'));
  }, [searchRideInfo]);

  const baseClass = 'pm-ride-search';

  return (
    <div className={baseClass}>
      <h2>Search Results</h2>
      <div className={`${baseClass}__content`}>

        <RideSearchForm />
        <List divided className="pm-rides-list">
          {ridesList.map((ride: MyRideInterface) => (
            <Ride ride={ride} onApply={handleRideApply} />
          ))}

        </List>
      </div>
    </div>
  );
};

export default RideSearch;
