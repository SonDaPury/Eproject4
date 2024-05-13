import axios from "axios";

const request = axios.create({
  baseURL: "http://localhost:5187/api",
});

export default request;
