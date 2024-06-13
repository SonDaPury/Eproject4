import { Box, Typography } from "@mui/material";

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
