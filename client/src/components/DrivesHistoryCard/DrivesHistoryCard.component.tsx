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

const DrivesHistoryCard: React.FC = () => {
  const [drives, setDrives] = useState<MyDriveInterface[]>([]);
  const [isFetching, setIsFetching] = useState<boolean>(false);

  useEffect(() => {
    setIsFetching(true);
    const userId = CredentialsService.getUserId();
    ApiService.getUserRidesAsDriver$(userId, {})
      .subscribe({
        next(x) {
          setDrives(x);
          setIsFetching(false);
        },
        error() {
          setIsFetching(false);
        },
        complete() {
          console.log('done');
        },
      });
  }, []);
  return (
    <Dimmer.Dimmable as={Segment} dimmed={isFetching}>
      {drives.map((el) => (
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

export default DrivesHistoryCard;
