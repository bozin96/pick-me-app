import ChatPage from '../pages/ChatPage';
import Dashboard from '../pages/Dashboard';
import Home from '../pages/Home';
import Notifications from '../pages/Notifications';
import RideForm from '../pages/Ride/RideForm';
import RideSearch from '../pages/Ride/RideSearch';
import UserProfile from '../pages/UserProfile';
import redirect from './modules/redirect';

export default [
  {
    path: '/not-found',
    component: () => 'not found',
  },
  {
    path: '/auth',
    authorized: false,
    component: Home,
    exact: false,
  },
  {
    path: '/create-ride',
    component: RideForm,
    exact: false,
  },
  {
    path: '/ride-search',
    component: RideSearch,
    exact: false,
    authorized: true,

  },
  {
    path: '/dashboard',
    component: Dashboard,
    exact: false,
    authorized: true,

  },
  {
    path: '/notifications',
    component: Notifications,
    exact: false,
    authorized: true,

  },
  {
    path: '/user-profile/:id',
    // eslint-disable-next-line @typescript-eslint/no-empty-function
    component: UserProfile,
    authorized: true,
    exact: true,

  },
  {
    path: '/chats',
    component: ChatPage,
    exact: false,
    authorized: true,

  },
  {
    path: '*',
    component: redirect('/auth', true),
  },
];
