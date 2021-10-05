import AuthPage from '../pages/AuthPage';
import redirect from './modules/redirect';
import PrivateSubrouter from './subrouter/Private/Private.subrouter';

export default [
  {
    path: '/',
    component: redirect('/auth', true),
    authorized: false,
  },
  {
    path: '/auth',
    authorized: false,
    component: AuthPage,
    exact: false,
  },
  {
    path: '/',
    component: PrivateSubrouter,
    exact: false,
    authorized: true,
  },
];
