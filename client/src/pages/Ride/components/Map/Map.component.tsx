/* eslint-disable no-param-reassign */
/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable max-len */
/* eslint-disable react/require-default-props */
import React, {
    createContext, useEffect,
} from 'react';
import MapService, { Microsoft } from '../../../../services/Map.service';
import './Map.styles.scss';

type IMapProps = {
    mapOptions?: any;
}

export const MapContext = createContext<Microsoft.Maps.Map>({} as Microsoft.Maps.Map);

const Map = React.forwardRef<HTMLDivElement, IMapProps>((props: IMapProps, ref) => {
    const { mapOptions = {} } = props;

    useEffect(() => {
        const initMap = (): void => {
            MapService.loadBingApi('AsrEpVFLl4bpmG5EUAggr91gdwpbwDfzb74vFXC_bFmAeTWSQnINDulRiswel16H').then((res: any) => {
                const m = new Microsoft.Maps.Map('#map');
                if (mapOptions) {
                    m.setOptions(mapOptions);
                }
            });
        };
        initMap();
    }, [mapOptions]);

    const baseClass = 'pm-map';

    return (
        <div
            id="map"
            className={`${baseClass}`}
        />
    );
});

export default Map;
