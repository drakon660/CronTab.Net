import axios from "axios";
import { User } from "./interfaces";

export const axiosIdentity = axios.create({
  baseURL: "https://localhost:7269",
  headers: {
    'Content-Type': 'application/json'
  }
});


export const authUser = async (user: User) => {
  const { data } = await axiosIdentity.post<string>('/identity', user);
  return data;
}

