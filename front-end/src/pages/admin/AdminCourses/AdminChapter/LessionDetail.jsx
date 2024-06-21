// LessionDetail.jsx
import { Box, IconButton, Typography } from "@mui/material";
import FormatListBulletedIcon from "@mui/icons-material/FormatListBulleted";
import DeleteOutlineIcon from "@mui/icons-material/DeleteOutline";
import { deleteLesson } from "@eproject4/services/lession.service";
import { useEffect, useState } from "react";
import DriveFileRenameOutlineIcon from "@mui/icons-material/DriveFileRenameOutline";
import { deleteExam, getExamById } from "@eproject4/services/exam.service";
import BorderColorIcon from "@mui/icons-material/BorderColor";
import UpdateExam from "../AdminExam/UpdateExam";

function LessionDetail({
  fetchDataAllLessonsOfChapter,
  lessionsOfChapter,
  exams,
}) {
  const { deleteLessonAction } = deleteLesson();
  const [mergeList, setMergeList] = useState([]);
  const { deleteExamAction } = deleteExam();
  const [openUpdateExamModal, setOpenUpdateExamModal] = useState(false);
  const [selectedExamId, setSelectedExamId] = useState(null);
  const handleUpdateExamModalOpen = (id) => {
    setSelectedExamId(id);
    setOpenUpdateExamModal(true);
  };

  const handleUpdateExamModalClose = () => {
    setOpenUpdateExamModal(false);
    setSelectedExamId(null);
  };
  const { getExamByIdAction } = getExamById();
  const [examDetail, setExamDetail] = useState({});

  const handleDeleteLession = async (id, timeLimit) => {
    if (timeLimit) {
      await deleteExamAction(id);
      fetchDataAllLessonsOfChapter();
    } else {
    await deleteLessonAction(Number(id));
      fetchDataAllLessonsOfChapter();
    }
  };

  const fetchDataExamDetail = async (id) => {
    const res = await getExamByIdAction(id);
    setExamDetail(res?.data || null);
  };

  const mergeLessonsAndExams = (lessons, exams) => {
    const mergedList = [];
    lessons?.lessons?.forEach((lesson) => {
      lesson?.lesson?.forEach((item) => {
        mergedList.push(item);
        if (item?.examID !== null && item?.examID !== undefined) {
          const relatedExam = exams.find((exam) => exam.id === item.examID);

          if (relatedExam) {
            mergedList.push(relatedExam);
          }
        }
      });
    });
    return mergedList;
  };

  const handleUpdateExamAndLesson = (id, timeLimit) => {
    if (timeLimit) {
      handleUpdateExamModalOpen(id);
    } else {
      console.log("Update lesson");
    }
  };

  useEffect(() => {
    const mergeLessons = mergeLessonsAndExams(lessionsOfChapter, exams);
    setMergeList(mergeLessons);
  }, [lessionsOfChapter, exams]);

  useEffect(() => {
    if (selectedExamId !== null) {
      fetchDataExamDetail(selectedExamId);
    }
  }, [selectedExamId]);

  return (
    <Box>
      <Box>
        {mergeList?.map((lession, index) => (
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
              }}>
                    <Box sx={{ display: "flex", alignItems: "center" }}>
                {lession?.timeLimit ? (
                  <>
                    <DriveFileRenameOutlineIcon sx={{ marginRight: "15px" }} />
                    <UpdateExam
                      openUpdateExamModal={openUpdateExamModal}
                      handleUpdateExamModalClose={handleUpdateExamModalClose}
                      lesson={lession}
                      fetchDataExamDetail={fetchDataExamDetail}
                      examDetail={examDetail}
                    />
                  </>
                ) : (
                      <FormatListBulletedIcon sx={{ marginRight: "15px" }} />
                )}
                <Typography>{lession?.title}</Typography>
                    </Box>
                    <Box>
                      <IconButton
                        onClick={() => {
                    handleUpdateExamAndLesson(lession?.id, lession?.timeLimit);
                  }}>
                  <BorderColorIcon />
                </IconButton>
                <IconButton
                  onClick={() => {
                    handleDeleteLession(lession?.id, lession?.timeLimit);
                        }}>
                        <DeleteOutlineIcon />
                      </IconButton>
                    </Box>
                  </Box>
                );
              })}
            </Box>
        ))}
      </Box>
    </Box>
  );
}

export default LessionDetail;
