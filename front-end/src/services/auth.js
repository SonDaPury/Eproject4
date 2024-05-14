import useAxiosWithLoading from "@eproject4/utils/hooks/useAxiosWithLoading";
import {
  setToken,
  setRefreshToken,
  setUser,
} from "@eproject4/helpers/authHelper";
import { useLocation, useNavigate } from "react-router-dom";
import { useDispatch } from "react-redux";
import { setUserInfo } from "@eproject4/redux/slices/userSlice";

// Register
export const registerForm = () => {
  const { callApi, loading, error, success, data } = useAxiosWithLoading();

  const registerFormAction = async (dataForm) => {
    if (
      dataForm?.username &&
      dataForm?.password &&
      dataForm?.email &&
      dataForm?.phoneNumber
    ) {
      const formData = new FormData();

      formData.append("username", dataForm.username);
      formData.append("password", dataForm.password);
      formData.append("email", dataForm.email);
      formData.append("phoneNumber", dataForm.phoneNumber);
      if (dataForm.avatar.length > 0) {
        formData.append("avatar", dataForm.avatar);
      }

      await callApi("/User/register", "post", formData, "Đăng ký thành công");
    } else {
      throw new Error("Trường dữ liệu không hợp lệ");
    }
  };
  return { registerFormAction, loading, error, success, data };
};

// Login
export const loginForm = () => {
  const { callApi, loading, error, success, data } = useAxiosWithLoading();
  const navigate = useNavigate();
  const location = useLocation();
  const dispatch = useDispatch();

  const from = location.state?.from?.pathname || "/";

  const loginFormAction = async (dataForm) => {
    if (dataForm?.username && dataForm?.password) {
      try {
        const body = {
          username: dataForm?.username,
          password: dataForm?.password,
        };

        const res = await callApi("/User/login", "post", body, "Đăng nhập thành công");
        console.log(res);

        const { accessToken, refreshToken } = res.data.item1;
        const user = res?.data?.item2;
        setUser(user);
        setToken(accessToken);
        setRefreshToken(refreshToken);
        dispatch(setUserInfo({ user, accessToken, refreshToken }));
        navigate(from, { replace: true });
      } catch (err) {
        console.log(err);
        throw new Error(err);
      }
    }
  };

  return { loginFormAction, loading, error, success, data };
};
