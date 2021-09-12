import React from 'react';
import './Ride.styles.scss';
import Map from './components/Map';

const mapsApiKey = 'AsrEpVFLl4bpmG5EUAggr91gdwpbwDfzb74vFXC_bFmAeTWSQnINDulRiswel16H';

const Ride: React.FC = () => {
    const baseClass = 'pm-ride';
    return (
        <div className={baseClass}>
            <Map
                mapOptions={{
                    center: [47.60357, -122.32945],
                    showLocateMeButton: true,
                    showTrafficButton: true,
                    credentials:
                        'AsrEpVFLl4bpmG5EUAggr91gdwpbwDfzb74vFXC_bFmAeTWSQnINDulRiswel16H',

                }}
            />

        </div>
    );
};

export default Ride;
