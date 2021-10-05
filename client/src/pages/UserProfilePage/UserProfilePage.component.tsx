/* eslint-disable max-len */
/* eslint-disable import/no-extraneous-dependencies */
import React, {
    useCallback, useEffect, useRef, useState,
} from 'react';
import { useParams } from 'react-router';
import { toast } from 'react-toastify';
import {
    Button,
    Card,
    Dimmer,
    Form, Icon,
    Image,
} from 'semantic-ui-react';
import { useAppDispatch } from '../../hooks/useAppDispatch';
import { useAppSelector } from '../../hooks/useAppSelector';
import ApiService from '../../services/Api.service';
import CredentialsService from '../../services/Credentials.service';
import { setUserData } from '../../store/reducers/auth.reducer';
import './UserProfilePage.styles.scss';

const UserProfilePage: React.FC = () => {
    const [formSubmitting, setFormSubmitting] = useState<boolean>(false);
    const { id } = useParams() as any;
    const [editMode, setEditMode] = useState(false);
    const dispatch = useAppDispatch();
    const { userData } = useAppSelector((state) => state.auth);

    const [formState, setFormState] = useState<any>({
        firstName: '',
        middleName: '',
        lastName: '',
        userPhoto: 'https://react.semantic-ui.com/images/wireframe/image.png',
    });

    useEffect(() => {
        if (JSON.stringify(userData) !== '{}') setFormState(userData);
    }, [userData]);

    const isMe = id === CredentialsService.getUserId();

    const handleOnChange = useCallback((e) => {
        setFormState((prev: any) => ({
            ...prev,
            [e.target.name]: e.target.value,
        }));
    }, []);

    const onFileChange = useCallback((e) => {
        const file = e.target.files[0];
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = function () {
            setFormState((prev: any) => ({ ...prev, userPhoto: reader.result }));
        };
    }, []);

    const handleFormReset = (): void => {
        setEditMode(false);
        setFormState(userData);
    };
    const handleOnSubmit = (): void => {
        const { userPhoto } = formState;
        const submitObject = {
            ...formState,
            userPhoto: userPhoto ? userPhoto.split(',')[1] : '',
        };
        setFormSubmitting(true);
        ApiService.updateUser$(id, submitObject).subscribe({
            next() {
                setEditMode(false);
                setFormSubmitting(false);
                dispatch(setUserData(formState));
            },
            error() {
                toast('Error Occured');
                setFormSubmitting(false);
            },
        });
    };
    const fileInputRef = useRef<any>();

    const openFileDialog = (): void => {
        fileInputRef?.current?.click();
    };
    return (
        <div className="pm-user-profile">
            <Form onSubmit={handleOnSubmit}>
                <Dimmer.Dimmable as={Card}>
                    <Image src={formState.userPhoto} ui={false} />
                    {editMode && (
                        <>
                            <input
                                ref={fileInputRef}
                                name="userPhoto"
                                type="file"
                                hidden
                                onChange={onFileChange}
                            />
                            <Button
                                type="button"
                                onClick={openFileDialog}
                                content="Upload Image"
                            />
                        </>
                    )}
                    <Card.Content>
                        <Card.Header>
                            {isMe && editMode ? (
                                <>
                                    <Form.Input
                                        name="firstName"
                                        placeholder="First Name"
                                        value={formState.firstName}
                                        onChange={handleOnChange}
                                    />
                                    <Form.Input
                                        name="middleName"
                                        placeholder="Middle Name"
                                        value={formState.middleName}
                                        onChange={handleOnChange}
                                    />
                                    <Form.Input
                                        name="lastName"
                                        placeholder="Last Name"
                                        value={formState.lastName}
                                        onChange={handleOnChange}
                                    />
                                    <Form.Input
                                        name="email"
                                        placeholder="Email"
                                        value={formState.email}
                                        onChange={handleOnChange}
                                    />
                                </>
                            ) : (
                                <>
                                    {userData?.firstName}
                                    &nbsp;
                                    {userData?.middleName}
                                    &nbsp;
                                    {userData?.lastName}
                                </>
                            )}
                        </Card.Header>
                        {!editMode && (
                            <Card.Description>{userData?.email}</Card.Description>
                        )}
                    </Card.Content>
                    {!editMode && (
                        <Card.Content extra>
                            {`${userData?.averageRate} / 5`}
                            <Icon name="star" />
                            {` from ${userData?.numberOfRates} rates`}
                        </Card.Content>
                    )}
                    {isMe && (
                        <Card.Content extra>
                            {!editMode ? (
                                <button type="button" className="pm-edit-button" onClick={() => setEditMode(true)}>Edit</button>
                            ) : (
                                <>
                                    <button type="button" onClick={handleFormReset} className="pm-discard-button">
                                        Discard
                                    </button>
                                    <Button type="submit" loading={formSubmitting} color="green">
                                        Save
                                    </Button>
                                </>
                            )}
                        </Card.Content>
                    )}
                </Dimmer.Dimmable>
            </Form>
        </div>
    );
};
export default UserProfilePage;
