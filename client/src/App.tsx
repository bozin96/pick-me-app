/* eslint-disable import/no-extraneous-dependencies */
import React from 'react';
import { Router } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import './App.scss';
import history from './common/history';
import Header from './components/Header';
import Routes from './router/components/MyRoutes';
import routes from './router/routes';

const App: React.FC = () => {
  const baseClass = 'pm-app';
  return (
    <div className={baseClass}>
        <Router history={history}>
        <Header />
        <Routes routes={routes} />
        </Router>
      <ToastContainer />
    </div>
  );
};

export default App;
