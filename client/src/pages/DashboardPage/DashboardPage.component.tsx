import React from 'react';
import { Grid } from 'semantic-ui-react';
import DrivesHistoryCard from '../../components/DrivesHistoryCard';
import RidesHistoryCard from '../../components/RidesHistoryCard';
import './DashboardPage.styles.scss';

const DashboardPage: React.FC = () => (
    <Grid divided columns="2" className="pm-dashboard">
        <Grid.Row>
            <Grid.Column>
                <h2>My Recent Drives</h2>
                <DrivesHistoryCard />
            </Grid.Column>
            <Grid.Column>
                <h2>My Recent Rides</h2>
                <RidesHistoryCard />
            </Grid.Column>
        </Grid.Row>
    </Grid>
);

export default DashboardPage;
