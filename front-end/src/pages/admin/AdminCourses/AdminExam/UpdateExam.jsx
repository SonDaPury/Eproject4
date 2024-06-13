import {
  Box,
  Button,
  Container,
  FormControl,
  FormControlLabel,
  FormLabel,
  IconButton,
  Modal,
  Radio,
  RadioGroup,
  TextField,
  Typography,
} from "@mui/material";
import {
  Controller,
  FormProvider,
  useFieldArray,
  useForm,
} from "react-hook-form";
import { useEffect } from "react";
import DeleteIcon from "@mui/icons-material/Delete";

const defaultQuestion = {
  text: "",
  options: ["", "", "", ""],
};

function UpdateExam({
  openUpdateExamModal,
  handleUpdateExamModalClose,
  lesson,
  fetchDataExamDetail,
  examDetail,
}) {
  const methods = useForm({
    defaultValues: {
      title: "",
      duration: "30",
      questions: [],
      correctOption: "",
    },
  });
  const {
    handleSubmit,
    control,
    reset,
    formState: { errors },
  } = methods;
  const { fields, append, remove } = useFieldArray({
    control,
    name: "questions",
  });

  useEffect(() => {
    if (lesson?.id && openUpdateExamModal) {
      fetchDataExamDetail(lesson.id);
    }
  }, [lesson, openUpdateExamModal]);

  useEffect(() => {
    if (examDetail) {
      let correctOption = "";
      const questions = examDetail?.exam?.questions?.map((question) => ({
        text: question?.questionText,
        options: question?.options?.map((option, index) => {
          if (option?.isCorrect) {
            correctOption = index;
          }
          return option.answer;
        }) || ["", "", "", ""],
        correctOption: correctOption || "",
      }));

      reset({
        title: examDetail?.exam?.title || "",
        duration: examDetail?.exam?.timeLimit || "30",
        questions: questions,
      });
    }
  }, [examDetail, reset]);

  const onSubmit = async (data) => {
    console.log(data);
  };

  return (
    <Box>
      <Modal
        open={openUpdateExamModal}
        onClose={handleUpdateExamModalClose}
        aria-labelledby="modal-modal-title"
        aria-describedby="modal-modal-description">
        <Box
          sx={{
            position: "absolute",
            top: "50%",
            left: "50%",
            transform: "translate(-50%, -50%)",
            width: 650,
            height: 700,
            bgcolor: "background.paper",
            boxShadow: 24,
            p: 4,
            overflowY: "auto",
          }}>
          <FormProvider {...methods}>
            <Typography variant="h6" align="center" gutterBottom>
              Cập nhật Bài Kiểm Tra
            </Typography>
            <Container maxWidth="md">
              <Controller
                name="title"
                control={control}
                rules={{ required: "Tiêu đề không được bỏ trống" }}
                render={({ field }) => (
                  <TextField
                    {...field}
                    label="Tiêu đề"
                    variant="outlined"
                    fullWidth
                    margin="normal"
                    error={!!methods.formState.errors.title}
                    helperText={methods.formState.errors.title?.message}
                  />
                )}
              />

              <Controller
                name="duration"
                control={control}
                rules={{
                  required: "Thời gian làm bài không được bỏ trống",
                  pattern: {
                    value: /^[1-9]\d*$/,
                    message: "Thời gian làm bài phải là số nguyên dương",
                  },
                }}
                render={({ field }) => (
                  <TextField
                    {...field}
                    label="Thời gian làm bài (phút)"
                    variant="outlined"
                    fullWidth
                    margin="normal"
                    type="number"
                    error={!!methods.formState.errors.duration}
                    helperText={methods.formState.errors.duration?.message}
                  />
                )}
              />
            </Container>
            <Container maxWidth="md">
              <Box component="form" onSubmit={handleSubmit(onSubmit)} mt={3}>
                {fields.map((question, index) => (
                  <Box
                    key={index}
                    mb={3}
                    p={2}
                    border={1}
                    borderRadius={2}
                    borderColor="grey.300">
                    <Box display="flex" justifyContent="space-between">
                      <Typography variant="h6">{`Câu ${index + 1}`}</Typography>
                      <IconButton onClick={() => remove(index)}>
                        <DeleteIcon />
                      </IconButton>
                    </Box>
                    <Controller
                      name={`questions.${index}.text`}
                      control={control}
                      rules={{ required: "Câu hỏi không được bỏ trống" }}
                      render={({ field }) => (
                        <TextField
                          {...field}
                          label="Nội dung câu hỏi"
                          variant="outlined"
                          fullWidth
                          margin="normal"
                          error={!!errors?.questions?.[index]?.text}
                          helperText={errors?.questions?.[index]?.text?.message}
                        />
                      )}
                    />
                    <FormControl
                      component="fieldset"
                      margin="normal"
                      sx={{ width: "95%" }}>
                      <FormLabel component="legend">Các lựa chọn</FormLabel>
                      <RadioGroup>
                        {question.options.map((option, optIndex) => (
                          <Box
                            key={optIndex}
                            display="flex"
                            alignItems="center"
                            width="100%"
                            justifyContent="space-between">
                            <Controller
                              name={`questions.${index}.options.${optIndex}`}
                              control={control}
                              rules={{
                                required: "Lựa chọn không được bỏ trống",
                              }}
                              render={({ field }) => (
                                <TextField
                                  {...field}
                                  label={`Lựa chọn ${optIndex + 1}`}
                                  sx={{ marginRight: "15px" }}
                                  variant="outlined"
                                  fullWidth
                                  margin="normal"
                                  error={
                                    !!errors?.questions?.[index]?.options?.[
                                      optIndex
                                    ]
                                  }
                                  helperText={
                                    errors?.questions?.[index]?.options?.[
                                      optIndex
                                    ]?.message
                                  }
                                />
                              )}
                            />
                            <Controller
                              name={`questions.${index}.correctOption`}
                              control={control}
                              render={({ field }) => (
                                <FormControlLabel
                                  control={
                                    <Radio
                                      {...field}
                                      value={optIndex}
                                      checked={field.value == optIndex}
                                    />
                                  }
                                  label="Đúng"
                                />
                              )}
                            />
                          </Box>
                        ))}
                      </RadioGroup>
                    </FormControl>
                  </Box>
                ))}
                <Button
                  variant="contained"
                  sx={{ color: "#FFF" }}
                  onClick={() => append({ ...defaultQuestion })}
                  fullWidth>
                  Thêm Câu Hỏi
                </Button>
                <Box mt={2}>
                  <Button
                    type="submit"
                    variant="contained"
                    color="secondary"
                    fullWidth>
                    Lưu Bài Kiểm Tra
                  </Button>
                </Box>
              </Box>
            </Container>
          </FormProvider>
        </Box>
      </Modal>
    </Box>
  );
}

export default UpdateExam;
