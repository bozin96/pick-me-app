import React, { useCallback, useState } from 'react';
import {
    Button, Form, Header, Message,
} from 'semantic-ui-react';
import ApiService from '../../../../../../services/Api.service';
import { UserRegisterInteface } from '../../../../../../types';

const SignUpForm: React.FC = (props) => {
    const [formState, setFormState] = useState<UserRegisterInteface>({
        firstName: '',
        middleName: '',
        lastName: '',
        password: '',
        confirmPassword: '',
    });
    const [formError, setFormError] = useState<string>();
    const [isSubmitting, setIsSubmitting] = useState<boolean>(false);

    const handleOnChange = useCallback((e) => setFormState((prev) => ({
        ...prev,
        [e.target.name]: e.target.value,
    })), []);

    const handleOnSubmit = async (values: any): Promise<any> => {
        setIsSubmitting(true);

        ApiService.register(formState).subscribe({
            next(x) { console.log(`got value ${x}`); },
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
                    errorMessage = 'Error occured while creating Legal Entity';
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
            <Header as="h2">Sign Up</Header>
            <Form.Input onChange={handleOnChange} fluid label="First Name" placeholder="First name" name="firstName" />
            <Form.Input onChange={handleOnChange} fluid label="Middle Name" placeholder="Middle name" name="middleName" />
            <Form.Input onChange={handleOnChange} fluid label="Last Name" placeholder="Last name" name="lastName" />
            <Form.Input onChange={handleOnChange} fluid label="Password" placeholder="Password" name="password" />
            <Form.Input onChange={handleOnChange} fluid label="Confirm Password" placeholder="Confirm Password" name="confirmPassword" />
            <Button type="submit" loading={isSubmitting}>Sign Up</Button>
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

export default SignUpForm;
