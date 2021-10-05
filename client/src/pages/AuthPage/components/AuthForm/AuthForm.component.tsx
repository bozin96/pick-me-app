/* eslint-disable import/order */
// eslint-disable-next-line import/no-extraneous-dependencies
import classNames from 'classnames';
import React, { useCallback, useState } from 'react';
import { Button } from 'semantic-ui-react';
import './AuthForm.styles.scss';
import SignInForm from './components/SignInForm';
import SignUpForm from './components/SignUpForm';

const AuthForm: React.FC = () => {
  const [isSignUp, setIsSignUp] = useState<boolean>(true);

  const baseClass = 'pm-auth-form';

  const switchToSignUp = useCallback(() => {
    setIsSignUp(true);
  }, []);

  const switchToSignIn = useCallback(() => {
    setIsSignUp(false);
  }, []);

  const overlayClasses = classNames({
    [`${baseClass}--overlay`]: true,
    [`${baseClass}--overlay--sign-in`]: !isSignUp,
    [`${baseClass}--overlay--sign-up`]: isSignUp,
  });

  return (
    <div className={baseClass}>
      <SignInForm />
      <SignUpForm />
      <div className={overlayClasses}>
        {!isSignUp ? (
          <>
            <h2>
              Welcome Back
            </h2>
            <p>
              To keep connected with us please login with you personal info
            </p>
            <Button onClick={switchToSignUp}>Sign In</Button>

          </>
        ) : (
          <>
            <h2>
              Hello Friend
            </h2>
            <p>
              Enter your personal details and start journey with us
            </p>
            <Button onClick={switchToSignIn}>Sign Up</Button>
          </>
        )}
      </div>
    </div>
  );
};

export default AuthForm;
