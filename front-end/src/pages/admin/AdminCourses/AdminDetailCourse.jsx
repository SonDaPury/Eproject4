import { getCourseById } from "@eproject4/services/courses.service";
import { Rating, Typography } from "@mui/material";
import Box from "@mui/material/Box";
import { useEffect, useState } from "react";
import { useSearchParams } from "react-router-dom";
import Avatar from "@mui/material/Avatar";
import AccountCircleIcon from "@mui/icons-material/AccountCircle";
import { getUser } from "@eproject4/helpers/authHelper";
import { getUserById } from "@eproject4/services/user.service";
import { Card, CardContent, Grid, LinearProgress } from "@mui/material";
import StarIcon from "@mui/icons-material/Star";

const data = [
  { label: "Bài học", value: "1,957", icon: <StarIcon /> },
  { label: "Bài kiểm tra", value: "51,429", icon: <StarIcon /> },
  { label: "Học viên", value: "9,419,418", icon: <StarIcon /> },
  { label: "Lượt xem", value: "76,395,167", icon: <StarIcon /> },
  { label: "Ngôn ngữ", value: "Vietnamese", icon: <StarIcon /> },
  { label: "Số giờ", value: "19:37:51", icon: <StarIcon /> },
];

const ratings = [
  { label: "5 Star", value: 67 },
  { label: "4 Star", value: 27 },
  { label: "3 Star", value: 5 },
  { label: "2 Star", value: 1 },
  { label: "1 Star", value: 0.1 },
];

function AdminDetailCourse() {
  const user = getUser();
  const { getUserByIdAction } = getUserById();
  const { getCourseByIdAction } = getCourseById();
  const [dataCourses, setDataCourses] = useState([]);
  const [searchParams, setSearchParams] = useSearchParams();
  const [author, setAuthor] = useState("");

  const idQuery = searchParams.get("id");

  useEffect(() => {
    const fetchCoursesData = async () => {
      try {
        const res = await getCourseByIdAction(idQuery);
        setDataCourses(res?.data);
      } catch (err) {
        throw new Error(err);
      }
    };

    const fetchUserByIdData = async () => {
      try {
        const res = await getUserByIdAction(dataCourses?.userId);
        setAuthor(res?.data?.username);
      } catch (err) {
        throw new Error(err);
      }
    };

    fetchCoursesData();
    fetchUserByIdData();
  }, []);

  return (
    <Box>
      <Box
        sx={{
          backgroundColor: "white",
          maxWidth: { lg: "1000px", xl: "1320px", sm: "800px" },
          marginX: "auto",
          display: {
            xs: "block",
            sm: "block",
            md: "flex",
            lg: "flex",
            xl: "flex",
          },
          padding: "24px",
        }}>
        <Box>
          <img
            src={dataCourses?.thumbnail}
            alt="Error"
            className="w-[300px] h-[200px] xl:w-[352px] xl:h-[230px]"
          />
        </Box>
        <Box
          sx={{
            marginLeft: {
              xs: "0",
              md: "24px",
              lg: "24px",
            },
            width: "calc(100% - 392px)",
          }}>
          <Typography
            sx={{ fontSize: { md: "18px", lg: "24px" }, fontWeight: 600 }}>
            {dataCourses?.title}
          </Typography>
          <Box
            sx={{
              marginTop: "30px",
              display: "flex",
              alignItems: "center",
              justifyContent: "space-between",
            }}>
            <Box sx={{ display: "flex", alignItems: "center" }}>
              <Avatar alt={<AccountCircleIcon />} src={user?.avatar} />
              <Box sx={{ marginLeft: "15px" }}>
                <Typography
                  sx={{ fontSize: "14px", fontWeight: 400, color: "#6E7485" }}>
                  Tạo bởi:
                </Typography>
                <Typography sx={{ fontSize: "16px", fontWeight: 500 }}>
                  {author}
                </Typography>
              </Box>
            </Box>
            <Box>
              <Rating
                name="read-only"
                value={dataCourses?.rating ?? 0}
                readOnly
              />
            </Box>
          </Box>
          <Box
            sx={{ marginTop: "50px", display: "flex", alignItems: "center" }}>
            <Box>
              <Typography sx={{ fontSize: { md: "17px", lg: "20px" } }}>
                {dataCourses?.price > 0
                  ? `${dataCourses?.price} Đ`
                  : "Miễn phí"}
              </Typography>
              <Typography
                sx={{
                  color: "#6E7485",
                  fontSize: "14px",
                  fontWeight: 400,
                  marginTop: "10px",
                }}>
                Giá khóa học
              </Typography>
            </Box>
            <Box sx={{ marginLeft: "50px" }}>
              <Typography sx={{ fontSize: { md: "17px", lg: "20px" } }}>
                10000000 Đ
              </Typography>
              <Typography
                sx={{
                  color: "#6E7485",
                  fontSize: "14px",
                  fontWeight: 400,
                  marginTop: "10px",
                }}>
                Tổng doanh thu
              </Typography>
            </Box>
          </Box>
        </Box>
      </Box>
      <Box>
        <Box>
          <Box>
            <div></div>
          </Box>
          <Box></Box>
        </Box>
        <Box></Box>
      </Box>
    </Box>
  );
}

export default AdminDetailCourse;
