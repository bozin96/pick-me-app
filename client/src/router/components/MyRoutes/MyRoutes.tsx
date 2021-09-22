/* eslint-disable @typescript-eslint/no-shadow */
/* eslint-disable react/jsx-props-no-spreading */
/* eslint-disable import/no-extraneous-dependencies */
import React, { useCallback } from 'react';
import { Route, Switch, useRouteMatch } from 'react-router-dom';
import EnhancedRoute from '../../routes/Enhanced';

type RoutesProps = {
  routes: any;
};
const MyRoutes:React.FC<RoutesProps> = (props:RoutesProps) => {
  const { routes } = props;
  const { url } = useRouteMatch();

  const finalPath = useCallback(
    (to = '') => {
      const { length } = url;
      if (url[length - 1] === to[0] && to[0] === '/') {
        return url + to.slice(1);
      }
      return url + to;
    },
    [url],
  );

  return (
    <Switch>
      {routes.map(({ path = '', exact = true, ...rest }) => (
        <Route
          key={`${path}`}
          path={finalPath(path)}
          exact={exact}
          render={(props) => <EnhancedRoute {...props} {...rest} />}
        />
      ))}
    </Switch>
  );
};

export default MyRoutes;
