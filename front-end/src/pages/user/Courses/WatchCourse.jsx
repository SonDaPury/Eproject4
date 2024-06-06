import ButtonCustomize from "@eproject4/components/ButtonCustomize";
import PropTypes from "prop-types";

import SourceDetail from "@eproject4/components/SourceDetail";
import { Title } from "@mui/icons-material";
import {
  Box,
  Button,
  CardMedia,
  Grid,
  LinearProgress,
  Tab,
  Tabs,
  Typography,
} from "@mui/material";
import React from "react";
import { useParams } from "react-router-dom";

function CustomTabPanel(props) {
  const { children, value, index, ...other } = props;

  return (
    <div
      role="tabpanel"
      hidden={value !== index}
      id={`simple-tabpanel-${index}`}
      aria-labelledby={`simple-tab-${index}`}
      {...other}>
      {value === index && (
        <Box
          sx={{
            padding: "24px 0px",
            textAlign: "justify",
          }}>
          <Typography
            sx={{ fontSize: "24px", fontWeight: 600, color: "#1D2026" }}>
            {children}
          </Typography>
        </Box>
      )}
    </div>
  );
}

CustomTabPanel.propTypes = {
  children: PropTypes.node,
  index: PropTypes.number.isRequired,
  value: PropTypes.number.isRequired,
};

