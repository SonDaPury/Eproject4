import useAxiosWithLoading from "@eproject4/utils/hooks/useAxiosWithLoading";
import axios from "axios";

export const createQuestionExam = () => {
  const createQuestionExamAction = async (data) => {
    const resdata = data?.questions?.map(async (question) => {
      try {
        if (
          question?.text &&
          question?.options &&
          question?.correctOption !== undefined
        ) {
          const completeOption = question?.options?.map((option, index) => {
            return {
              Answer: option,
              IsCorrect: index == question?.correctOption,
            };
          });

          const dataForm = new FormData();
          dataForm.append("Question", question.text);
          dataForm.append("Options", JSON.stringify(completeOption));

          const res = await axios.post(
            "http://localhost:5187/api/Exam/createquestion",
            dataForm,
            {
              headers: {
                "Content-Type": "multipart/form-data",
              },
            }
          );
          return res?.data;
        }
      } catch (err) {
        throw new Error(err);
      }
    });

    return Promise.all(resdata);
  };

  return { createQuestionExamAction };
};

// Create Exam
export const createExam = () => {
  const { callApi } = useAxiosWithLoading();

  const createExamAction = async (data) => {
    try {
      const res = await callApi(
        "/Exam",
        "post",
        data,
        "Tạo bài kiểm tra thành công",
        true
      );
      return res;
    } catch (err) {
      throw new Error(err);
    }
  };

  return { createExamAction };
};

// Connect exam with question
export const connectExamWithQuestion = () => {
  const { callApi } = useAxiosWithLoading();

  const connectExamWithQuestionAction = async (data) => {
    try {
      const res = await callApi(
        "/Exam/connectexamquestion",
        "post",
        data,
        null,
        null
      );
      return res;
    } catch (err) {
      throw new Error(err);
    }
  };

  return { connectExamWithQuestionAction };
};

// Get All Exam
export const getAllExam = () => {
  const { callApi } = useAxiosWithLoading();

  const getAllExamAction = async () => {
    try {
      const res = await callApi("/Exam", "get", null, null, null);
      return res;
    } catch (err) {
      throw new Error(err);
    }
  };

  return { getAllExamAction };
};

// Delete Exam
export const deleteExam = () => {
  const { callApi } = useAxiosWithLoading();

  const deleteExamAction = async (id) => {
    try {
      const res = await callApi(
        `/Exam/${id}`,
        "delete",
        null,
        "Xóa bài kiểm tra thành công",
        true
      );
      return res;
    } catch (err) {
      throw new Error(err);
    }
  };

  return { deleteExamAction };
};

// Get Exam By Id
export const getExamById = () => {
  const { callApi } = useAxiosWithLoading();

  const getExamByIdAction = async (id) => {
    try {
      const res = await callApi(`/Exam/detail/${id}`, "get", null, null, null);
      return res;
    } catch (err) {
      throw new Error(err);
    }
  };

  return { getExamByIdAction };
};
