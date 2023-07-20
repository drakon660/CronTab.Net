import { PayloadAction, createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import *  as identityApi from "../../app/api/identityApi";
import { Cron, User } from "../../app/api/interfaces";

export const authUser = createAsyncThunk(
  'auth/token',
  async (user: User) => {
    const response = await identityApi.authUser(user);
    return response;
  }
)

export const refreshUser = createAsyncThunk(
  'auth/refresh-token',
  async () => {
    const response = await identityApi.refreshTokenUser();
    return response;
  }
)

interface AuthPayload {
  //user: string | null,
  token: string | null
}

interface AuthState {
  cron: Cron[],
  user: string | null,
  token: string | null
}

// Define the initial state using that type
const initialState: AuthState = {
  cron: [],
  user: null,
  token: null
}

const authSlice = createSlice({
  name: 'auth',
  initialState: initialState,
  reducers: {

  },
  extraReducers: (builder) => {
    builder.addCase(authUser.fulfilled, (state, action: PayloadAction<string>) => {
      state.token = action.payload;
    });
    builder.addCase(refreshUser.fulfilled, (state, action: PayloadAction<string>) => {
      state.token = action.payload;
    });
  }
  // reducers: {
  //   setCredentials: (state, action: PayloadAction<AuthPayload>) => {
  //     state.user = action.payload.user;
  //     state.token = action.payload.token;
  //   },
  //   logaout: (state, action: PayloadAction<AuthPayload>) => {
  //     state.user = null;
  //     state.token = null;
  //   }
  // }

});

//export const { setCredentials, logaout } = authSlice.actions;


export default authSlice.reducer;

export const selectCurrentUser = (state: AuthState) => state.user;
export const selectCurrentToken = (state: AuthState) => state.token;
