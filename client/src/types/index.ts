export interface RouteLegInterface {
    startLongitude: number,
    startLatitude: number,
    endLongitude: number,
    endLatitude: number,
    distance: number,
    time: number,
    numberOfFreeSpaces: number
}

export interface Waypoint {
    address: string,
    longitude: number,
    latitude: number
}

export type MyRideInterface = {
    id: string;
    startDate: string,
    routeLegs: RouteLegInterface[];
    waypoints: Waypoint[];
    numberOfPassengers: number;
    petFriendly: boolean;
    driverName: string

}

export interface UserLoginInteface {
    username: string;
    password: string,
}

export interface UserRegisterInteface {
    firstName: string,
    middleName: string,
    lastName: string,
    password: string,
    confirmPassword: string
}

export interface AuthApiResponse {
    errors: any,
    refreshToken: string,
    success: boolean,
    token: string,
    userId: string
}

export interface RequestType {
    Body: string, // "Body content"
    Header: string, // "Header"
    RideId: string, // "7ff2c780-a493-4908-8460-d665966fc3ff"
    Type: number, // 0
    UserFromName: string,
    UserFromId: string, // "6384b6be-7fb7-46a1-80df-8c8ba4c63835"
    UserFromImage: any,
    UserToId: string // "d3015522-d131-49dd-9220-4a50c0da3cd1
    Id: string
}

export interface User {
    averageRate: number,
    email: string,
    firstName: string,
    id: string,
    lastName: string,
    middleName: null,
    numberOfRates: number
    userPhoto: string | null
    averateRate:number
}
