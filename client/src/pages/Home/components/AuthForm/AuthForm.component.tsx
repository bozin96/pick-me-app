/* eslint-disable import/order */
import React, { useCallback, useState } from 'react';
import { Field, Form } from 'react-final-form';
import Input from '../../../../components/Input';
import Button from '../../../../components/Button';
import './AuthForm.styles.scss';
// eslint-disable-next-line import/no-extraneous-dependencies
import classNames from 'classnames';

const AuthForm: React.FC = () => {
    const [isSignUp, setIsSignUp] = useState<boolean>(false);

    const baseClass = 'pm-auth-form';

    const switchToSignUp = useCallback(() => {
        setIsSignUp(true);
    }, []);

    const switchToSignIn = useCallback(() => {
        setIsSignUp(false);
    }, []);

    const handleOnSubmit = (values: any): Promise<any> => {
        console.log(values);
        return new Promise((resolve) => {
            setTimeout(() => {
                resolve(true);
            }, 5000);
        });
    };

    const overlayClasses = classNames({
        [`${baseClass}--overlay`]: true,
        [`${baseClass}--overlay--sign-in`]: !isSignUp,
        [`${baseClass}--overlay--sign-up`]: isSignUp,
    });

    return (
        <Form onSubmit={handleOnSubmit}>
            {({ handleSubmit }) => (
                <form onSubmit={handleSubmit} className={baseClass}>
                    <div className={`${baseClass}--sign-in`}>
                        <h2 className={`${baseClass}__title`}>
                            Sign In
                        </h2>
                        <span>
                            or use your account
                        </span>
                        <Field name="signIn.email" component={Input} placeholder="Email" />
                        <Field name="SignIn.passowrd" component={Input} placeholder="Password" />

                        <span>Forgot Password</span>

                        <Button variant="primary" type="submit">Sign In</Button>
                    </div>
                    <div className={`${baseClass}--sign-up`}>
                        <h2 className={`${baseClass}__title`}>
                            Sign Up
                        </h2>
                        <span>
                            or use your email for registration
                        </span>
                        <Field name="signUp.name" component={Input} placeholder="Name" />
                        <Field name="signUp.email" component={Input} placeholder="Email" />
                        <Field name="signUp.password" component={Input} placeholder="Password" />

                        <Button variant="primary" type="submit">Sign Up</Button>
                    </div>
                    <div className={overlayClasses}>
                        {isSignUp ? (
                            <>
                                <h2>
                                    Welcome Back
                                </h2>
                                <p>
                                    To keep connected with us please login with you personal info
                                </p>
                                <Button variant="secondary" onClick={switchToSignIn}>Sign In</Button>

                            </>
                        ) : (
                            <>
                                <h2>
                                    Hello Friend
                                </h2>
                                <p>
                                    Enter your personal details and start journey with us
                                </p>
                                <Button variant="primary" onClick={switchToSignUp}>Sign Up</Button>
                            </>
                        )}
                    </div>
                </form>
            )}
        </Form>
    );
};

export default AuthForm;
