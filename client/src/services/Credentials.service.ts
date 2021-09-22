/* eslint-disable @typescript-eslint/no-explicit-any */
const getUserId = (): any => localStorage.getItem('userId');

const setUserId = (userId: string): void => {
  localStorage.setItem('userId', userId);
};

const getToken = (): string => localStorage.getItem('token') || '';
const setToken = (token: string): any => {
  localStorage.setItem('token', token);
};
const clearLocalStorage = (): any => {
  localStorage.removeItem('token');
  localStorage.removeItem('userId');
};
export default {
  setUserId,
  getUserId,
  getToken,
  setToken,
  clearLocalStorage,
};
