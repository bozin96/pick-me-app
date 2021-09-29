import React from 'react';
import Redirect from '../components/Redirect';

export default
(path:string, absolute = false):React.FC => () => <Redirect to={path} absolute={absolute} />;
