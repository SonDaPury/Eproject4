import Home from "../pages/user/Home";
import Courses from "../pages/user/Courses";

// Route khong can login van xem duoc
export const publicRoutes = [
  { path: "/", component: Home },
  { path: "/courses", component: Courses },
];

// Route can login moi xem duoc
export const privateRoutes = [];
