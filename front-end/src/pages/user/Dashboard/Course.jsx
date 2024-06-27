import React, { useEffect, useState } from "react";
import {
  Box,
  FormControl,
  Grid,
  InputAdornment,
  InputLabel,
  MenuItem,
  Pagination,
  Select,
  TextField,
  Typography,
} from "@mui/material";
import HorizontalCourseCard from "@eproject4/components/StDashboard/HorizontalCourseCard";
import PaginationTemp from "@eproject4/components/PaginationTemp";
import SearchIcon from "@mui/icons-material/Search";

import { getAllOrdersPag } from "@eproject4/services/order.service";

// import { getAllOrders } from "@eproject4/services/order.service";

export default function Course() {
  const [courseData, setCourseData] = useState([]);
  const [pageIndex, setPageIndex] = useState(1);
  const [pageSize] = useState(4);
  

  // const cleanDescription = DOMPurify.sanitize(courseData.description);
  function removeTags(description) {
    if (typeof description !== "string") {
      return ""; // Trả về chuỗi rỗng nếu description không phải là chuỗi
    }
    const parser = new DOMParser();
    const doc = parser.parseFromString(description, "text/html");
    return doc.body.textContent || "";
  }

  // Ví dụ sử dụng với dữ liệu của bạn
  // const cleanedDescription = removeTags(courseData.description);
  const { getAllOrdersPagAction } = getAllOrdersPag();

  useEffect(() => {
    const fetchAllOrderPageData = async () => {
      try {
        const res = await getAllOrdersPagAction(pageIndex, pageSize);
        console.log("All Orders:", res);
        if (res.status === 200) {
          setCourseData(res?.data);
        } else {
          console.error("Failed to fetch initial orders:", res);
        }
      } catch (err) {
        console.error("Error fetching all orders:", err);
      }
    };
    fetchAllOrderPageData();
  }, [pageIndex, pageSize]);
  const handleChangePagination = (e, newPage) => {
    setPageIndex(newPage);
  };

  const [sort, setSort] = React.useState("");
  const [status, setStatus] = React.useState("");
  // const Length = courses.length;

  const handleSortChange = (event) => {
    setSort(event.target.value);
  };

  const handleStatusChange = (event) => {
    setStatus(event.target.value);
  };
  return (
    <Box>
      <>
        <Typography variant="h6" sx={{ marginBottom: 2 }}>
          Courses ({courseData.length})
        </Typography>

        <Box
          sx={{
            display: "flex",
            alignItems: "center",
            gap: 1,
            marginBottom: "30px",
            maxWidth: "70%",
          }}>
          <TextField
            fullWidth
            size="small"
            placeholder="Tìm kiếm khóa học của bạn..."
            InputProps={{
              startAdornment: (
                <InputAdornment position="start">
                  <SearchIcon />
                </InputAdornment>
              ),
            }}
            sx={{ flexGrow: 1, minWidth: 256 }}
          />
          <FormControl size="small" sx={{ minWidth: 206 }}>
            <InputLabel id="sort-label">Sắp xếp theo</InputLabel>
            <Select
              labelId="sort-label"
              value={sort}
              label="Sắp xếp theo"
              onChange={handleSortChange}>
              <MenuItem value="latest">Latest</MenuItem>
              <MenuItem value="oldest">Oldest</MenuItem>
            </Select>
          </FormControl>
          <FormControl size="small" sx={{ minWidth: 206 }}>
            <InputLabel id="status-label">Trạng thái</InputLabel>
            <Select
              labelId="status-label"
              value={status}
              label="Trạng thái"
              onChange={handleStatusChange}>
              <MenuItem value="all">All Courses</MenuItem>
              <MenuItem value="active">Active</MenuItem>
              <MenuItem value="completed">Completed</MenuItem>
            </Select>
          </FormControl>
        </Box>

        <Grid
          container
          spacing={2}
          sx={{ width: "calc(100% + 27px)", marginLeft: "-13px" }}>
          {courseData.map((course, index) => {
            const cleanedDescriptions = removeTags(course.description); // Mảng các mô tả đã được làm sạch
            return (
              <Grid
                item
                xs={12}
                sm={6}
                md={3}
                key={index}
                sx={{ padding: "16px 5px  !important " }}>
                <HorizontalCourseCard
                  key={course.souresID}
                  title={course.sourceTitle}
                  subtitle={cleanedDescriptions} // Nối các mô tả thành chuỗi
                  image={course.thumbbnail}
                  progress={course.progress}
                />
              </Grid>
            );
          })}
        </Grid>
      </>
      <Box
        sx={{
          display: "flex",
          justifyContent: "center", // Căn giữa Pagination
          marginTop: "25px",
        }}>
        <Pagination
          count={Math.ceil(Number(courseData?.totalCount) / Number(pageSize))}
          onChange={handleChangePagination}
          page={pageIndex}
        />
      </Box>
    </Box>
  );
}
