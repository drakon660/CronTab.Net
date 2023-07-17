import axios from "axios";
import { Cron } from "./interfaces";

export const axiosCron = axios.create({
    baseURL: "https://localhost:7159",
    headers: {
        'Content-Type': 'application/json'
    },
});

export const getCron = async (token:string) => {
    axiosCron.defaults.headers["Authorization"] = `Bearer ${token}`;
    const  { data } = await axiosCron.get<Cron[]>("/cron-tab");
    return data;
}