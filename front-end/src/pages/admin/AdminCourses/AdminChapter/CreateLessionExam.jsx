import { Box, Modal } from "@mui/material";

function CreateLessionExam({ openCreateModal, handleClose }) {
  const styleModalCreate = {
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
  };
  return (
    <Box>
      <Modal
        aria-labelledby="modal-title"
        aria-describedby="modal-description"
        open={openCreateModal}
        onClose={handleClose}>
        <Box sx={styleModalCreate}></Box>
      </Modal>
    </Box>
  );
}

export default CreateLessionExam;
