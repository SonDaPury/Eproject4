import { Box, IconButton, Typography } from "@mui/material";
import FormatListBulletedIcon from "@mui/icons-material/FormatListBulleted";
import DeleteOutlineIcon from "@mui/icons-material/DeleteOutline";
import { deleteLesson } from "@eproject4/services/lession.service";

function LessionDetail({ lessionsOfChapter, fetchDataAllLessonsOfChapter }) {
  const { deleteLessonAction } = deleteLesson();

  const handleDeleteLession = async (id) => {
    await deleteLessonAction(Number(id));
    await fetchDataAllLessonsOfChapter();
  };

  return (
    <Box>
      <Box>
        {lessionsOfChapter?.map((lession, index) => {
          return (
            <Box key={index}>
              {lession?.lesson.map((item, index) => {
                return (
                  <Box
                    sx={{
                      padding: "12px 20px",
                      backgroundColor: "#FFF",
                      marginBottom: "15px",
                      display: "flex",
                      justifyContent: "space-between",
                    }}
                    key={index}>
                    <Box sx={{ display: "flex", alignItems: "center" }}>
                      <FormatListBulletedIcon sx={{ marginRight: "15px" }} />
                      <Typography>{item?.title}</Typography>
                    </Box>
                    <Box>
                      <IconButton
                        onClick={() => {
                          handleDeleteLession(item?.id);
                        }}>
                        <DeleteOutlineIcon />
                      </IconButton>
                    </Box>
                  </Box>
                );
              })}
            </Box>
          );
        })}
      </Box>
    </Box>
  );
}

export default LessionDetail;
