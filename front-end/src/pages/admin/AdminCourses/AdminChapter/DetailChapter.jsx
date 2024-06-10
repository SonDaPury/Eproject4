import { useSortable } from "@dnd-kit/sortable";
import {
  Accordion,
  AccordionDetails,
  AccordionSummary,
  Box,
  IconButton,
  Typography,
} from "@mui/material";
import { makeStyles } from "@mui/styles";
import ArrowDropDownIcon from "@mui/icons-material/ArrowDropDown";
import DeleteOutlineIcon from "@mui/icons-material/DeleteOutline";
import BorderColorIcon from "@mui/icons-material/BorderColor";
import DragIndicatorIcon from "@mui/icons-material/DragIndicator";
import { CSS } from "@dnd-kit/utilities";

const useStyles = makeStyles((theme) => ({
  content: {
    justifyContent: "space-between",
  },
}));

function DetailChapter({
  id,
  chapter,
  index,
  handleUpdateTitleAndDescription,
  handleDeleteChapter,
}) {
  const { attributes, listeners, setNodeRef, transform, transition } =
    useSortable({ id });

  const style = {
    transform: CSS.Transform.toString(transform),
    transition,
    marginBottom: "15px",
  };

  const classes = useStyles();
  return (
    <Box ref={setNodeRef} style={style} {...attributes}>
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
                {...listeners}
                onMouseDown={(e) => e.stopPropagation()}>
                <DragIndicatorIcon />
              </IconButton>
              <IconButton
                onClick={(e) => {
                  e.stopPropagation();
                  handleUpdateTitleAndDescription(chapter, e);
                }}>
                <BorderColorIcon />
              </IconButton>
              <IconButton
                onClick={(e) => {
                  e.stopPropagation();
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
      </Box>
    </Box>
  );
}

export default DetailChapter;
