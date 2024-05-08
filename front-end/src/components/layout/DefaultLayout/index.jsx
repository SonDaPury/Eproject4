import Footer from "./Footer";
import Header from "./Header";
import Box from "@mui/material/Box";
import ExampleRedux from "../../ExampleRedux.jsx";

function DefaultLayout({ children }) {
  return (
    <div>
      <Header />
      {/*<ExampleRedux />*/}
      <Box>
        <Box>{children}</Box>
      </Box>
      {/* <Footer /> */}
    </div>
  );
}

export default DefaultLayout;
