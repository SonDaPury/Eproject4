import useAxiosWithLoading from "@eproject4/utils/hooks/useAxiosWithLoading";

export const searchHome = () => {
  const { callApi } = useAxiosWithLoading();
  const searchDebounceAction = async (keyword) => {
    try {
      const body = {
        query: keyword,
      };
      const res = await callApi(
        `/Search/searchfulltext`,
        "post",
        body,
        null,
        false
      );
      return res;
    } catch (err) {
      throw new Error(err);
    }
  };
  return { searchDebounceAction };
};
