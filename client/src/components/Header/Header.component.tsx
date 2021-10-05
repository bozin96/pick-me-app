/* eslint-disable max-len */
/* eslint-disable import/no-extraneous-dependencies */
import React, { useEffect, useState } from 'react';
import { NavLink } from 'react-router-dom';
import { toast } from 'react-toastify';
import {
    Header as SemnaticHeader, Icon, Menu,
} from 'semantic-ui-react';
import { useAppDispatch } from '../../hooks/useAppDispatch';
import { useAppSelector } from '../../hooks/useAppSelector';
import useNotifications from '../../hooks/useNotifications';
import CredentialsService from '../../services/Credentials.service';
import { clearAuth } from '../../store/reducers/auth.reducer';
import ToastMessage from '../ToastMessage';
import './Header.styles.scss';

const Header: React.FC = () => {
    const [isAuthorized, setIsAuthorized] = useState<boolean>(false);
    const dispatch = useAppDispatch();
    const newNotificationObserver$ = useNotifications(isAuthorized);

    const {
        token, userData: {
            userPhoto, firstName, lastName,
        },
        userId,
    } = useAppSelector((state) => state.auth);

    useEffect(() => {
        setIsAuthorized(!!token && !!userId);
    }, [token, userId]);

    const displayNotificationAsToast = (notification: Notification): void => {
        toast(ToastMessage, { data: notification });
    };
    useEffect(() => {
        let subscripton: any = null;
        if (newNotificationObserver$) {
            subscripton = newNotificationObserver$.subscribe((res) => displayNotificationAsToast(res));
        }
        return (() => {
            if (subscripton) {
                subscripton.unsubscribe();
            }
        });
    }, [newNotificationObserver$]);

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
                                {`${firstName} ${lastName}`}
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
                                    dispatch(clearAuth());
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
