/* eslint-disable max-len */
/* eslint-disable no-console */
/* eslint-disable react/jsx-props-no-spreading */
import React, { useEffect, useState } from 'react';
import {
  Dimmer, Header, Segment,
} from 'semantic-ui-react';
import ApiService from '../../services/Api.service';
import CredentialsService from '../../services/Credentials.service';
import { MyRideInterface } from '../../types';
import MyRide from '../MyRide';

const RidesHistoryCard: React.FC = () => {
  const [rides, setRides] = useState<MyRideInterface[]>([]);
  const [isFetching, setIsFetching] = useState<boolean>(false);

  useEffect(() => {
    setIsFetching(true);
    const userId = CredentialsService.getUserId();
    ApiService.getUserRidesAsPassenger(userId)
      .subscribe({
        next(x) {
          setRides(x);
          setIsFetching(false);
        },
        error(err) {
          setIsFetching(false);
        },
      });
  }, []);
  return (
    <Dimmer.Dimmable as={Segment} dimmed={isFetching}>
      {rides.map((el) => (
        <MyRide {...el} />
      ))}
      <Dimmer active={isFetching}>
        <Header as="h2" icon inverted>
          Loading
        </Header>
      </Dimmer>
    </Dimmer.Dimmable>
  );
};

export default RidesHistoryCard;
