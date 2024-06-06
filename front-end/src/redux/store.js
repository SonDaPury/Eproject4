import { configureStore } from "@reduxjs/toolkit";
import { exampleSlice } from "./slices/exampleSlice.js";
import userReducer from "./slices/userSlice.js";
import topicReducer from "./slices/topicSlice.js";
import subTopicsReducer from "./slices/subTopicSlice.js";
import { favoritesSlice } from "./slices/favoriteSlice.js";

export const store = configureStore({
  reducer: {
    favorites: favoritesSlice.reducer,
    example: exampleSlice.reducer,
    user: userReducer,
    topics: topicReducer,
    subTopics: subTopicsReducer,
  },
});
