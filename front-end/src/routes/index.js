import Home from "../pages/user/Home";
import Courses from "../pages/user/Courses";
import About from "../pages/user/About/";
import Category from "@eproject4/pages/user/Courses/Category";
import ListCoures from "@eproject4/pages/user/Courses/ListCourses";
import CourseDetail from "@eproject4/pages/user/Courses/CourseDetail";

// Route khong can login van xem duoc
export const publicRoutes = [
  { path: "/", component: Home },
  { path: "/khoa-hoc", component: Courses },
  { path: "/ve-chung-toi", component: About },
  { path: "/category/:topicName", component: Category },
  { path: "/course-list/:topicName", component: ListCoures },
  { path: "/course-detail/:category/:title", component: CourseDetail },
];

// Route can login moi xem duoc
export const privateRoutes = [];
