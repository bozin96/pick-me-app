import React, { useEffect, useState } from 'react';
import { Dimmer } from 'semantic-ui-react';
import Header from '../../../components/Header';
import { useAppDispatch } from '../../../hooks/useAppDispatch';
import { useAppSelector } from '../../../hooks/useAppSelector';
import ApiService from '../../../services/Api.service';
import CredentialsService from '../../../services/Credentials.service';
import { setToken, setUserData, setUserId } from '../../../store/reducers/auth.reducer';
import { User } from '../../../types';
import MyRoutes from '../../components/MyRoutes';
import routes from './routes';

const PrivateSubrouter: React.FC = () => {
  const lsToken = CredentialsService.getToken();
  const lsUserId = CredentialsService.getUserId();
  const { userId, userData, token } = useAppSelector((state) => state.auth);
  const [isFetching, setIsFetching] = useState<boolean>(false);
  const dispatch = useAppDispatch();

  useEffect(() => {
    if (lsToken && !token) {
      dispatch(setToken(lsToken));
    }
    if (lsUserId && !userId) {
      dispatch(setUserId(lsUserId));
    }
  }, [lsToken, token, dispatch, userId, lsUserId]);

  useEffect(() => {
    const fetchUserInfo = (): void => {
      setIsFetching(true);

      ApiService.getUser$(userId).subscribe((res: User): void => {
        const { userPhoto: photo, ...rest } = res;

        dispatch(setUserData({ ...rest, userPhoto: `data:image/png;base64,${photo}` } as User));
        setIsFetching(false);
      });
    };

    if (JSON.stringify(userData) === '{}' && userId !== '') {
      fetchUserInfo();
    }
  }, [dispatch, userData, userId]);

  if (isFetching) {
    return (
        <Dimmer active inverted>
          <h2 color="black">Loading</h2>
        </Dimmer>
    );
  }

  return (
    <div className="ickyc-widget">
      <Header />
      <div className="ickyc-widget__content">
        <MyRoutes routes={routes} />
      </div>
    </div>
  );
};

export default PrivateSubrouter;
