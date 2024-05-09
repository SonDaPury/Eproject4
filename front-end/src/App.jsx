import { publicRoutes, privateRoutes } from "./routes";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import { DefaultLayout } from "./components/layout";
import { Fragment } from "react";
import NotFound from "./components/NotFound";

function App() {
  return (
    <Router>
      <>
        <div>
          <Routes>
            {publicRoutes.map((route, index) => {
              const Component = route.component;
              let Layout = DefaultLayout;

              if (route.layout) {
                Layout = route.layout;
              } else if (route.layout === null) {
                Layout = Fragment;
              }

              return (
                <Route
                  key={index}
                  path={route.path}
                  element={
                    <Layout>
                      <Component />
                    </Layout>
                  }
                />
              );
            })}
            <Route
              path="*"
              element={
                <DefaultLayout>
                  <NotFound />
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
