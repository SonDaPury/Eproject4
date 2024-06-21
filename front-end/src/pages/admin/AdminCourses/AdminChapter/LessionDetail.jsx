/* eslint-disable no-lonely-if */
// LessionDetail.jsx
import { Box, Button, IconButton, Typography } from "@mui/material";
import FormatListBulletedIcon from "@mui/icons-material/FormatListBulleted";
import DeleteOutlineIcon from "@mui/icons-material/DeleteOutline";
import {
  deleteLesson,
  updateLesson,
} from "@eproject4/services/lession.service";
import { useEffect, useState } from "react";
import DriveFileRenameOutlineIcon from "@mui/icons-material/DriveFileRenameOutline";
import { deleteExam, getExamById } from "@eproject4/services/exam.service";
import BorderColorIcon from "@mui/icons-material/BorderColor";
import UpdateExam from "../AdminExam/UpdateExam";
import useCustomSnackbar from "@eproject4/utils/hooks/useCustomSnackbar";
import { closestCenter, DndContext } from "@dnd-kit/core";
import {
  arrayMove,
  SortableContext,
  useSortable,
  verticalListSortingStrategy,
} from "@dnd-kit/sortable";
import DragIndicatorIcon from "@mui/icons-material/DragIndicator";
import { CSS } from "@dnd-kit/utilities";

function LessionDetail({
  fetchDataAllLessonsOfChapter,
  chapter,
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
  const { showSnackbar } = useCustomSnackbar();
  const [isDragged, setIsDragged] = useState(false);
  const { updateLessonAction } = updateLesson();

  const handleUpdateExamModalClose = () => {
    setOpenUpdateExamModal(false);
    setSelectedExamId(null);
  };
  const { getExamByIdAction } = getExamById();
  const [examDetail, setExamDetail] = useState({});

  const handleDeleteLession = async (id, timeLimit, lession) => {
    if (timeLimit) {
      await deleteExamAction(id);
      fetchDataAllLessonsOfChapter();
    } else {
      if (!lession?.examID) {
        await deleteLessonAction(Number(id));
        fetchDataAllLessonsOfChapter();
        return;
      } else {
        showSnackbar(
          "Phải xóa bài kiểm tra bên dưới trước khi xóa bài học này",
          "error"
        );
        return;
      }
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

  const handleDragEnd = (e) => {
    const { active, over } = e;
    if (over) {
      if (active.id !== over.id) {
        const data = (items) => {
          const oldIndex = items.findIndex((item) => item.id === active.id);
          const newIndex = items.findIndex((item) => item.id === over.id);
          const newItems = arrayMove(items, oldIndex, newIndex);

          let index = 1;
          newItems.forEach((item) => {
            if (!item?.timeLimit) {
              item.index = index;
              index++;
            }
          });

          return newItems;
        };

        setMergeList(data(mergeList));
      }
    }
  };

  const handleDragStart = () => {
    setIsDragged(true);
  };

  const handleCancelUpdateOrderLesson = () => {
    const mergeLessons = mergeLessonsAndExams(lessionsOfChapter, exams);
    setMergeList(mergeLessons);
    setIsDragged(false);
  };

  const handleUpdateOrderLesson = async () => {
    try {
      await Promise.all(
        mergeList.map(async (item) => {
          console.log(item);
          const dataLessonUpdate = {
            id: item?.id,
            title: item?.title,
            author: item?.author,
            description: item?.description,
            videoDuration: Number(item?.videoDuration),
            view: Number(item?.view),
            status: true,
            chapterId: chapter?.id,
            serialDto: {
              index: item?.index,
              exam_ID: item?.examID,
            },
          };
          // await await updateLessonAction({})
        })
      );
    } catch (err) {
      throw new Error(err);
    }
  };

  return (
    <Box>
      <DndContext
        onDragStart={handleDragStart}
        collisionDetection={closestCenter}
        onDragEnd={handleDragEnd}>
        <SortableContext
          items={mergeList.map((lession) => {
            return lession?.id;
          })}
          strategy={verticalListSortingStrategy}>
          {mergeList?.map((lession, index) => {
            return (
              <Box key={lession?.id}>
                <DetailLessonSection
                  id={lession?.id}
                  lession={lession}
                  openUpdateExamModal={openUpdateExamModal}
                  handleUpdateExamModalClose={handleUpdateExamModalClose}
                  fetchDataExamDetail={fetchDataExamDetail}
                  examDetail={examDetail}
                  fetchDataAllLessonsOfChapter={fetchDataAllLessonsOfChapter}
                  handleUpdateExamAndLesson={handleUpdateExamAndLesson}
                  handleDeleteLession={handleDeleteLession}
                />
              </Box>
            );
          })}
        </SortableContext>
      </DndContext>
      {isDragged && (
        <>
          <Button
            onClick={handleCancelUpdateOrderLesson}
            sx={{
              borderRadius: 0,
              boxShadow: "none",
              backgroundColor: "#FFEEE8",
              marginTop: "20px",
              color: "#FF6636",
              "&:hover": {
                backgroundColor: "#FFEEE8",
              },
              fontSize: "16px",
              padding: "10px 24px",
              width: "100%",
            }}>
            Hủy
          </Button>
          <Button
            onClick={handleUpdateOrderLesson}
            sx={{
              borderRadius: 0,
              boxShadow: "none",
              backgroundColor: "#FFEEE8",
              marginTop: "20px",
              color: "#FF6636",
              "&:hover": {
                backgroundColor: "#FFEEE8",
              },
              fontSize: "16px",
              padding: "10px 24px",
              width: "100%",
            }}>
            Lưu
          </Button>
        </>
      )}
    </Box>
  );
}

export default LessionDetail;

const DetailLessonSection = ({
  id,
  lession,
  openUpdateExamModal,
  handleUpdateExamModalClose,
  fetchDataExamDetail,
  examDetail,
  fetchDataAllLessonsOfChapter,
  handleUpdateExamAndLesson,
  handleDeleteLession,
}) => {
  const { attributes, listeners, setNodeRef, transform, transition } =
    useSortable({ id });

  const style = {
    transform: CSS.Transform.toString(transform),
    transition,
    marginBottom: "15px",
  };

  return (
    <Box
      ref={setNodeRef}
      {...attributes}
      style={style}
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
            <IconButton {...listeners} onMouseDown={(e) => e.stopPropagation()}>
              <DragIndicatorIcon />
            </IconButton>
            <DriveFileRenameOutlineIcon sx={{ marginRight: "15px" }} />
            <UpdateExam
              openUpdateExamModal={openUpdateExamModal}
              handleUpdateExamModalClose={handleUpdateExamModalClose}
              lesson={lession}
              fetchDataExamDetail={fetchDataExamDetail}
              examDetail={examDetail}
              fetchDataAllLessonsOfChapter={fetchDataAllLessonsOfChapter}
            />
          </>
        ) : (
          <>
            <IconButton {...listeners} onMouseDown={(e) => e.stopPropagation()}>
              <DragIndicatorIcon />
            </IconButton>
            <FormatListBulletedIcon sx={{ marginRight: "15px" }} />
          </>
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
            handleDeleteLession(lession?.id, lession?.timeLimit, lession);
          }}>
          <DeleteOutlineIcon />
        </IconButton>
      </Box>
    </Box>
  );
};
