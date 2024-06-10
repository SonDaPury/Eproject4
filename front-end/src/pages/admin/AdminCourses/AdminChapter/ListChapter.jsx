import {
  Accordion,
  AccordionDetails,
  AccordionSummary,
  Box,
  IconButton,
  Typography,
} from "@mui/material";
import ArrowDropDownIcon from "@mui/icons-material/ArrowDropDown";
import DeleteOutlineIcon from "@mui/icons-material/DeleteOutline";
import { makeStyles } from "@mui/styles";
import {
  deleteChapter,
  updateChapter,
} from "@eproject4/services/chapter.service";
import { useSearchParams } from "react-router-dom";
import BorderColorIcon from "@mui/icons-material/BorderColor";
import UpdateChapter from "./UpdateChapter";
import { useState } from "react";

const useStyles = makeStyles((theme) => ({
  content: {
    justifyContent: "space-between",
  },
}));

function ListChapter({ listChapters, getChapterOfCourse }) {
  const classes = useStyles();
  let sortedChapter = listChapters?.sort((a, b) => a?.index - b?.index);
  const { updateChapterAction } = updateChapter();
  const [searchParams] = useSearchParams();
  const idCourse = searchParams.get("id-course");
  const { deleteChapterAction } = deleteChapter();
  const [openUpdateChapterModal, setOpenUpdateChapterModal] = useState(false);
  const [currentChapter, setCurrentChapter] = useState(null);
  const handleUpdateChapterModalOpen = (chapter) => {
    setOpenUpdateChapterModal(true);
    setCurrentChapter(chapter);
  };
  const handleUpdateChapterModalClose = () => setOpenUpdateChapterModal(false);

  const updateIndexesOnDelete = (index) => {
    const newListChapters = sortedChapter?.filter(
      (chapter) => chapter.index !== index
    );

    newListChapters?.forEach(async (chapter) => {
      if (chapter.index > index) {
        chapter.index--;
      }
    });

    sortedChapter = newListChapters.sort((a, b) => a?.index - b?.index);
  };

  const handleDeleteChapter = async (index, e, id) => {
    e.stopPropagation();
    updateIndexesOnDelete(index);
    await deleteChapterAction(id);
    await updateChapterAction(sortedChapter);
    getChapterOfCourse(idCourse);
  };

  const handleUpdateTitleAndDescription = (chapter, e) => {
    e.stopPropagation();
    handleUpdateChapterModalOpen(chapter);
  };

  return (
    <Box sx={{ marginTop: "30px" }}>
      {listChapters?.map((chapter, index) => {
        return (
          <Box key={index} sx={{ marginBottom: "15px" }}>
            <Accordion sx={{ boxShadow: "none", backgroundColor: "#F5F7FA" }}>
              <AccordionSummary
                expandIcon={<ArrowDropDownIcon />}
                aria-controls="panel2-content"
                id="panel2-header"
                sx={{
                  display: "flex",
                }}
                classes={{
                  content: classes.content,
                }}>
                <Box sx={{ display: "flex", alignItems: "center" }}>
                  <Typography
                    sx={{
                      fontSize: "16px",
                      fontWeight: 500,
                      marginRight: "10px",
                    }}>
                    {chapter?.title}:
                  </Typography>
                  <Typography
                    sx={{ fontSize: "16px", fontWeight: 400 }}
                    component="span">
                    {chapter?.description}
                  </Typography>
                </Box>
                <Box sx={{ display: "flex", alignItems: "center" }}>
                  <IconButton
                    onClick={(e) => {
                      handleUpdateTitleAndDescription(chapter, e);
                    }}>
                    <BorderColorIcon />
                  </IconButton>
                  <IconButton
                    onClick={(e) => {
                      handleDeleteChapter(chapter?.index, e, chapter?.id);
                    }}>
                    <DeleteOutlineIcon />
                  </IconButton>
                </Box>
              </AccordionSummary>
              <AccordionDetails>
                <Typography>
                  Lorem ipsum dolor sit amet, consectetur adipiscing elit.
                  Suspendisse malesuada lacus ex, sit amet blandit leo lobortis
                  eget.
                </Typography>
              </AccordionDetails>
            </Accordion>
            <UpdateChapter
              openUpdateChapterModal={openUpdateChapterModal}
              handleUpdateChapterModalClose={handleUpdateChapterModalClose}
              getChapterOfCourse={getChapterOfCourse}
              chapter={currentChapter}
            />
          </Box>
        );
      })}
    </Box>
  );
}

export default ListChapter;
