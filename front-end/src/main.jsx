import CssBaseline from "@mui/material/CssBaseline";
import React from "react";
import ReactDOM from "react-dom/client";
import { store } from "./redux/store.js";
import { Provider } from "react-redux";

import App from "./App.jsx";
import "./index.css";

ReactDOM.createRoot(document.getElementById("root")).render(
  <React.StrictMode>
    <Provider store={store}>
      <CssBaseline />
      <App />
    </Provider>
  </React.StrictMode>
);
