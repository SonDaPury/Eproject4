import { configureStore } from "@reduxjs/toolkit";
import { exampleSlice } from "./slices/exampleSlice.js";

export const store = configureStore({
  reducer: {
    example: exampleSlice.reducer,
  },
});
