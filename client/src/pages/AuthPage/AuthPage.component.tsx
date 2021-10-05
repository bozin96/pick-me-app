import React from 'react';
import Header from '../../components/Header';
import './AuthPage.styles.scss';
import AuthForm from './components/AuthForm';

const AuthPage: React.FC = () => {
    const baseClass = 'pm-auth';
    return (
        <>
        <Header />
        <div className={baseClass}>
            <AuthForm />
        </div>
        </>
    );
};

export default AuthPage;
