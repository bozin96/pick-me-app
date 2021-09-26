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
    Form,
    Header,
    Image,
    Rating,
} from 'semantic-ui-react';
import { userInfoSubject } from '../../common/observers';
import ApiService from '../../services/Api.service';
import CredentialsService from '../../services/Credentials.service';
import { User } from '../../types';
import './UserProfile.styles.scss';

const UserProfile: React.FC = () => {
    const [userInfo, setUserInfo] = useState<User>();
    const [fetchinInfo, setFetchingInfo] = useState<boolean>(true);
    const [formSubmitting, setFormSubmitting] = useState<boolean>(false);
    const { id } = useParams() as any;
    const [editMode, setEditMode] = useState(false);
    const [formState, setFormState] = useState<any>({
        firstName: '',
        middleName: '',
        lastName: '',
        userPhoto: 'https://react.semantic-ui.com/images/wireframe/image.png',
    });

    useEffect(() => {
        const loadUserData = (): void => {
            setFetchingInfo(true);
            ApiService.getUser(id).subscribe({
                next(res: User) {
                    const {
                        firstName,
                        lastName,
                        middleName,
                        userPhoto,
                        email,
                        averateRate,
                    } = res;
                    const data = {
                        firstName,
                        lastName,
                        middleName,
                        email,
                        averateRate,
                        userPhoto: userPhoto
                            ? `data:image/png;base64,${userPhoto}`
                            : 'https://react.semantic-ui.com/images/wireframe/image.png',
                    } as User;
                    setUserInfo(data);
                    setFormState(data);
                    setFetchingInfo(false);
                },
                error(err) {
                    toast.error('Error Fetching User Info');
                    setFetchingInfo(false);
                },
            });
        };
        if (id) {
            loadUserData();
        }
    }, [id]);

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
        setFormState(userInfo);
    };
    const handleOnSubmit = (): void => {
        if (!editMode) {
            setEditMode((prev: boolean) => !prev);
        } else {
            const { userPhoto } = formState;
            const submitObject = {
                ...formState,
                userPhoto: userPhoto ? userPhoto.split(',')[1] : '',
            };
            setFormSubmitting(true);
            ApiService.updateUser(id, submitObject).subscribe({
                next(x) {
                    console.log(x);
                    userInfoSubject.next(formState);
                    setEditMode(false);
                    setFormSubmitting(false);
                    setUserInfo(formState);
                },
                error(err) {
                    toast('Error Occured');
                    setFormSubmitting(false);
                },
            });
        }
    };
    const fileInputRef = useRef<any>();

    const openFileDialog = (): void => {
        fileInputRef?.current?.click();
    };
    return (
        <div className="pm-user-profile">
            <Form onSubmit={handleOnSubmit}>
                <Dimmer.Dimmable as={Card} dimmed={fetchinInfo}>
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
                                    {userInfo?.firstName}
                                    &nbsp;
                                    {userInfo?.middleName}
                                    &nbsp;
                                    {userInfo?.lastName}
                                </>
                            )}
                        </Card.Header>
                        {!editMode && (
                            <Card.Description>{userInfo?.email}</Card.Description>
                        )}
                    </Card.Content>
                    {!editMode && (
                        <Card.Content extra>
                            <Rating
                                rating={userInfo?.averageRate}
                                maxRating={5}
                                size="huge"
                                disabled
                            />
                        </Card.Content>
                    )}
                    {isMe && (
                        <Card.Content extra>
                            {!editMode ? (
                                <Button type="submit">Edit</Button>
                            ) : (
                                <>
                                    <Button basic loading={formSubmitting} type="button" onClick={handleFormReset} color="red">
                                        Discard
                                    </Button>
                                    <Button type="submit" loading={formSubmitting} color="green">
                                        Save
                                    </Button>
                                </>
                            )}
                        </Card.Content>
                    )}
                    <Dimmer active={fetchinInfo}>
                        <Header as="h2" inverted>
                            Fetching User Info ...
                        </Header>
                    </Dimmer>
                </Dimmer.Dimmable>
            </Form>
        </div>
    );
};
export default UserProfile;
