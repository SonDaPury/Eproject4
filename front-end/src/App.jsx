import { publicRoutes, privateRoutes, adminRoutes } from "./routes";
import {
  BrowserRouter as Router,
  Route,
  Routes,
  Navigate,
} from "react-router-dom";
import { DefaultLayout } from "./components/layout";
import { Fragment } from "react";
import NotFound from "./components/NotFound";
import { getToken, getUser } from "./helpers/authHelper";
import useCustomSnackbar from "@eproject4/utils/hooks/useCustomSnackbar.js";

function App() {
  const token = getToken();
  const roleId = getUser()?.roleId;
  const { showSnackbar } = useCustomSnackbar();
  return (
    <Router>
      <>
        <div>
          <Routes>
            {publicRoutes.map((route, index) => {
              const Component = route.component;
              let Layout = route.layout || DefaultLayout;
              if (route.layout === null) {
                Layout = Fragment;
              }
              return (
                <Route
                  key={index}
                  path={route.path}
                  element={
                    <Layout>
                      <Component/>
                    </Layout>
                  }
                />
              );
            })}
            {privateRoutes.map((route, index) => {
              const Component = route.component;
              let Layout = route.layout || DefaultLayout;
              if (route.layout === null) {
                Layout = Fragment;
              }
              return (
                <Route
                  key={index}
                  path={route.path}
                  element={
                    token ? (
                      <Layout>
                        <Component/>
                      </Layout>
                    ) : (
                      <Navigate to="/dang-nhap"/>
                    )
                  }
                />
              );
            })}
            {adminRoutes.map((route, index) => {
              const Component = route.component;
              let Layout = route.layout || DefaultLayout;
              if (route.layout === null) {
                Layout = Fragment;
              }
              return (
                <Route
                  key={index}
                  path={route.path}
                  element={
                    token && roleId === 1 ? (
                      <Layout>
                        <Component/>
                      </Layout>
                    ) : (
                      <>
                        {showSnackbar("Bạn không có quyền truy cập", "error")}
                        <Navigate to="/dang-nhap"/>
                      </>
                    )
                  }
                />
              );
            })}
            <Route
              path="*"
              element={
                <DefaultLayout>
                  <NotFound/>
                </DefaultLayout>
              }
            />
          </Routes>
        </div>
      </>
    </Router>
  );
}

export default App;
