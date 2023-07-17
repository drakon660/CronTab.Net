import { configureStore, ThunkAction, Action } from '@reduxjs/toolkit';
import authReducer from "./features/auth/authSlice";
import cronReducer from "./features/auth/cronSlice";

export const store = configureStore({
  reducer: {
    auth : authReducer,
    crontab : cronReducer
  },
});

export type AppDispatch = typeof store.dispatch;
export type RootState = ReturnType<typeof store.getState>;
export type AppThunk<ReturnType = void> = ThunkAction<
  ReturnType,
  RootState,
  unknown,
  Action<string>
>;
