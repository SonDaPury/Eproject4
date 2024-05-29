import Home from "../pages/user/Home";
import Courses from "../pages/user/Courses";
import About from "../pages/user/About/";
import Category from "@eproject4/pages/user/Courses/Category";
import ListCourses from "@eproject4/pages/user/Courses/ListCourses";
import CourseDetail from "@eproject4/pages/user/Courses/CourseDetail";
import AdminLayout from "@eproject4/components/layout/AdminLayout";
import Login from "@eproject4/pages/auth/Login";
import Register from "@eproject4/pages/auth/Register";
import Topic from "@eproject4/pages/admin/Topic/Topic.jsx";
import Dashboard from "@eproject4/pages/admin/Dashboard";
import SubTopic from "@eproject4/pages/admin/SubTopic";
import UserDashboard from "@eproject4/pages/user/Dashboard";

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
export const privateRoutes = [
  { path: "/dashboard-student", component: UserDashboard },
];

// Route chi danh cho admin
export const adminRoutes = [
  {
    path: "/admin/danh-muc",
    component: Topic,
    layout: AdminLayout,
    name: "Danh mục",
  },
  {
    path: "/admin/danh-muc-con",
    component: SubTopic,
    layout: AdminLayout,
    name: "Danh mục con",
  },
  {
    path: "/admin/dashboard",
    component: Dashboard,
    layout: AdminLayout,
    name: "Dashboard",
  },
];
