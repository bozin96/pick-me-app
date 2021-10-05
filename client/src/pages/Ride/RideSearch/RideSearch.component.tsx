/* eslint-disable import/no-extraneous-dependencies */
/* eslint-disable max-len */
/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable react/jsx-props-no-spreading */
/* eslint-disable react/jsx-no-undef */
import classNames from 'classnames';
import React, { useCallback, useEffect, useState } from 'react';
import { toast } from 'react-toastify';
import { Icon, Label, List } from 'semantic-ui-react';
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
  const [hideChat, setHideChat] = useState(false);

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
    ApiService.requestRide$(rideId, searchRideInfo).subscribe(() => toast('Ride Applied Successfully'));
  }, [searchRideInfo]);

  const baseClass = 'pm-ride-search';

  const handleChatClick = (userId: string): void => {
    ApiService.getOrCreateChat$(userId).subscribe((res: any) => setChatInfo({ ...res, receiverId: userId }));
  };

  const chatClasses = classNames(({
    'pm-collapsible-chat': true,
    'pm-collapsible-chat--collapsed': hideChat,
  }));
  return (
    <div className={baseClass}>
      <h2>Search Results</h2>
      <div className={`${baseClass}__content`}>

        <RideSearchForm />
        <List divided className="pm-rides-list">
          {!ridesList.length ? (
            <Label content="No Rides To Show" />
          ) : (
            ridesList.map((ride: MyDriveInterface) => (
              <Ride ride={ride} onApply={handleRideApply} onChatClick={handleChatClick} />
            ))
          )}

        </List>
        {chatInfo.chatId
          && (
            <div className={chatClasses}>
              <Icon name="caret square right" size="big" onClick={() => setHideChat((prev) => !prev)} />
              <Chat chatId={chatInfo.chatId} receiverId={chatInfo.receiverId} />
            </div>
          )}
      </div>
    </div>
  );
};

export default RideSearch;
