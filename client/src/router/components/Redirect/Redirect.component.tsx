/* eslint-disable import/no-extraneous-dependencies */
import React, { useMemo } from 'react';
import { Redirect as ReactRedirect, RedirectProps, useRouteMatch } from 'react-router-dom';

export type Props = RedirectProps & {
  absolute?: boolean;
};
const Redirect:React.FC<Props> = (props:Props) => {
  const { to, absolute = false } = props;
  const { url } = useRouteMatch();

  const finalPath = useMemo(() => {
    if (absolute) return to;
    const { length: urlLength } = url;
    const toString = to as string;
    if (url[urlLength - 1] === toString[0] && toString[0] === '/') {
      return url + toString.slice(1);
    }
    return url + to;
  }, [url, to, absolute]);

  return <ReactRedirect to={finalPath} />;
};

export default Redirect;
