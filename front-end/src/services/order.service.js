import useAxiosWithLoading from "@eproject4/utils/hooks/useAxiosWithLoading";

export const getOrderforFree = () => {
  const { callApi } = useAxiosWithLoading();

  const getOrderforFreeAction = async (userID, sourceID) => {
    try {
      const payload = {
        userID: userID,
        sourceID: sourceID,
      };

      const res = await callApi(
        "/Order/insert-source-free",
        "post",
        payload,
        null,
        false
      );
      return res;
    } catch (err) {
      throw new Error(err);
    }
  };

  return { getOrderforFreeAction };
};
