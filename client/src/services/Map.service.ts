/* eslint-disable import/no-mutable-exports */
/* eslint-disable @typescript-eslint/no-explicit-any */
export interface MapWindow extends Window {
    Microsoft: any;
    bingAPIReady: () => void;
}

declare let window: MapWindow;
export let Microsoft: any;

const loadBingApi = (key?: string): Promise<void> => {
    const callbackName = 'bingAPIReady';
    let url = `https://www.bing.com/api/maps/mapcontrol?callback=${callbackName}`;
    if (key) {
        url += `&key=${key}`;
    }

    return new Promise((resolve, reject) => {
        const script = document.createElement('script');
        script.type = 'text/javascript';
        script.async = true;
        script.defer = true;
        script.src = url;
        window.bingAPIReady = () => {
            Microsoft = window.Microsoft;
            resolve();
        };
        script.onerror = (error: any) => {
            reject(error);
        };
        document.body.appendChild(script);
    });
};

export default {
    loadBingApi,
};
