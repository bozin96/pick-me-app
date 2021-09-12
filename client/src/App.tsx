import React from 'react';
import './App.scss';
import Ride from './pages/Ride';

const App: React.FC = () => {
  const baseClass = 'pm-app';
  return (
    <div className={baseClass}>
      <header className={`${baseClass}__header`} />
      <Ride />
    </div>
  );
};

export default App;
