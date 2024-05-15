import Home from "../pages/user/Home";
import Courses from "../pages/user/Courses";
import About from "../pages/user/About/";
import Category from "@eproject4/pages/user/Courses/category";
import ListCategory from "@eproject4/pages/user/Courses/listcategory";


// Route khong can login van xem duoc
export const publicRoutes = [
  { path: "/", component: Home },
  { path: "/khoa-hoc", component: Courses },
  { path: "/ve-chung-toi", component: About },
  { path: "/category/:topicName", component: Category },
  { path: "/category-list/:topicName", component: ListCategory },
];

// Route can login moi xem duoc  
export const privateRoutes = [];
