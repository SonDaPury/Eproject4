import request from "@eproject4/utils/request";

export const getToken = () => {
  return localStorage.getItem("token");
};

export const setToken = (token) => {
  localStorage.setItem("token", token);
};

export const register = async ({
  username = "admin2",
  password = "12345678",
  email = "admin2@gmail.com",
  phoneNumber = "0912342132",
  avatar = "string",
}) => {
  console.log(username);
  if (username && password && email && phoneNumber && avatar) {
    const response = await request.post("/User/register", {
      username,
      password,
      email,
      phoneNumber,
      avatar,
    });
    console.log(response);

    return response;
  }
};
