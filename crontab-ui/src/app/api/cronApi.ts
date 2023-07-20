import axios from "axios";
import { Cron } from "./interfaces";
import { store } from "../../store";
import { refreshUser }  from "../../features/auth/authSlice";
import jwt_decode from "jwt-decode";

axios.defaults.withCredentials = true;

export const axiosCron = axios.create({
    baseURL: "https://localhost:7159",
    headers: {
        'Content-Type': 'application/json'
    },
});

export const getCron = async () => {
    const  { data } = await axiosCron.get<Cron[]>("/cron-tab");
    return data;
}

axiosCron.interceptors.request.use(
  config => {
    console.log("request interceptor");
      if (!config.headers['Authorization']) {
          config.headers['Authorization'] = `Bearer ${store.getState().auth.token}`;
      }
      return config;
  }, (error) => Promise.reject(error)
);

axiosCron.interceptors.response.use(function (response) {
  return response;
}, function (error) {
  if (401 === error.response.status) {
    let token = store.getState().auth.token;
    if(token){
      console.log("token exists");
    let currentDate = new Date();
    const decodedToken: { exp: number } = jwt_decode(token!);
    if (decodedToken.exp * 1000 < currentDate.getTime()) {
      store.dispatch(refreshUser());
    }
  }
  } else {
      return Promise.reject(error);
  }
});

//https://alimozdemir.medium.com/asp-net-core-jwt-and-refresh-token-with-httponly-cookies-b1b96c849742
//Lets try it
//https://github.com/gitdagray/react_jwt_auth/blob/main/src/hooks/useAxiosPrivate.js

// axiosCron.interceptors.request.use(
//     async (config) => {
      
//         console.log();
//     //   const user = store?.getState()?.userData?.user;
  
//     //   let currentDate = new Date();
//     //   if (user?.accessToken) {
//     //     const decodedToken: { exp: number } = jwt_decode(user?.accessToken);
//     //     if (decodedToken.exp * 1000 < currentDate.getTime()) {
//     //       await store.dispatch(refreshToken());
//     //       if (config?.headers) {
//     //         config.headers["authorization"] = `Bearer ${
//     //           store?.getState()?.userData?.user?.accessToken
//     //         }`;
//     //       }
//     //     }
//     //   }
//       return config;
//     },
//     (error) => {
//       return Promise.reject(error);
//     }
//   );


