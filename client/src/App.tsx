import React from 'react';
import './App.scss';
import Home from './pages/Home';

const App: React.FC = () => {
  const baseClass = 'pm-app';
  return (
    <div className={baseClass}>
      <header className={`${baseClass}__header`} />

      <Home />
    </div>
  );
};

export default App;
