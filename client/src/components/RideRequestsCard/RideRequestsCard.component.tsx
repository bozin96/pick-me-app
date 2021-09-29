/* eslint-disable max-len */
/* eslint-disable no-console */
/* eslint-disable react/jsx-props-no-spreading */
import React, { useEffect, useState } from 'react';
import { Dimmer, Segment } from 'semantic-ui-react';
import ApiService from '../../services/Api.service';
import CredentialsService from '../../services/Credentials.service';
import RideRequest from '../RideRequest';

const RideRequestsCard: React.FC = () => {
  const [requestsList, setRequestsList] = useState<any[]>([]);
  const [isFetching, setIsFetching] = useState<boolean>(false);

  useEffect(() => {
    // setIsFetching(true);
    ApiService.getNotifications(CredentialsService.getUserId()).subscribe({

      next(x) {
        setRequestsList(x);
        setIsFetching(false);
      },
      error(err) {
        setIsFetching(false);
      },
    });
  }, []);
  return (
    <Dimmer.Dimmable as={Segment} dimmed={isFetching}>
      {requestsList.map((req: any) => (
        <RideRequest {...req} />
      ))}
    </Dimmer.Dimmable>
  );
};

export default RideRequestsCard;
