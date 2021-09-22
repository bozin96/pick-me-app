/* eslint-disable import/no-extraneous-dependencies */
import React from 'react';
import { NavLink } from 'react-router-dom';
import {
    Header as SemnaticHeader, Icon, Menu,
} from 'semantic-ui-react';
import useNotifications from '../../hooks/useNotifications';
import CredentialsService from '../../services/Credentials.service';
import './Header.styles.scss';

const Header: React.FC = () => {
    useNotifications(!!CredentialsService.getToken());

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
                        to="/auth"
                        onClick={() => {
                            CredentialsService.clearLocalStorage();
                        }}
                >
                        <Icon name="sign-in" />
                </NavLink>
                </Menu.Item>
        </Menu>
    </SemnaticHeader>
);
};

export default Header;
