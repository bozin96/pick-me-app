import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { User } from '../../types/index';

interface AuthState {
    token: string,
    userId: string,
    userData: User
}

const initialState: AuthState = {
    token: '',
    userId: '',
    userData: {} as User,
};

const authSlice = createSlice({
    name: 'auth',
    initialState,
    reducers: {
        setToken: (state, action: PayloadAction<string>) => ({
            ...state,
            token: action.payload,
        }),
        setUserId: (state, action: PayloadAction<string>) => ({
            ...state,
            userId: action.payload,
        }),
        setUserData: (state, action: PayloadAction<User>) => ({
            ...state,
            userData: action.payload,
        }),
        clearAuth: () => initialState,
    },
});

export const {
    setToken, setUserData, clearAuth, setUserId,
} = authSlice.actions;

export default authSlice.reducer;
