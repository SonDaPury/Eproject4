import { Box } from "@mui/material";
import { useEffect, useState } from "react";
import { deleteTopic, getTopics } from "@eproject4/services/topic.service";
import { useDispatch, useSelector } from "react-redux";
import { topicsSelector } from "@eproject4/redux/selectors";
import {
  setTopics,
  deleteTopicReducer,
} from "@eproject4/redux/slices/topicSlice";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import Paper from "@mui/material/Paper";
import TablePagination from "@mui/material/TablePagination";
import Button from "@mui/material/Button";
import CreateTopic from "./CreateTopic";
import UpdateTopic from "./UpdateTopic";

function createData(id, nameTopic) {
  return { id, nameTopic };
}

function Topic() {
  const data = useSelector(topicsSelector);
  const dispatch = useDispatch();
  const { getTopicsAction } = getTopics();
  const { deleteTopicAction } = deleteTopic();
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(5);
  const [openModal, setOpenModal] = useState(false);
  const [openUpdateModal, setOpenUpdateModal] = useState(false);
  const [selectedTopic, setSelectedTopic] = useState(null);

  const rows = data.topics.map((item) => {
    return createData(item?.id, item?.topicName);
  });

  const fetchDataTopics = async () => {
    const res = await getTopicsAction();
    dispatch(setTopics(res?.data));
  };

  useEffect(() => {
    fetchDataTopics();
  }, []);

  const handleChangePage = (event, newPage) => {
    setPage(newPage);
  };

  const handleChangeRowsPerPage = (event) => {
    setRowsPerPage(parseInt(event.target.value, 10));
    setPage(0);
  };

  const handleOpenModal = () => {
    setOpenModal(true);
  };

  const handleCloseModal = () => {
    setOpenModal(false);
  };

  const handleDeleteTopic = async (id) => {
    await deleteTopicAction(id);
    dispatch(deleteTopicReducer(id));
  };

  const handleOpenUpdateModal = (topic) => {
    setSelectedTopic(topic);
    setOpenUpdateModal(true);
  };

  const handleCloseUpdateModal = () => {
    setOpenUpdateModal(false);
    setSelectedTopic(null);
  };

  return (
    <Box
      sx={{
        width: "80%",
        marginX: "auto",
        backgroundColor: "#FFF",
        height: "auto",
        padding: "20px",
      }}>
      <Box>
        <Button
          variant="contained"
          onClick={handleOpenModal}
          sx={{
            color: "white",
            marginBottom: "50px",
          }}>
          Thêm danh mục
        </Button>
        <TableContainer component={Paper}>
          <Table sx={{ minWidth: 650 }} aria-label="simple table">
            <TableHead>
              <TableRow>
                <TableCell>Id</TableCell>
                <TableCell>Tên danh mục</TableCell>
                <TableCell>Hành động</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {rows
                .slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
                .map((row, index) => {
                  return (
                    <TableRow
                      key={index}
                      sx={{
                        "&:last-child td, &:last-child th": { border: 0 },
                      }}>
                      <TableCell component="th" scope="row">
                        {row.id}
                      </TableCell>
                      <TableCell>{row.nameTopic}</TableCell>
                      <TableCell>
                        <Button
                          variant="outlined"
                          sx={{ marginRight: "10px" }}
                          onClick={() => handleOpenUpdateModal(row)}>
                          Cập nhật
                        </Button>
                        <Button
                          variant="outlined"
                          onClick={() => {
                            handleDeleteTopic(row.id);
                          }}>
                          Xóa
                        </Button>
                      </TableCell>
                    </TableRow>
                  );
                })}
            </TableBody>
          </Table>
        </TableContainer>
        <TablePagination
          rowsPerPageOptions={[5]}
          component="div"
          count={rows.length}
          rowsPerPage={rowsPerPage}
          page={page}
          onPageChange={handleChangePage}
          onRowsPerPageChange={handleChangeRowsPerPage}
        />
      </Box>
      <CreateTopic
        open={openModal}
        handleClose={handleCloseModal}
        existingTopics={data.topics}
        onTopicAdded={fetchDataTopics}
      />
      {selectedTopic && (
        <UpdateTopic
          open={openUpdateModal}
          handleClose={handleCloseUpdateModal}
          existingTopics={data.topics}
          topic={selectedTopic}
          onTopicUpdated={fetchDataTopics} // Truyền callback để cập nhật danh sách
        />
      )}
    </Box>
  );
}

export default Topic;
