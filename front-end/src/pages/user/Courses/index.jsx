import Box from "@mui/material/Box";
import TextField from "@mui/material/TextField";
import TuneIcon from "@mui/icons-material/Tune";
import InputAdornment from "@mui/material/InputAdornment";
import SearchIcon from "@mui/icons-material/Search";
import Divider from "@mui/material/Divider";
import Typography from "@mui/material/Typography";
import { Swiper, SwiperSlide } from "swiper/react";
import { Navigation } from "swiper/modules";
import theme from "@eproject4/theme";
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
import seedData from "@eproject4/utils/seedData";
import "swiper/css";
import "swiper/css/navigation";
import ButtonCustomize from "@eproject4/components/ButtonCustomize";
import { useState } from "react";
import { FormControl, InputLabel, OutlinedInput } from "@mui/material";

const categories = [
  {
    id: 1,
    name: "Danh mục 1",
    children: [
      { id: 11, name: "Danh mục con 1.1" },
      { id: 12, name: "Danh mục con 1.2" },
      // Thêm danh mục con khác nếu cần
    ],
  },
  {
    id: 2,
    name: "Danh mục 2",
    children: [
      { id: 21, name: "Danh mục con 2.1" },
      { id: 22, name: "Danh mục con 2.2" },
      // Thêm danh mục con khác nếu cần
    ],
  },
  // Thêm danh mục khác nếu cần
];

function Courses() {
  let i = 0;
  const courseTopics = [];
  const [isShowFilter, setIsShowFilter] = useState(false);

  const removeDuplicate = (arr) => {
    return Array.from(new Set(arr));
  };

  seedData().forEach((course) => {
    courseTopics.push(course?.topic);
  });

  const topics = removeDuplicate(courseTopics);
  console.log(topics);
  const data = seedData();

  const handleClickFilter = () => {
    setIsShowFilter(!isShowFilter);
  };

  return (
    <Box sx={{ marginTop: "56px" }}>
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

      <Box>
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
          {/* Filter */}
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
                            <StarIcon sx={{ width: "18px", height: "18px" }} />
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
                            <StarIcon sx={{ width: "18px", height: "18px" }} />
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
                            <StarIcon sx={{ width: "18px", height: "18px" }} />
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
                            <StarIcon sx={{ width: "18px", height: "18px" }} />
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
                            <StarIcon sx={{ width: "18px", height: "18px" }} />
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
                            <InputAdornment position="start">$</InputAdornment>
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
                            <InputAdornment position="start">$</InputAdornment>
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

        <Box>
          {topics.map((topic, index) => {
            ++i;
            return (
              <Box
                key={index}
                sx={{
                  margin: "auto",
                  backgroundColor: i % 2 === 0 ? "#F5F7FA" : "white",
                }}>
                <Box
                  sx={{
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
                    maxWidth: "1320px",
                    margin: "auto",
                    paddingTop: "5px",
                    paddingBottom: "40px",
                  }}>
                  <Typography
                    variant="h4"
                    sx={{
                      textAlign: "center",
                      fontWeight: 500,
                      marginTop: "30px",
                    }}>
                    {topic}
                  </Typography>
                  <Box sx={{ marginTop: "30px", textAlign: "center" }}>
                    <Swiper
                      className="mb-[20px]"
                      style={{
                        "--swiper-pagination-color": "#626262",
                        "--swiper-navigation-color": "#626262",
                      }}
                      modules={[Navigation]}
                      navigation
                      spaceBetween={10}
                      slidesPerView={5}
                      breakpoints={{
                        300: {
                          slidesPerView: 1,
                        },
                        768: {
                          slidesPerView: 3,
                        },
                        1200: {
                          slidesPerView: 5,
                        },
                      }}>
                      {data.map((item, i) => {
                        if (item?.topic === topic) {
                          return (
                            <SwiperSlide key={i}>
                              <CardCourse
                                path={`/course-detail/${item?.topic}/${encodeURIComponent(item?.title)}`}
                                title={item?.title}
                                category={item?.topic}
                                price={item?.price}
                                students={item?.views}
                                image={item?.imageThumbnail}
                                rating={item?.rating}
                              />
                            </SwiperSlide>
                          );
                        }
                      })}
                    </Swiper>
                    <ButtonCustomize
                      text="Xem tất cả"
                      navigateTo={`/category/${topic}`}
                    />
                  </Box>
                </Box>
              </Box>
            );
          })}
        </Box>
      </Box>
    </Box>
  );
}

export default Courses;
