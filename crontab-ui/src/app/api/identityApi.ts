import axios from "axios";
import { User } from "./interfaces";

axios.defaults.withCredentials = true;

export const axiosIdentity = axios.create({
  baseURL: "https://localhost:7269",
  headers: {
    'Content-Type': 'application/json'
  },
});


export const authUser = async (user: User) => {
  const { data } = await axiosIdentity.post<string>('/identity/token', user);
  return data;
}

export const refreshTokenUser = async () => {
  const { data } = await axiosIdentity.post<string>('/identity/refresh-token');
  return data;
}

