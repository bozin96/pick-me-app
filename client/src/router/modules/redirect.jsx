import React from 'react';
import Redirect from '../components/Redirect';

export default (path, absolute = false) => () => <Redirect to={path} absolute={absolute} />;
