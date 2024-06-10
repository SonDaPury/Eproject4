import { getCourseById } from "@eproject4/services/courses.service";
import { Button, Rating, Typography } from "@mui/material";
import Box from "@mui/material/Box";
import { useEffect, useState } from "react";
import { Link, useSearchParams } from "react-router-dom";
import Avatar from "@mui/material/Avatar";
import AccountCircleIcon from "@mui/icons-material/AccountCircle";
import { getUser } from "@eproject4/helpers/authHelper";
import { getUserById } from "@eproject4/services/user.service";
import { Card, CardContent, Grid, LinearProgress } from "@mui/material";
import PlayCircleIcon from "@mui/icons-material/PlayCircle";
import GroupIcon from "@mui/icons-material/Group";
import SubtitlesIcon from "@mui/icons-material/Subtitles";
import AccessTimeIcon from "@mui/icons-material/AccessTime";
import ChatIcon from "@mui/icons-material/Chat";
import VisibilityIcon from "@mui/icons-material/Visibility";
import StarIcon from "@mui/icons-material/Star";

const dataColumn1 = [
  {
    label: "Bài học",
    value: "1,957",
    icon: <PlayCircleIcon sx={{ color: "#FF6636" }} />,
  },
  {
    label: "Học viên",
    value: "9,419,418",
    icon: <GroupIcon sx={{ color: "#E34444" }} />,
  },
  {
    label: "Ngôn ngữ",
    value: "Vietnamese",
    icon: <SubtitlesIcon sx={{ color: "#1D2026" }} />,
  },
  {
    label: "Số giờ",
    value: "19:37:51",
    icon: <AccessTimeIcon sx={{ color: "#564FFD" }} />,
  },
];

const dataColumn2 = [
  {
    label: "Bài kiểm tra",
    value: "51,429",
    icon: <ChatIcon sx={{ color: "#564FFD" }} />,
  },
  {
    label: "Lượt xem",
    value: "76,395,167",
    icon: <VisibilityIcon sx={{ color: "#1D2026" }} />,
  },
];

const ratings = [
  { label: "5", value: 67 },
  { label: "4", value: 27 },
  { label: "3", value: 5 },
  { label: "2", value: 1 },
  { label: "1", value: 0.1 },
];

function AdminDetailCourse() {
  const user = getUser();
  const { getUserByIdAction } = getUserById();
  const { getCourseByIdAction } = getCourseById();
  const [dataCourses, setDataCourses] = useState({ userId: 0 });
  const [searchParams] = useSearchParams();
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

    fetchCoursesData();
  }, []);

  useEffect(() => {
    const fetchUserByIdData = async () => {
      try {
        const res = await getUserByIdAction(dataCourses?.userId);
        setAuthor(res?.data?.username);
      } catch (err) {
        throw new Error(err);
      }
    };
    fetchUserByIdData();
  }, [dataCourses]);

  return (
    <Box sx={{ paddingBottom: "15px" }}>
      <Box
        sx={{
          backgroundColor: "white",
          maxWidth: { lg: "1000px", xl: "1200px", sm: "800px" },
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
      <Box
        sx={{
          maxWidth: { lg: "1000px", xl: "1200px", sm: "800px" },
          marginX: "auto",
        }}>
        <Box sx={{ flexGrow: 1, paddingY: 4 }}>
          <Grid container spacing={3}>
            <Grid item xs={12} lg={8}>
              <Grid container spacing={3}>
                {/* Cột 1 */}
                <Grid item xs={12} lg={6}>
                  {dataColumn1.map((item, index) => (
                    <Card key={index} sx={{ mb: 2 }}>
                      <CardContent>
                        <Box display="flex" alignItems="center">
                          <Box>{item.icon}</Box>
                          <Box ml={3}>
                            <Typography
                              sx={{ fontSize: "24px", fontWeight: 400 }}
                              variant="h5">
                              {item.value}
                            </Typography>
                            <Typography
                              color="textSecondary"
                              sx={{ fontSize: "14px", fontWeight: 400 }}>
                              {item.label}
                            </Typography>
                          </Box>
                        </Box>
                      </CardContent>
                    </Card>
                  ))}
                </Grid>

                {/* Cột 2 */}
                <Grid item xs={12} lg={6}>
                  {dataColumn2.map((item, index) => (
                    <Card key={index} sx={{ mb: 2 }}>
                      <CardContent>
                        <Box display="flex" alignItems="center">
                          <Box sx={{ color: item.color }}>{item.icon}</Box>
                          <Box ml={3}>
                            <Typography variant="h5">{item.value}</Typography>
                            <Typography color="textSecondary">
                              {item.label}
                            </Typography>
                          </Box>
                        </Box>
                      </CardContent>
                    </Card>
                  ))}
                </Grid>
              </Grid>
            </Grid>

            {/* Cột 3 */}
            <Grid item xs={12} lg={4}>
              <Card>
                <CardContent>
                  <Typography variant="h6">Overall Course Rating</Typography>
                  <Box mt={2}>
                    {ratings.map((rating, index) => (
                      <Box
                        key={index}
                        display="flex"
                        alignItems="center"
                        mb={1}>
                        <Box
                          width="10%"
                          sx={{ display: "flex", alignItems: "center" }}>
                          <Typography sx={{ marginRight: "5px" }}>
                            {rating.label}
                          </Typography>{" "}
                          <StarIcon
                            sx={{ fontSize: "15px", color: "#FD8E1F" }}
                          />
                        </Box>
                        <Box width="80%" mx={2}>
                          <LinearProgress
                            variant="determinate"
                            value={rating.value}
                          />
                        </Box>
                        <Box width="10%">
                          <Typography>{rating.value}%</Typography>
                        </Box>
                      </Box>
                    ))}
                  </Box>
                </CardContent>
              </Card>
            </Grid>
          </Grid>
        </Box>
      </Box>
      <Box
        sx={{
          maxWidth: { lg: "1000px", xl: "1200px", sm: "800px" },
          marginX: "auto",
        }}>
        <Link
          to={`/admin/khoa-hoc/${dataCourses.slug}/bai-hoc?id-course=${dataCourses.id}`}>
          <Button
            sx={{ color: "white", borderRadius: 0, boxShadow: "none" }}
            variant="contained">
            Xem bài học
          </Button>
        </Link>
      </Box>
    </Box>
  );
}

export default AdminDetailCourse;
