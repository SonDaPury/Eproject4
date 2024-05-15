import seedData from "@eproject4/utils/seedData";
import { Box, createTheme, Grid, Pagination, Stack } from "@mui/material";
import { useParams } from "react-router-dom";

import Typography from "@mui/material/Typography";

import CardCourse from "@eproject4/components/CardCourse.jsx";

import "swiper/css";
import "swiper/css/navigation";
import { useEffect, useState } from "react";
import { ThemeProvider } from "@emotion/react";
import FilterPanel from "@eproject4/components/FilterPanel";

const Category = () => {
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
        {/* Top5 */}
        <Box
          sx={{
            width: "100vw",
            backgroundColor: "#F5F7FA",
            padding: "40px 0", // Padding trên dưới
            marginLeft: "calc(-50vw + 50%)", // Để toàn bộ Box chiếm toàn bộ chiều rộng màn hình
          }}>
          <Typography
            variant="h4"
            sx={{ textAlign: "center", marginBottom: "30px", fontWeight: 500 }}>
            Khóa Học Bán Chạy Trong {topicName}
          </Typography>
          <Box
            sx={{
              display: "flex",
              justifyContent: "center",
              alignItems: "center",
              gap: "25px",
              flexWrap: "wrap",
              padding: "0 10px", // Padding trái phải để không dính liền
            }}>
            {TopCourse.map((item, i) => (
              <Box key={i}>
                <CardCourse
                  title={item?.title}
                  category={item?.topic}
                  price={item?.price}
                  students={item?.views}
                  image={item?.imageThumbnail}
                  rating={item?.rating}
                />
              </Box>
            ))}
          </Box>
        </Box>
        {/* Filter */}
        {/* <Box sx={{ marginTop: "56px" }}>
          <Box
            sx={{
              maxWidth: "450px",
              [theme.breakpoints.up("lg")]: {
                maxWidth: "1080px",
              },
              [theme.breakpoints.up("xl")]: {
                maxWidth: "1320px",
              },
              [theme.breakpoints.up("md")]: {
                maxWidth: "800px",
              },
              [theme.breakpoints.up("sm")]: {
                maxWidth: "550px",
              },
              margin: "auto",
              display: "flex",
              justifyContent: "space-between",
            }}>
            <button onClick={handleClickFilter}>
              <Box
                sx={{
                  width: "108px",
                  height: "40px",
                  border: "1px solid #FFDDD1",
                  paddingTop: "7px",
                }}>
                <TuneIcon sx={{ marginRight: "10px" }} />
                Lọc
              </Box>
            </button>
            <Box>
              <TextField
                id="input-with-icon-textfield"
                placeholder="Search..."
                InputProps={{
                  startAdornment: (
                    <InputAdornment position="start">
                      <SearchIcon />
                    </InputAdornment>
                  ),
                }}
                variant="standard"
              />
            </Box>
          </Box>
          <Divider
            sx={{
              maxWidth: "450px",
              marginX: "auto",
              marginTop: "30px",
              [theme.breakpoints.up("lg")]: {
                maxWidth: "1080px",
              },
              [theme.breakpoints.up("xl")]: {
                maxWidth: "1320px",
              },
              [theme.breakpoints.up("md")]: {
                maxWidth: "800px",
              },
              [theme.breakpoints.up("sm")]: {
                maxWidth: "550px",
              },
            }}
          />
          <Box
            sx={{
              position: "relative",
              maxWidth: "450px",
              margin: "auto",
              [theme.breakpoints.up("lg")]: {
                maxWidth: "1080px",
              },
              [theme.breakpoints.up("xl")]: {
                maxWidth: "1320px",
              },
              [theme.breakpoints.up("md")]: {
                maxWidth: "800px",
              },
              [theme.breakpoints.up("sm")]: {
                maxWidth: "550px",
              },
            }}>
            <Box
              sx={{
                position: "absolute",
                left: 0,
                display: isShowFilter ? "block" : "none",
                zIndex: 1000,
              }}>
              <Box sx={{ maxWidth: "272px" }}>
                <Accordion>
                  <AccordionSummary
                    sx={{ height: "20px" }}
                    expandIcon={<ExpandMoreIcon />}>
                    <Typography
                      sx={{
                        fontSize: "18px",
                        fontWeight: 500,
                        textTransform: "uppercase",
                      }}>
                      Danh mục
                    </Typography>
                  </AccordionSummary>
                  <Divider />
                  <AccordionDetails sx={{ marginTop: "10px" }}>
                    <div>
                      {topics.map((topic, i) => {
                        return (
                          <Accordion key={i}>
                            <AccordionSummary
                              expandIcon={<ExpandMoreIcon />}
                              aria-controls="panel1-content"
                              id="panel1-header">
                              <Typography
                                sx={{ fontSize: "14px", fontWeight: 500 }}>
                                {topic}
                              </Typography>
                            </AccordionSummary>
                            <AccordionDetails>
                              Lorem ipsum dolor sit amet, consectetur adipiscing
                              elit. Suspendisse malesuada lacus ex, sit amet
                              blandit leo lobortis eget.
                            </AccordionDetails>
                          </Accordion>
                        );
                      })}
                    </div>
                  </AccordionDetails>
                </Accordion>

                <Accordion>
                  <AccordionSummary
                    sx={{ height: "20px" }}
                    expandIcon={<ExpandMoreIcon />}>
                    <Typography
                      sx={{
                        fontSize: "18px",
                        fontWeight: 500,
                        textTransform: "uppercase",
                      }}>
                      Đánh giá
                    </Typography>
                    <Divider />
                  </AccordionSummary>
                  <Divider />
                  <AccordionDetails>
                    <div>
                      <ListItem
                        sx={{
                          padding: 0,
                          display: "flex",
                          justifyContent: "space-between",
                        }}>
                        <Box
                          sx={{
                            display: "flex",
                            alignItems: "center",
                          }}>
                          <Checkbox />
                          <Box
                            sx={{
                              display: "flex",
                              alignItems: "center",
                            }}>
                            <ListItemIcon
                              sx={{
                                minWidth: 0,
                                marginRight: "7px",
                                color: "#FD8E1F",
                              }}>
                              <StarIcon
                                sx={{ width: "18px", height: "18px" }}
                              />
                            </ListItemIcon>
                            <ListItemText
                              sx={{
                                fontSize: "14px",
                                fontWeight: 400,
                                color: "#4E5566",
                              }}
                              primary={"5 stars"}
                            />
                          </Box>
                        </Box>
                        <Typography
                          sx={{
                            fontSize: "12px",
                            fontWeight: 400,
                            color: "#8C94A3",
                          }}>
                          1345
                        </Typography>
                      </ListItem>
                      <ListItem
                        sx={{
                          padding: 0,
                          display: "flex",
                          justifyContent: "space-between",
                        }}>
                        <Box
                          sx={{
                            display: "flex",
                            alignItems: "center",
                          }}>
                          <Checkbox />
                          <Box
                            sx={{
                              display: "flex",
                              alignItems: "center",
                            }}>
                            <ListItemIcon
                              sx={{
                                minWidth: 0,
                                marginRight: "7px",
                                color: "#FD8E1F",
                              }}>
                              <StarIcon
                                sx={{ width: "18px", height: "18px" }}
                              />
                            </ListItemIcon>
                            <ListItemText
                              sx={{
                                fontSize: "14px",
                                fontWeight: 400,
                                color: "#4E5566",
                              }}
                              primary={"4 stars & up"}
                            />
                          </Box>
                        </Box>
                        <Typography
                          sx={{
                            fontSize: "12px",
                            fontWeight: 400,
                            color: "#8C94A3",
                          }}>
                          1345
                        </Typography>
                      </ListItem>
                      <ListItem
                        sx={{
                          padding: 0,
                          display: "flex",
                          justifyContent: "space-between",
                        }}>
                        <Box
                          sx={{
                            display: "flex",
                            alignItems: "center",
                          }}>
                          <Checkbox />
                          <Box
                            sx={{
                              display: "flex",
                              alignItems: "center",
                            }}>
                            <ListItemIcon
                              sx={{
                                minWidth: 0,
                                marginRight: "7px",
                                color: "#FD8E1F",
                              }}>
                              <StarIcon
                                sx={{ width: "18px", height: "18px" }}
                              />
                            </ListItemIcon>
                            <ListItemText
                              sx={{
                                fontSize: "14px",
                                fontWeight: 400,
                                color: "#4E5566",
                              }}
                              primary={"3 stars & up"}
                            />
                          </Box>
                        </Box>
                        <Typography
                          sx={{
                            fontSize: "12px",
                            fontWeight: 400,
                            color: "#8C94A3",
                          }}>
                          1345
                        </Typography>
                      </ListItem>
                      <ListItem
                        sx={{
                          padding: 0,
                          display: "flex",
                          justifyContent: "space-between",
                        }}>
                        <Box
                          sx={{
                            display: "flex",
                            alignItems: "center",
                          }}>
                          <Checkbox />
                          <Box
                            sx={{
                              display: "flex",
                              alignItems: "center",
                            }}>
                            <ListItemIcon
                              sx={{
                                minWidth: 0,
                                marginRight: "7px",
                                color: "#FD8E1F",
                              }}>
                              <StarIcon
                                sx={{ width: "18px", height: "18px" }}
                              />
                            </ListItemIcon>
                            <ListItemText
                              sx={{
                                fontSize: "14px",
                                fontWeight: 400,
                                color: "#4E5566",
                              }}
                              primary={"2 stars & up"}
                            />
                          </Box>
                        </Box>
                        <Typography
                          sx={{
                            fontSize: "12px",
                            fontWeight: 400,
                            color: "#8C94A3",
                          }}>
                          1345
                        </Typography>
                      </ListItem>
                      <ListItem
                        sx={{
                          padding: 0,
                          display: "flex",
                          justifyContent: "space-between",
                        }}>
                        <Box
                          sx={{
                            display: "flex",
                            alignItems: "center",
                          }}>
                          <Checkbox />
                          <Box
                            sx={{
                              display: "flex",
                              alignItems: "center",
                            }}>
                            <ListItemIcon
                              sx={{
                                minWidth: 0,
                                marginRight: "7px",
                                color: "#FD8E1F",
                              }}>
                              <StarIcon
                                sx={{ width: "18px", height: "18px" }}
                              />
                            </ListItemIcon>
                            <ListItemText
                              sx={{
                                fontSize: "14px",
                                fontWeight: 400,
                                color: "#4E5566",
                              }}
                              primary={"1 stars & up"}
                            />
                          </Box>
                        </Box>
                        <Typography
                          sx={{
                            fontSize: "12px",
                            fontWeight: 400,
                            color: "#8C94A3",
                          }}>
                          1345
                        </Typography>
                      </ListItem>
                    </div>
                  </AccordionDetails>
                </Accordion>
                <Accordion>
                  <AccordionSummary
                    sx={{ height: "20px" }}
                    expandIcon={<ExpandMoreIcon />}>
                    <Typography
                      sx={{
                        fontSize: "18px",
                        fontWeight: 500,
                        textTransform: "uppercase",
                      }}>
                      Giá
                    </Typography>
                  </AccordionSummary>
                  <Divider />
                  <AccordionDetails sx={{ marginTop: "10px" }}>
                    <div>
                      <Box sx={{ display: "flex" }}>
                        <FormControl fullWidth sx={{ m: 1, width: "49%" }}>
                          <InputLabel htmlFor="outlined-adornment-amount">
                            Từ
                          </InputLabel>
                          <OutlinedInput
                            sx={{ height: "48px" }}
                            id="outlined-adornment-amount"
                            startAdornment={
                              <InputAdornment position="start">
                                $
                              </InputAdornment>
                            }
                            label="Amount"
                          />
                        </FormControl>
                        <FormControl fullWidth sx={{ m: 1, width: "49%" }}>
                          <InputLabel htmlFor="outlined-adornment-amount">
                            Đến
                          </InputLabel>
                          <OutlinedInput
                            id="outlined-adornment-amount"
                            sx={{ height: "48px" }}
                            startAdornment={
                              <InputAdornment position="start">
                                $
                              </InputAdornment>
                            }
                            label="Amount"
                          />
                        </FormControl>
                      </Box>
                      <Box>
                        <ListItem
                          sx={{
                            padding: 0,
                            display: "flex",
                            justifyContent: "space-between",
                          }}>
                          <Box
                            sx={{
                              display: "flex",
                              alignItems: "center",
                            }}>
                            <Checkbox />
                            <Box
                              sx={{
                                display: "flex",
                                alignItems: "center",
                              }}>
                              <ListItemIcon
                                sx={{
                                  minWidth: 0,
                                  marginRight: "7px",
                                  color: "#FD8E1F",
                                }}></ListItemIcon>
                              <ListItemText
                                sx={{
                                  fontSize: "14px",
                                  fontWeight: 400,
                                  color: "#4E5566",
                                }}
                                primary={"Trả phí"}
                              />
                            </Box>
                          </Box>
                        </ListItem>
                        <ListItem
                          sx={{
                            padding: 0,
                            display: "flex",
                            justifyContent: "space-between",
                          }}>
                          <Box
                            sx={{
                              display: "flex",
                              alignItems: "center",
                            }}>
                            <Checkbox />
                            <Box
                              sx={{
                                display: "flex",
                                alignItems: "center",
                              }}>
                              <ListItemIcon
                                sx={{
                                  minWidth: 0,
                                  marginRight: "7px",
                                  color: "#FD8E1F",
                                }}></ListItemIcon>
                              <ListItemText
                                sx={{
                                  fontSize: "14px",
                                  fontWeight: 400,
                                  color: "#4E5566",
                                }}
                                primary={"Miễn phí"}
                              />
                            </Box>
                          </Box>
                        </ListItem>
                      </Box>
                    </div>
                  </AccordionDetails>
                </Accordion>
              </Box>
            </Box>
          </Box>
        </Box> */}
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
              <Box xs={12} sm={6} md={3} lg={3} key={i}>
                <CardCourse
                  path={`/course-detail/${item?.topic}/${encodeURIComponent(item?.title)}`}
                  title={item?.title}
                  category={item?.topic}
                  price={item?.price}
                  students={item?.views}
                  image={item?.imageThumbnail}
                  rating={item?.rating}
                />
              </Box>
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

export default Category;
