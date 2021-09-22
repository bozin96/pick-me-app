import { useEffect, useState } from 'react';
import MapService from '../services/Map.service';

export default (mapContainer:any = null, mapOptions: any = null): any => {
    const [map, setMap] = useState<any>();

    useEffect(() => {
        const mapDiv = document.createElement('div');
        const initMap = (): void => {
            MapService.loadBingApi('AsrEpVFLl4bpmG5EUAggr91gdwpbwDfzb74vFXC_bFmAeTWSQnINDulRiswel16H').then((res: any) => {
                const m = new Microsoft.Maps.Map(mapDiv, mapOptions);
                setMap(m);
            });
            if (mapContainer && !mapContainer.contains(document.getElementById('myMap'))) {
                mapContainer.appendChild(mapDiv);
            }
        };
        if (mapDiv) {
            initMap();
        }
    }, [mapOptions, mapContainer]);

    return map;
};
