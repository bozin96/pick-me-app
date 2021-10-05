import ChatPage from '../../../pages/ChatPage';
import DashboardPage from '../../../pages/DashboardPage';
import NotificationPage from '../../../pages/NotificationPage';
import RideFormPage from '../../../pages/RideFormPage';
import RideSearchPage from '../../../pages/RideSearchPage';
import UserProfilePage from '../../../pages/UserProfilePage';
import redirect from '../../modules/redirect';

export default [
    {
        path: '/create-ride',
        component: RideFormPage,
        exact: false,
      },
      {
        path: '/ride-search',
        component: RideSearchPage,
        exact: false,
        authorized: true,

      },
      {
        path: '/dashboard',
        component: DashboardPage,
        exact: false,
        authorized: true,

      },
      {
        path: '/notifications',
        component: NotificationPage,
        exact: false,
        authorized: true,

      },
      {
        path: '/user-profile/:id',
        // eslint-disable-next-line @typescript-eslint/no-empty-function
        component: UserProfilePage,
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
