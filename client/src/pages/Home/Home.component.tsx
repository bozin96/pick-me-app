import React from 'react';
import AuthForm from './components/AuthForm';
import './Home.styles.scss';

const Home: React.FC = () => {
    const baseClass = 'pm-home';
    return (
        <div className={baseClass}>
            <AuthForm />
        </div>
    );
};

export default Home;
