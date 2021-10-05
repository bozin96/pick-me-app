import React from 'react';
import AuthForm from './components/AuthForm';
import './HomePage.styles.scss';

const HomePage: React.FC = () => {
    const baseClass = 'pm-home';
    return (
        <div className={baseClass}>
            <AuthForm />
        </div>
    );
};

export default HomePage;
