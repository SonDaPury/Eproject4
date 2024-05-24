import useAxiosWithLoading from "@eproject4/utils/hooks/useAxiosWithLoading";

// Get topics
export const getTopics = () => {
  const { callApi } = useAxiosWithLoading();

  const getTopicsAction = async () => {
    try {
      const res = await callApi("/Topic", "get", null, null, false);
      return res;
    } catch (err) {
      throw new Error(err);
    }
  };
  return { getTopicsAction };
};

// Create topic
export const createTopic = () => {
  const { callApi } = useAxiosWithLoading();

  const createTopicAction = async (data) => {
    try {
      const res = await callApi(
        "/Topic",
        "post",
        data,
        "Tạo danh mục thành công",
        true
      );
      return res;
    } catch (err) {
      throw new Error(err);
    }
  };
  return { createTopicAction };
};

// delete topic
export const deleteTopic = () => {
  const { callApi } = useAxiosWithLoading();

  const deleteTopicAction = async (id) => {
    const res = await callApi(
      `/Topic/${id}`,
      "delete",
      null,
      "Xóa danh mục thành công",
      true
    );

    return res;
  };
  return { deleteTopicAction };
};

// update topics
export const updateTopic = () => {
  const { callApi } = useAxiosWithLoading();

  const updateTopicAction = async (dataUpdate) => {
    try {
      const res = await callApi(
        `/Topic/${dataUpdate?.id}`,
        "put",
        dataUpdate,
        "Cập nhật danh mục thành công"
      );
    } catch (err) {
      throw new Error(err);
    }
  };
  return { updateTopicAction };
};
