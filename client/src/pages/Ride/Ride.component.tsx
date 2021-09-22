import React from 'react';
import Map from './components/Map';
import './Ride.styles.scss';

const mapsApiKey = 'AsrEpVFLl4bpmG5EUAggr91gdwpbwDfzb74vFXC_bFmAeTWSQnINDulRiswel16H';

const Ride: React.FC = () => {
    const baseClass = 'pm-ride';
    return (
        <div className={baseClass}>
            <Map
                mapOptions={{
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
