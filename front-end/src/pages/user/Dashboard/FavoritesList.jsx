import React, { useState } from "react";
import {
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Button,
  IconButton,
  Typography,
  Box,
  Avatar,
} from "@mui/material";
import FavoriteBorderIcon from "@mui/icons-material/FavoriteBorder";
import FavoriteIcon from "@mui/icons-material/Favorite";
import seedData from "@eproject4/utils/seedData";
import StarIcon from "@mui/icons-material/Star";

const Allcourses = seedData();
const couresFive = Allcourses.slice(0, 5);
const length = couresFive.length;

export default function FavoritesList() {
  const [favorited, setFavorited] = useState(false);
  const handleToggleFavorite = () => {
    setFavorited(); // Chuyển đổi trạng thái khi nhấn nút
  };
  return (
    <Box>
      <Typography variant="h6" sx={{ marginBottom: 2 }}>
        Danh sách yêu thích ({length})
      </Typography>
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow
              sx={{ color: "#4E5566", fontSize: "14px", fontWeight: 500 }}>
              <TableCell>Khóa học</TableCell>
              <TableCell align="right">Giá</TableCell>
              <TableCell align="center">Hành động</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {couresFive.map((course, index) => (
              <TableRow key={index}>
                <TableCell
                  component="th"
                  scope="row"
                  sx={{ display: "flex", alignItems: "center" }}>
                  <Avatar
                    src={course.imageThumbnail}
                    variant="square"
                    sx={{ width: 160, height: 120, marginRight: 2 }}
                  />
                  <div>
                    <Box
                      sx={{ display: "flex", alignItems: "center" }}
                      gap="3px">
                      <StarIcon
                        sx={{ color: "#FD8E1F", width: "16px", height: "16px" }}
                      />{" "}
                      <Typography variant="body2" color="primary">
                        {course.rating} ({course.views.toLocaleString()} Review)
                      </Typography>
                    </Box>

                    <Typography variant="h6">{course.title}</Typography>
                    <Typography
                      variant="body2"
                      color="text.secondary"
                      sx={{ marginTop: "25px" }}>
                      Đăng bởi: {course.topic}
                    </Typography>
                  </div>
                </TableCell>
                <TableCell align="right">
                  <Typography
                    variant="h6"
                    color="text.primary"
                    sx={{ color: "#FF6636" }}>
                    ${course.price}
                  </Typography>
                  {/* {course.originalPrice && (
                    <Typography
                      variant="body2"
                      sx={{
                        textDecoration: "line-through",
                        color: "text.secondary",
                      }}>
                      ${course.originalPrice}
                    </Typography>
                  )} */}
                </TableCell>
                <TableCell align="center">
                  <Button
                    variant="contained"
                    sx={{
                      marginRight: 1,
                      width: "176px",
                      backgroundColor: "#F5F7FA",
                      color: "#1D2026",
                      fontSize: "16px",
                      fontWeight: 600,
                      height: "48px",
                      "&:hover": {
                        backgroundColor: "#FF6636", // Thay đổi màu nền khi hover
                        color: "#FFF", // Thay đổi màu chữ khi hover
                      },
                    }}>
                    Buy Now
                  </Button>
                  <Button
                    sx={{
                      backgroundColor: "#FF6636", // Thay đổi màu nền khi hover
                      color: "#FFF",
                      "&:hover": {
                        backgroundColor: "#FFEEE8",
                        color: "#FF6636",
                      },
                      height: "48px",
                    }}
                    variant="outlined">
                    Add To Cart
                  </Button>
                  <Button
                    onClick={handleToggleFavorite}
                    color="error"
                    sx={{
                      marginLeft: 1,
                      backgroundColor: "#FFEEE8",
                      width: "48px",
                      height: "48px",
                    }}>
                    {favorited ? <FavoriteIcon /> : <FavoriteBorderIcon />}
                  </Button>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </Box>
  );
}
