import { configureStore } from "@reduxjs/toolkit";
import { exampleSlice } from "./slices/exampleSlice.js";
import userReducer from "./slices/userSlice.js";
import topicReducer from "./slices/topicSlice.js";
import subTopicsReducer from "./slices/subTopicSlice.js";

export const store = configureStore({
  reducer: {
    example: exampleSlice.reducer,
    user: userReducer,
    topics: topicReducer,
    subTopics: subTopicsReducer,
  },
});
