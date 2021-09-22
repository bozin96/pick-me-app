/* eslint-disable react/jsx-props-no-spreading */
/* eslint-disable import/no-extraneous-dependencies */
import React, { useMemo } from 'react';
import { RouteComponentProps, RouteProps } from 'react-router';
import { Redirect } from 'react-router-dom';
import CredentialsService from '../../../services/Credentials.service';

export type EnhancedRouteProps = {
  componentProps?: Record<string, any>;
  authorized?:boolean
} & RouteProps<string> &
  RouteComponentProps;
const EnhancedRoute:React.FC<EnhancedRouteProps> = (props:EnhancedRouteProps) => {
  const {
    authorized = false,
    component,
    componentProps,
    ...rest
  } = props;

  const lsToken = CredentialsService.getToken();

  const finalRoute = useMemo(() => {
    if ((authorized) && !lsToken) {
      return <Redirect to={{ pathname: '/auth', state: { from: props.location } }} />;
    }
    const Component = component as React.FC;
      return <Component {...rest} {...componentProps} />;
  }, [authorized, lsToken, component, rest, componentProps, props.location]);

  return finalRoute;
};

export default EnhancedRoute;
