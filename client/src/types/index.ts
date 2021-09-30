export interface RouteLeg {
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

export interface MyDriveInterface {
    id: string;
    startDate: string,
    routeLegs: RouteLeg[];
    waypoints: Waypoint[];
    numberOfPassengers: number;
    petFriendly: boolean;
    driverName: string,
    driverId: string,
    driverRate: number,
    routeIndex: number,
    price: number,
    time: string,
    numberFreeSeats: number
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
    body: string, // "Body content"
    header: string, // "Header"
    rideId: string, // "7ff2c780-a493-4908-8460-d665966fc3ff"
    type: number, // 0
    userFromName: string,
    userFromId: string, // "6384b6be-7fb7-46a1-80df-8c8ba4c63835"
    userFromImage: any,
    userToId: string // "d3015522-d131-49dd-9220-4a50c0da3cd1
    id: string
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
}

export interface ChatMessageSend {
    chatId: string;
    text: string;
}
export interface ChatMessageReceive {
    text: string;
    sendUserId: string;
    timestamp: string
}

export interface MyRideInterface {
    driverName: string,
    endWaypoint: string,
    id: string,
    passengerId: string,
    review: number
    rideId: string
    startDate: string,
    startWaypoint: string
}

export interface IChat {
    chatId: string,
    firstUserId: string,
    firstUserName: string,
    firstUserPhoto: string,
    secondUserId: string,
    secondUserName: string,
    secondUserPhoto: string,
    numberOfUnreadedMessages: number,
    lastMessageSenderId: number,
}

export interface IChatMessage {
    chat: string
    chatId: string,
    creationDate: string,
    id: string,
    sendUser: string,
    sendUserId: string,
    text: string,
    timestamp: string,
}
export interface RideRequestNotificationInterface {
    body: string,
    header: string,
    rideId: string,
    type: number,
    userFromName: string,
    userFromId: string,
    userFromImage: string,
    userToId: string,
    id: string,
    isVisible: boolean
}

export interface Notification {
    rideId: string,
    type: 'RequestForRide' | 'ResponseOnRideRequest' | 'RideReview',
    header: string,
    body: string,
    userFromId: string,
    userToId: string,
    payload: string
}
