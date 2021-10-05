/* eslint-disable import/no-extraneous-dependencies */
import React, { useEffect, useState } from 'react';
import { NavLink } from 'react-router-dom';
import {
    Header as SemnaticHeader, Icon, Menu,
} from 'semantic-ui-react';
import { userInfoSubject } from '../../common/observers';
import ApiService from '../../services/Api.service';
import CredentialsService from '../../services/Credentials.service';
import { User } from '../../types';
import './Header.styles.scss';

const Header: React.FC = () => {
    const [userName, setUserName] = useState<string>('');
    const [userPhoto, setUserPhoto] = useState<string | null>('');
    const [isAuthorized, setIsAuthorized] = useState<boolean>(false);

    const token = CredentialsService.getToken();
    const userId = CredentialsService.getUserId();

    useEffect(() => {
        const loadUserData = (): void => {
            ApiService.getUser$(userId).subscribe((res: User): void => {
                const { userPhoto: photo, firstName, lastName } = res;
                setUserName(`${firstName} ${lastName}`);
                setUserPhoto(`data:image/png;base64,${photo}`);
            });
        };
        if (userId) {
            loadUserData();
        }
    }, [userId]);

    useEffect(() => {
        userInfoSubject.subscribe((res: Partial<User>) => {
            const { userPhoto: photo = '', firstName, lastName } = res;
            setUserName(`${firstName} ${lastName}`);
            setUserPhoto(photo || 'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png');
        });
    }, []);

    useEffect(() => {
        setIsAuthorized(!!token && !!userId);
    }, [token, userId]);

    return (
        <SemnaticHeader className="pm-header">
            <Menu horizontal>
                <Menu.Item>
                    <NavLink
                        to="/dashboard"
                    >
                        <h2>
                            PickMeApp
                        </h2>
                    </NavLink>
                </Menu.Item>
                <Menu.Item>
                    <span className="spacer" />
                </Menu.Item>
                {isAuthorized && (
                    <>
                        <Menu.Item>
                            <NavLink
                                to="/create-ride"
                            >
                                Ride
                            </NavLink>
                        </Menu.Item>
                        <Menu.Item>
                            <NavLink
                                to="/ride-search"
                            >
                                Search Rides
                            </NavLink>
                        </Menu.Item>
                        <Menu.Item>
                            <NavLink
                                to="/notifications"
                            >
                                <Icon name="mail" />
                            </NavLink>
                        </Menu.Item>
                        <Menu.Item>
                            <NavLink
                                to="/chats"
                            >
                                <Icon name="chat" />
                            </NavLink>
                        </Menu.Item>
                        <Menu.Item>
                            <NavLink
                                to={`/user-profile/${CredentialsService.getUserId()}`}
                            >
                                {userName}
                                {userPhoto && (
                                    <img alt="" src={userPhoto} />
                                )}
                            </NavLink>
                        </Menu.Item>
                        <Menu.Item>
                            <NavLink
                                to="/auth"
                                onClick={() => {
                                    CredentialsService.clearLocalStorage();
                                }}
                            >
                                <Icon name="sign-in" />
                            </NavLink>
                        </Menu.Item>
                    </>
                )}

            </Menu>
        </SemnaticHeader>
    );
};

export default Header;
