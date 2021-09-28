/* eslint-disable max-len */
/* eslint-disable no-console */
/* eslint-disable react/jsx-props-no-spreading */
import React, { useEffect, useState } from 'react';
import {
  Dimmer, Header, Segment,
} from 'semantic-ui-react';
import ApiService from '../../services/Api.service';
import CredentialsService from '../../services/Credentials.service';
import { MyDriveInterface } from '../../types';
import MyDrive from '../MyDrive';

const DrivingHistoryCard: React.FC = () => {
  const [rides, setRides] = useState<MyDriveInterface[]>([]);
  const [isFetching, setIsFetching] = useState<boolean>(false);

  useEffect(() => {
    setIsFetching(true);
    const userId = CredentialsService.getUserId();
    ApiService.getUserRidesAsDriver(userId, {})
      // .pipe(map((res) => res.slice(0, 5)))
      .subscribe({
        next(x) {
          setRides(x);
          setIsFetching(false);
        },
        error(err) {
          setIsFetching(false);
        },
        complete() {
          console.log('done');
        },
      });
  }, []);
  return (
    <Dimmer.Dimmable as={Segment} dimmed={isFetching}>
      {rides.map((el) => (
        <MyDrive {...el} />
      ))}
      <Dimmer active={isFetching}>
        <Header as="h2" icon inverted>
          Loading
        </Header>
      </Dimmer>
    </Dimmer.Dimmable>
  );
};

export default DrivingHistoryCard;
