/* eslint-disable import/no-extraneous-dependencies */
import React from 'react';
import { Provider } from 'react-redux';
import { Router } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import './App.scss';
import history from './common/history';
import MyRoutes from './router/components/MyRoutes';
import routes from './router/routes';
import { store } from './store';

const App: React.FC = () => {
  const baseClass = 'pm-app';

  return (
    <Provider store={store}>
      <div className={baseClass}>
        <Router history={history}>
          <MyRoutes routes={routes} />
        </Router>
        <ToastContainer />
      </div>
    </Provider>
  );
};

export default App;
