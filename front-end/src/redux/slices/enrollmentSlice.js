// enrollmentSlice.js
import { createSlice } from "@reduxjs/toolkit";

const initialState = {
  enrolledCourses: {},
};

export const enrollmentSlice = createSlice({
  name: "enrollment",
  initialState,
  reducers: {
    setEnrollmentStatus: (state, action) => {
      const { courseId, isEnrolled } = action.payload;
      state.enrolledCourses[courseId] = isEnrolled;
    },
    addEnrollment: (state, action) => {
      const { userId, courseId } = action.payload;
      if (!state.enrolledCourses[courseId]) {
        state.enrolledCourses[courseId] = true;
      }
    },
  },
});

export const { setEnrollmentStatus, addEnrollment } = enrollmentSlice.actions;
