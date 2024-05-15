import theme from "@eproject4/theme";
import seedData from "@eproject4/utils/seedData";
import { Box, createTheme, Grid, Pagination, Stack } from "@mui/material";
import { useParams } from "react-router-dom";

import TextField from "@mui/material/TextField";
import TuneIcon from "@mui/icons-material/Tune";
import InputAdornment from "@mui/material/InputAdornment";
import SearchIcon from "@mui/icons-material/Search";
import Divider from "@mui/material/Divider";
import Typography from "@mui/material/Typography";

import CardCourse from "@eproject4/components/CardCourse.jsx";
import Accordion from "@mui/material/Accordion";
import StarIcon from "@mui/icons-material/Star";
import ListItem from "@mui/material/ListItem";
import ListItemText from "@mui/material/ListItemText";
import ListItemIcon from "@mui/material/ListItemIcon";
import Checkbox from "@mui/material/Checkbox";
import AccordionSummary from "@mui/material/AccordionSummary";
import AccordionDetails from "@mui/material/AccordionDetails";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";

import "swiper/css";
import "swiper/css/navigation";
import { useEffect, useState } from "react";
import { FormControl, InputLabel, OutlinedInput } from "@mui/material";
import { ThemeProvider } from "@emotion/react";
import FilterPanel from "@eproject4/components/FilterPanel";

const ListCategory = () => {
  // Roters
  const { topicName } = useParams();
  const courseTopics = [];
  const [isShowFilter, setIsShowFilter] = useState(false);
  const removeDuplicate = (arr) => {
    return Array.from(new Set(arr));
  };
  seedData().forEach((course) => {
    courseTopics.push(course?.topic);
  });
  const topics = removeDuplicate(courseTopics);
  const handleClickFilter = () => {
    setIsShowFilter(!isShowFilter);
  };

  // Lọc dữ liệu

  const [filteredData, setFilteredData] = useState([]);
  // Xử lý xét Topic
  useEffect(() => {
    const data = seedData();
    const filteredData = data.filter((course) => course.topic === topicName);
    setFilteredData(filteredData);
  }, [topicName]);
  // Sắp xếp số lượng View
  const ViewStudent = filteredData.sort((a, b) => b.views - a.views);

  const TopCourse = ViewStudent.slice(0, 5);

  // Màu phân trang
  const theme = createTheme({
    palette: {
      primary: {
        main: "##FF6636", // Màu cam
      },
    },
  });

  return (
    <Box sx={{ maxWidth: "1320px", margin: "auto" }}>
      <Box>
        {" "}
        <div>
          <FilterPanel
            isShowFilter={isShowFilter}
            topics={topics}
            handleClickFilter={handleClickFilter}
          />
        </div>
        {/* ................ */}
        {/* All list */}
        <Box sx={{ marginTop: "40px", textAlign: "center" }}>
          <Box
            sx={{
              display: "flex",
              gap: "25px",
              flexWrap: "wrap",
            }}>
            {filteredData.map((item, i) => (
              <Grid xs={12} sm={6} md={3} lg={3} key={i}>
                <CardCourse
                  title={item?.title}
                  category={item?.topic}
                  price={item?.price}
                  students={item?.views}
                  image={item?.imageThumbnail}
                  rating={item?.rating}
                />
              </Grid>
            ))}
          </Box>

          <ThemeProvider theme={theme}>
            <Stack
              spacing={2}
              sx={{
                justifyContent: "center",
                alignItems: "center",
                margin: "45px 0px",
              }}>
              <Pagination count={10} color="primary" />
            </Stack>
          </ThemeProvider>
        </Box>
      </Box>
    </Box>
  );
};

export default ListCategory;
