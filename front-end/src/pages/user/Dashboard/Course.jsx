import React from "react";
import {
  Box,
  FormControl,
  Grid,
  InputAdornment,
  InputLabel,
  MenuItem,
  Select,
  TextField,
  Typography,
} from "@mui/material";
import HorizontalCourseCard from "@eproject4/components/StDashboard/HorizontalCourseCard";
import PaginationTemp from "@eproject4/components/PaginationTemp";
import SearchIcon from "@mui/icons-material/Search";

export default function Course() {
  const courses = [
    {
      title: "Kevin Lee's UI/UX Master/Teacher Program",
      description: "1. Introductions",
      image: "https://bom.so/vV4j7x",
      progress: 0,
    },
    {
      title: "The Complete 2023 Web Development Bootcamp",
      description: "12%. What You’ll Need to Get Started - Setup",
      image: "https://via.placeholder.com/140",
      progress: 81,
    },
    {
      title: "Copywriting - Become a Freelance Copywriter",
      description: "1. How to get started with Figma",
      image: "https://via.placeholder.com/140",
      progress: 0,
    },
    {
      title: "2021 Complete Python Bootcamp From Zero to Hero",
      description: "8. Advanced CSS - Selector Priority",
      image: "https://via.placeholder.com/140",
      progress: 12,
    },
  ];
  const [sort, setSort] = React.useState("");
  const [status, setStatus] = React.useState("");
  const Length = courses.length;

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
          Courses ({Length})
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
            sx={{ flexGrow: 1 ,minWidth: 256 }}
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
          {courses.map((course, index) => (
            <Grid
              item
              xs={12}
              sm={6}
              md={3}
              key={index}
              sx={{ padding: "16px 5px  !important " }}>
              <HorizontalCourseCard
                key={course.id}
                title={course.title}
                subtitle={course.description}
                image={course.imageUrl}
                progress={course.progress}
              />
            </Grid>
          ))}
        </Grid>
      </>
      <PaginationTemp />
    </Box>
  );
}
