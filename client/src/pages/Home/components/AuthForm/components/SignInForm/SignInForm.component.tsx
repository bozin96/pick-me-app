import React, { useCallback, useState } from 'react';
import {
    Button, Form, Header, Message,
} from 'semantic-ui-react';
import history from '../../../../../../common/history';
import ApiService from '../../../../../../services/Api.service';
import CredentialsService from '../../../../../../services/Credentials.service';
import { AuthApiResponse, UserLoginInteface } from '../../../../../../types';

const SignInForm: React.FC = () => {
    const [formState, setFormState] = useState<UserLoginInteface>({
        username: '',
        password: '',
    });
    const [formError, setFormError] = useState<string>();
    const [isSubmitting, setIsSubmitting] = useState<boolean>(false);

    const handleOnChange = useCallback((e) => setFormState((prev) => ({
        ...prev,
        [e.target.name]: e.target.value,
    })), []);

    const handleOnSubmit = (): void => {
        setIsSubmitting(true);
        ApiService.login(formState).subscribe({
            next(data: AuthApiResponse) {
                const { token, userId } = data;
                CredentialsService.setToken(token);
                CredentialsService.setUserId(userId);
                history.push('/dashboard');
            },
            error(err) {
                let errorMessage = '';
                if (err?.response) {
                    const { status, data: errorData } = err.response;
                    if (status === 500) {
                        errorMessage = 'Internal Server Error, Try Again Later';
                    } else {
                        // eslint-disable-next-line prefer-destructuring
                        errorMessage = errorData?.error || errorData?.title;
                    }
                } else {
                    errorMessage = 'Error occured';
                }

                setFormError(errorMessage);
                setIsSubmitting(false);
            },

            complete() {
                setIsSubmitting(false);
            },
        });
    };
    return (
        <Form onSubmit={handleOnSubmit}>
            <Header as="h2">Sign In</Header>
            <Form.Input onChange={handleOnChange} fluid label="Username" placeholder="Username" name="username" />
            <Form.Input onChange={handleOnChange} fluid label="Password" placeholder="Password" name="password" />
            <Button type="submit" loading={isSubmitting}>Sign In</Button>

            {formError && (
                <Message
                    negative
                    header="Submission Error"
                    content={formError}
                />
            )}
        </Form>
    );
};
export default SignInForm;
