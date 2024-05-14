import { configureStore } from "@reduxjs/toolkit";
import { exampleSlice } from "./slices/exampleSlice.js";
import userReducer from "./slices/userSlice.js";

export const store = configureStore({
  reducer: {
    example: exampleSlice.reducer,
    user: userReducer,
  },
});
