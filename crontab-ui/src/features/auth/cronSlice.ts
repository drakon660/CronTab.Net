import { PayloadAction, ThunkDispatch, createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import * as cronApi from "../../app/api/cronApi";
import { RootState } from "../../store";
import { Cron } from "../../app/api/interfaces";

export const getCron = createAsyncThunk<Cron[], undefined, { state : RootState}>(
    'crontab/all',
    async (_, thunkApi) => {
      const auth =  thunkApi.getState().auth;
      console.log(auth.token);
      //const stateItem = state.getState();
      //console.log(stateItem.auth.token);
      const response = await cronApi.getCron(auth.token!);
      return response;
    }
  )

interface CrontabState {
    cron: Cron[]
}

const initialState: CrontabState = {
    cron: []
};

const cronSlice = createSlice({
    name: "cron",
    initialState: initialState,
    reducers: {

    },
    extraReducers: (builder) => {

        builder.addCase(getCron.fulfilled, (state, action: PayloadAction<Cron[]>) => {
            state.cron = action.payload;
        })
    }

});


export default cronSlice.reducer;

export const selectCron = (state: RootState) => state.crontab.cron;