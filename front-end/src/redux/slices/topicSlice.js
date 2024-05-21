import { createSlice } from "@reduxjs/toolkit";

const initialState = {
  topics: [],
  topicDetail: {},
};

const topicSlice = createSlice({
  name: "topic",
  initialState,
  reducers: {
    setTopics: (state, action) => {
      state.topics = [...action.payload];
    },
    deleteTopicReducer: (state, action) => {
      state.topics = state.topics.filter((item) => {
        return item.id !== action.payload;
      });
    },
  },
});

export const { setTopics, deleteTopicReducer } = topicSlice.actions;
export default topicSlice.reducer;
