import { Box, Typography } from "@mui/material";
import * as Yup from "yup";

const validationSchema = Yup.object().shape({
  title: Yup.string().required("Title is required"),
  description: Yup.string().required("Description is required"),
  video: Yup.mixed().required("Video is required"),
});

function CreateLession() {
  const chunkSize = 1048576 * 3;

  return (
    <Box>
      <Typography sx={{ fontSize: "20px", fontWeight: 500 }}>
        Tạo bài giảng
      </Typography>
      <Box></Box>
    </Box>
  );
}

export default CreateLession;
