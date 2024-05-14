import Home from "../pages/user/Home";
import Courses from "../pages/user/Courses";
import About from "../pages/user/About/";
import Category from "@eproject4/pages/user/Courses/Category";
import ListCourses from "@eproject4/pages/user/Courses/ListCourses";
import CourseDetail from "@eproject4/pages/user/Courses/CourseDetail";

import Login from "@eproject4/pages/auth/Login";
import Register from "@eproject4/pages/auth/Register";
import Topic from "@eproject4/pages/admin/Topic.jsx";

// Route khong can login van xem duoc
export const publicRoutes = [
  { path: "/", component: Home },
  { path: "/khoa-hoc", component: Courses },
  { path: "/ve-chung-toi", component: About },
  { path: "/category/:topicName", component: Category },
  { path: "/course-list/:topicName", component: ListCourses },
  { path: "/course-detail/:category/:title", component: CourseDetail },
  { path: "/dang-nhap", component: Login },
  { path: "/dang-ky", component: Register },
];

// Route can login moi xem duoc
export const privateRoutes = [];

// Route chi danh cho admin
export const adminRoutes = [
  {
    path: "/admin/topic", component: Topic
  }
];
