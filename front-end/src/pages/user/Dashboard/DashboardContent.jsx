import React from "react";
import {
  Container,
  Box,
  Typography,
  Avatar,
  Button,
  Grid,
  Card,
  CardContent,
  CardMedia,
  Tabs,
  Tab,
  Paper,
} from "@mui/material";

import styled from "@emotion/styled";
import CardCourse from "@eproject4/components/CardCourse";
import CardCourseDb from "@eproject4/components/StDashboard/HorizontalCourseCard";
import HorizontalCourseCard from "@eproject4/components/StDashboard/HorizontalCourseCard";
import PlayCircleOutlineIcon from "@mui/icons-material/PlayCircleOutline";
import LibraryAddCheckIcon from "@mui/icons-material/LibraryAddCheck";
import EmojiEventsIcon from "@mui/icons-material/EmojiEvents";

export default function DashboardContent() {
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

  const stats = [
    {
      icon: <PlayCircleOutlineIcon sx={{ color: "#FF6636", fontSize: 30 }} />,
      number: 957,
      description: "Khóa học đã mua",
      bg: "#FFEEE8",
    },
    {
      icon: <LibraryAddCheckIcon sx={{ color: "#6A5ACD", fontSize: 30 }} />,
      number: 6,
      description: "Khóa học đang học",
      bg: "#E6E6FA",
    },
    {
      icon: <EmojiEventsIcon sx={{ color: "#2E8B57", fontSize: 30 }} />,
      number: 951,
      description: "Khóa học hoàn thành",
      bg: "#98FB98",
    },
  ];

  return (
    <Box>
      <>
        <Typography variant="h6" sx={{ marginBottom: 2 }}>
          Dashboard
        </Typography>

        <Box sx={{ flexGrow: 1, padding: "24px 0px", width: "984px" }}>
          <Grid container spacing={2}>
            {stats.map((stat, index) => (
              <Grid item xs={4} key={index}>
                <Paper
                  sx={{
                    display: "flex",
                    alignItems: "center",
                    padding: 2,
                    backgroundColor: stat.bg,
                  }}
                  elevation={3}>
                  <Card
                    sx={{
                      width: "60px",
                      height: "60px",
                      display: "flex",
                      justifyContent: "center",
                      alignItems: "center",
                      marginRight: "24px",
                    }}>
                    <Box>{stat.icon}</Box>
                  </Card>

                  <Box>
                    <Typography
                      variant="h4"
                      sx={{
                        color: "#1D2026",
                        fontSize: "24px",
                        fontWeight: 400,
                      }}>
                      {stat.number}
                    </Typography>
                    <Typography
                      sx={{
                        color: "#4E5566",
                        fontSize: "14px",
                        fontWeight: 400,
                      }}>
                      {stat.description}
                    </Typography>
                  </Box>
                </Paper>
              </Grid>
            ))}
          </Grid>
        </Box>

        <Typography variant="h6" sx={{ marginBottom: 2 }}>
          Tiếp tục học
        </Typography>

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
    </Box>
  );
}
