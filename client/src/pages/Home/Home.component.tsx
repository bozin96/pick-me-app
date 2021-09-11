import React from 'react';
import Card from './components/Card';
import './Home.styles.scss';
import AuthForm from './components/AuthForm';

const Home: React.FC = () => {
    const baseClass = 'pm-home';
    return (
        <div className={baseClass}>
            <AuthForm />
        </div>
    );
};

export default Home;
