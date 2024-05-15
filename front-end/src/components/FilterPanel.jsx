import React from "react";
import {
  Box,
  Button,
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

function FilterPanel({ isShowFilter, topics, handleClickFilter }) {
  const navigate = useNavigate();
  const handleCategoryClick = (topic) => {
    navigate(`/category-list/${topic}`);
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
                        onChange={() => handleCategoryClick(topic)}>
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
