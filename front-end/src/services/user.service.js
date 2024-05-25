import useAxiosWithLoading from "@eproject4/utils/hooks/useAxiosWithLoading";

// Get user by id
export const getUserById = () => {
  const { callApi } = useAxiosWithLoading();

  const getUserByIdAction = async (id) => {
    try {
      const res = await callApi(`/User/${id}`, "get", null, null, false);
      return res;
    } catch (err) {
      throw new Error(err);
    }
  };

  return { getUserByIdAction };
};