function a11yProps(index) {
  return {
    id: `simple-tab-${index}`,
    "aria-controls": `simple-tabpanel-${index}`,
  };
}
const WatchCourse = () => {
  const { id, title } = useParams();

  const progress = 15;
  // Tabs
  const [value, setValue] = React.useState(0);

  const handleChange = (event, newValue) => {
    setValue(newValue);
  };
  // const cleanDescription = DOMPurify.sanitize(courseData.description);
  console.log(id, title);
  return (
    <>
      <Box>
        <Box
          sx={{
            display: "flex",
            padding: "20px 32px",
            justifyContent: "space-between",
            // alignItems: "center",
            backgroundColor: "#F5F7FA",
            height: "98px",
          }}>
          <Box sx={{ display: "flex" }}>
            <Button
              sx={{
                width: "40px",
                height: "40px",
                borderRadius: "50%",
                border: "1px solid #1D2026",
                display: "flex",
                justifyContent: "center",
                alignItems: "center",
                backgroundColor: "#FFFFFF",
                padding: 0,
                minWidth: 0,
              }}>
              <svg
                xmlns="http://www.w3.org/2000/svg"
                width="16.5px"
                height="13.5px"
                viewBox="0 0 18 16"
                fill="none">
                <path d="M17.25 8H0.75" stroke="#1D2026" />
                <path d="M7.5 1.25L0.75 8L7.5 14.75" stroke="#1D2026" />
              </svg>
            </Button>
            <Box sx={{ paddingLeft: "16px" }}>
              <Typography
                sx={{ fontSize: "18px", fontWeight: 500 }}
                component={"h2"}>
                {title}
              </Typography>
              <Box sx={{ display: "flex", marginBottom: "25px" }}>
                <Box sx={{ display: "flex", alignItems: "center" }}>
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    width="20"
                    height="20"
                    viewBox="0 0 20 20"
                    fill="none">
                    <path
                      d="M2.5 16.25V5C2.5 4.83424 2.56585 4.67527 2.68306 4.55806C2.80027 4.44085 2.95924 4.375 3.125 4.375H7.29167C7.4269 4.375 7.55848 4.41886 7.66667 4.5L9.83333 6.125C9.94152 6.20614 10.0731 6.25 10.2083 6.25H15.625C15.7908 6.25 15.9497 6.31585 16.0669 6.43306C16.1842 6.55027 16.25 6.70924 16.25 6.875V8.75"
                      stroke="#FF6636"
                    />
                    <path
                      d="M2.5 16.25L4.84285 10.3929C4.88924 10.2769 4.96932 10.1775 5.07275 10.1075C5.17619 10.0374 5.29823 10 5.42315 10H9.18576C9.30916 10 9.42979 9.96348 9.53245 9.89503L11.0925 8.85497C11.1952 8.78652 11.3158 8.75 11.4392 8.75H17.8829C17.9819 8.75 18.0795 8.77353 18.1677 8.81866C18.2558 8.86379 18.332 8.92922 18.3899 9.00956C18.4478 9.0899 18.4858 9.18284 18.5007 9.28074C18.5156 9.37864 18.5071 9.47869 18.4758 9.57264L16.25 16.25H2.5Z"
                      stroke="#FF6636"
                    />
                  </svg>
                  <Typography
                    sx={{
                      color: "#4E5566",
                      fontSize: "14px",
                      fontWeight: 400,
                      margin: "0px 5px",
                    }}
                    component="p">
                    Chương
                  </Typography>
                </Box>
                <Box sx={{ display: "flex", alignItems: "center" }}>
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    width="20"
                    height="20"
                    viewBox="0 0 20 20"
                    fill="none">
                    <path
                      d="M10 17.5C14.1421 17.5 17.5 14.1421 17.5 10C17.5 5.85786 14.1421 2.5 10 2.5C5.85786 2.5 2.5 5.85786 2.5 10C2.5 14.1421 5.85786 17.5 10 17.5Z"
                      stroke="#564FFD"
                    />
                    <path
                      d="M12.5 10L8.75 7.5V12.5L12.5 10Z"
                      stroke="#564FFD"
                    />
                  </svg>
                  <Typography
                    sx={{
                      color: "#4E5566",
                      fontSize: "14px",
                      fontWeight: 400,
                      margin: "0px 5px",
                    }}
                    component="p">
                    Bài Học
                  </Typography>
                </Box>
                <Box sx={{ display: "flex", alignItems: "center" }}>
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    width="20"
                    height="20"
                    viewBox="0 0 20 20"
                    fill="none">
                    <path
                      d="M10 17.5C14.1421 17.5 17.5 14.1421 17.5 10C17.5 5.85786 14.1421 2.5 10 2.5C5.85786 2.5 2.5 5.85786 2.5 10C2.5 14.1421 5.85786 17.5 10 17.5Z"
                      stroke="#FD8E1F"
                    />
                    <path d="M10 5.625V10H14.375" stroke="#FD8E1F" />
                  </svg>
                  <Typography
                    sx={{
                      color: "#4E5566",
                      fontSize: "14px",
                      fontWeight: 400,
                      margin: "0px 5px",
                    }}
                    component="p">
                    19h 37m
                  </Typography>
                </Box>
              </Box>
            </Box>
          </Box>

          <ButtonCustomize width="100px" height="48px" text="Bài kế tiếp" />
        </Box>

        {/* ádsadsadaaaaaaaaaaaaaaaaaaaaaaassssssssssssss */}

        <Box sx={{ padding: "36px 27px 0px 32px" }}>
          <Grid container spacing={2}>
            <Grid item xs={7}>
              <Box sx={{}}>
                <Box>
                  {" "}
                  <CardMedia
                    sx={{ width: "835px", height: "471px", objectFit: "cover" }}
                    component="video"
                    controls
                    //src={courseData.videoIntro} // Thay thế 'your-video-url.mp4' bằng URL của video bạn muốn hiển thị
                  >
                    Your browser does not support the video tag.
                  </CardMedia>
                  <Box>
                    <Typography
                      component={"h2"}
                      sx={{ fontSize: "25px", fontWeight: 600 , padding:"24px 0px" }}>
                      2. Sign up in Webflow
                    </Typography>
                    <hr />
                  </Box>
                  <Box sx={{ width: "100%" }}>
                    <Box sx={{ borderBottom: 1, borderColor: "divider" }}>
                      <Tabs
                        value={value}
                        onChange={handleChange}
                        aria-label="basic tabs example">
                        <Tab label="Mô Tả" {...a11yProps(0)} />
                        <Tab label=" Nội dung bài Học" {...a11yProps(1)} />
                      </Tabs>
                    </Box>
                    <CustomTabPanel value={value} index={0}>
                      Tổng quan
                      <Typography
                        component="div"
                        sx={{
                          textAlign: "justify",
                          color: "#4E5566",
                          fontSize: "14px",
                          fontWeight: 400,
                          paddingTop: "20px",
                        }}
                      />
                    </CustomTabPanel>

                    <CustomTabPanel value={value} index={1}></CustomTabPanel>
                  </Box>
                </Box>
              </Box>
            </Grid>
            <Grid item xs={5}>
              <Box>
                <Box sx={{ width: "100%", mb: 2 }}>
                  <Box
                    sx={{
                      display: "flex",
                      justifyContent: "space-between",
                      alignItems: "center",
                      mb: 1,
                    }}>
                    <Typography
                      variant="h6"
                      sx={{ fontSize: "24px", fontWeight: 600 }}>
                      Nội dung khóa học
                    </Typography>
                    <Typography
                      variant="body2"
                      color="#23BD33"
                      sx={{
                        fontSize: "16px",
                        fontWeight: 600,
                      }}>{`${progress}% Hoàn thành`}</Typography>
                  </Box>
                  <LinearProgress
                    variant="determinate"
                    value={progress}
                    sx={{
                      height: 4,
                      borderRadius: 5,
                      "& .MuiLinearProgress-bar": {
                        borderRadius: 5,
                        backgroundColor: "#23BD33",
                      },
                      backgroundColor: "#E0E0E0", // Màu nền của thanh tiến trình
                    }}
                  />
                </Box>

                <Box>
                  <SourceDetail />
                </Box>
              </Box>
            </Grid>
          </Grid>
        </Box>
      </Box>
    </>
  );
};

export default WatchCourse;
