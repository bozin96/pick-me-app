/* eslint-disable max-len */
/* eslint-disable react/jsx-props-no-spreading */
/* eslint-disable import/no-extraneous-dependencies */
import React, { useMemo } from 'react';
import { useAppSelector } from '../../../hooks/useAppSelector';
import CredentialsService from '../../../services/Credentials.service';
import { EnhancedRouteProps } from '../../../types';
import Redirect from '../../components/Redirect';

const EnhancedRoute:React.FC<EnhancedRouteProps> = (props) => {
  const {
    authorized = false,
    component,
    componentProps,
    ...rest
  } = props;
  const lsToken = CredentialsService.getToken();
  const lsUserId = CredentialsService.getUserId();
  const { token, userId } = useAppSelector((state) => state.auth);

  const isAuthenticated = useMemo(() => (lsToken || token) && (lsUserId || userId), [lsUserId, token, userId, lsToken]);

  const finalRoute = useMemo(() => {
    const Component = component as React.FC;

    if ((authorized) && !isAuthenticated) {
      return <Redirect to={{ pathname: '/auth', state: { from: props.location } }} />;
    }
    return <Component {...rest} {...componentProps} />;
  }, [authorized, isAuthenticated, component, rest, componentProps, props.location]);

  return finalRoute;
};

export default EnhancedRoute;
