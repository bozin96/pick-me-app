/* eslint-disable max-len */
/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable react/jsx-props-no-spreading */
/* eslint-disable react/jsx-no-undef */
import React, { useCallback, useEffect, useState } from 'react';
import { toast } from 'react-toastify';
import { List } from 'semantic-ui-react';
import { RideSearchDataSubject, RidesSearchResultsSubject } from '../../../common/observers';
import Ride from '../../../components/Ride';
import ApiService from '../../../services/Api.service';
import { MyDriveInterface } from '../../../types';
import Chat from '../../ChatPage/components/Chat/Chat.component';
import RideSearchForm from './components/RideSearchForm';
import './RideSearch.styles.scss';

const RideSearch: React.FC = () => {
  const [ridesList, setRidesList] = useState<any>([]);
  const [searchRideInfo, setSearchRideInfo] = useState({});
  const [chatInfo, setChatInfo] = useState<any>({});

  useEffect(() => {
    const subscription = RidesSearchResultsSubject.subscribe((res) => setRidesList(res));
    return () => {
      subscription.unsubscribe();
    };
  }, []);

  useEffect(() => {
    const subscription = RideSearchDataSubject.subscribe((res: any) => setSearchRideInfo(res));
    return () => {
      subscription.unsubscribe();
    };
  }, []);

  const handleRideApply = useCallback((rideId: string): void => {
    ApiService.requestRide(rideId, searchRideInfo).subscribe(() => toast('Ride Applied Successfully'));
  }, [searchRideInfo]);

  const baseClass = 'pm-ride-search';

  const handleChatClick = (userId: string): void => {
    ApiService.getOrCreateChat(userId).subscribe((res: any) => setChatInfo({ ...res, receiverId: userId }));
  };

  return (
    <div className={baseClass}>
      <h2>Search Results</h2>
      <div className={`${baseClass}__content`}>

        <RideSearchForm />
        <List divided className="pm-rides-list">
          {ridesList.map((ride: MyDriveInterface) => (
            <Ride ride={ride} onApply={handleRideApply} onChatClick={handleChatClick} />
          ))}

        </List>
        {chatInfo.chatId && <Chat chatId={chatInfo.chatId} receiverId={chatInfo.receiverId} />}
      </div>
    </div>
  );
};

export default RideSearch;
