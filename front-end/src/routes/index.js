import Home from "../pages/user/Home";
import Courses from "../pages/user/Courses";
import About from "../pages/user/About/";
import Category from "@eproject4/pages/user/Courses/category";

// Route khong can login van xem duoc
export const publicRoutes = [
  { path: "/", component: Home },
  { path: "/khoa-hoc", component: Courses },
  { path: "/ve-chung-toi", component: About },
  { path: "/category/:topicName", component: Category },
];

// Route can login moi xem duoc  
export const privateRoutes = [];
