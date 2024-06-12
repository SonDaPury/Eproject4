import React, { useEffect } from "react";
import {
  Box,
  TextField,
  InputAdornment,
  Accordion,
  AccordionSummary,
  AccordionDetails,
  Typography,
  Divider,
  ListItem,
  Checkbox,
  FormControl,
  InputLabel,
  OutlinedInput,
  ListItemIcon,
  ListItemText,
} from "@mui/material";
import TuneIcon from "@mui/icons-material/Tune";
import SearchIcon from "@mui/icons-material/Search";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import StarIcon from "@mui/icons-material/Star";
import theme from "@eproject4/theme";
import { useNavigate } from "react-router-dom";
import { useState } from "react";
import { searchFullText } from "@eproject4/services/search.service";

function FilterPanel({
  isShowFilter,
  topics,
  handleClickFilter,
  handleSearchResults,
}) {
  const [searchKeyword, setSearchKeyword] = useState("");
  const { searchFullTextAction } = searchFullText();
  const navigate = useNavigate();
  const handleCourseClick = (topic) => {
    navigate(`/course-list/${topic}`);
  };

  // useEffect(() => {
  //   window.addEventListener("keyup", handleSearchChange);
  //   return () => {
  //     window.removeEventListener("keyup", handleSearchChange);
  //   };
  // }, []);
  const handleSearchChange = async (event) => {
    const keyword = event.target.value;
    setSearchKeyword(keyword);
  };

  const handleSearchKeyUp = async (event) => {
    if (event.keyCode === 13 && searchKeyword.length > 2) {
      try {
        const res = await searchFullTextAction(searchKeyword);
        console.log("Full Response:", res); // Log toàn bộ đối tượng res

        // Kiểm tra và log định dạng của res.data
        if (res?.data && Array.isArray(res.data)) {
          const items = res.data;
          console.log("Items found:", items);
          handleSearchResults(items); // Truyền kết quả tìm kiếm lên ListCourses
        } else {
          console.log("No items found or data is not an array");
          handleSearchResults([]); // Truyền mảng rỗng nếu không có items
        }

        console.log("Title:", res?.data || []);
      } catch (err) {
        console.error(err);
      }
    } else if (searchKeyword.length <= 2) {
      handleSearchResults([]); // Clear search results if keyword is less than 3 characters
    }
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
            value={searchKeyword}
            onChange={handleSearchChange}
            onKeyUp={handleSearchKeyUp}
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
                      <Accordion
                        key={i}
                        onChange={() => handleCourseClick(topic)}>
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
                          elit. Suspendisse malesuada lacus ex, sit amet blandit
                          leo lobortis eget.
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
    </Box>
  );
}

export default FilterPanel;
