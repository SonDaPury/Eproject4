import { Link } from "react-router-dom";
import AppBar from "@mui/material/AppBar";
import Box from "@mui/material/Box";
import Toolbar from "@mui/material/Toolbar";
import IconButton from "@mui/material/IconButton";
import Typography from "@mui/material/Typography";
import Menu from "@mui/material/Menu";
import MenuIcon from "@mui/icons-material/Menu";
import Container from "@mui/material/Container";
import Avatar from "@mui/material/Avatar";
import Button from "@mui/material/Button";
import Tooltip from "@mui/material/Tooltip";
import MenuItem from "@mui/material/MenuItem";
import { ReactComponent as LmsLogo } from "@eproject4/assets/images/logo.svg";
import React from "react";
import { Badge, styled, SvgIcon } from "@mui/material";
import FavoriteBorderIcon from "@mui/icons-material/FavoriteBorder";
import ShoppingCartIcon from "@mui/icons-material/ShoppingCart";
import ButtonCustomize from "@eproject4/components/ButtonCustomize";
import { getToken, signOut } from "@eproject4/helpers/authHelper";

const pages = [
  { text: "Khóa học", path: "/khoa-hoc" },
  { text: "Về chúng tôi", path: "/ve-chung-toi" },
];
const settings = ["Profile", "Account", "Dashboard", "Logout"];

const StyledBadge = styled(Badge)(({ theme }) => ({
  "& .MuiBadge-badge": {
    right: -3,
    top: 5,
    border: `2px solid ${theme.palette.background.paper}`,
    padding: "0 4px",
  },
}));

function Header() {
  const [anchorElNav, setAnchorElNav] = React.useState(null);
  const [anchorElUser, setAnchorElUser] = React.useState(null);
  const token = getToken();

  const handleOpenNavMenu = (event) => {
    setAnchorElNav(event.currentTarget);
  };
  const handleOpenUserMenu = (event) => {
    setAnchorElUser(event.currentTarget);
  };

  const handleCloseNavMenu = () => {
    setAnchorElNav(null);
  };

  const handleCloseUserMenu = () => {
    setAnchorElUser(null);
  };
  const handleLogout = () => {
    setAnchorElUser(null);
    signOut();
  };

  return (
    <Box>
      <AppBar
        position="static"
        sx={{
          backgroundColor: (theme) => theme.palette.primary.layout,
        }}>
        <Container maxWidth="xxl">
          <Toolbar disableGutters>
            <SvgIcon
              component={LmsLogo}
              inheritViewBox
              sx={{
                color: "#FFFFFF",
                display: { xs: "none", md: "flex" },
                mr: "10px",
              }}
            />
            <Link to="/" className="mr-[50px] hidden md:flex">
              <Typography
                variant="h6"
                noWrap
                href="#app-bar-with-responsive-menu"
                sx={{
                  fontSize: "20px",
                  display: { xs: "none", md: "flex" },
                  fontWeight: 500,
                  color: "#FFFFFF",
                }}>
                E-Learning
              </Typography>
            </Link>

            <Box sx={{ flexGrow: 1, display: { xs: "flex", md: "none" } }}>
              <IconButton
                size="large"
                aria-label="account of current user"
                aria-controls="menu-appbar"
                aria-haspopup="true"
                onClick={handleOpenNavMenu}
                sx={{ color: "#FFFFFF" }}>
                <MenuIcon/>
              </IconButton>
              <Menu
                id="menu-appbar"
                anchorEl={anchorElNav}
                anchorOrigin={{
                  vertical: "bottom",
                  horizontal: "left",
                }}
                keepMounted
                transformOrigin={{
                  vertical: "top",
                  horizontal: "left",
                }}
                open={Boolean(anchorElNav)}
                onClose={handleCloseNavMenu}
                sx={{
                  display: { xs: "block", md: "none" },
                }}>
                <Link to="/">
                  <MenuItem onClick={handleCloseNavMenu}>
                    <Typography textAlign="center">Trang chủ</Typography>
                  </MenuItem>
                </Link>

                {pages.map((item, index) => (
                  <Link to={item.path} key={index}>
                    <MenuItem key={index} onClick={handleCloseNavMenu}>
                      <Typography textAlign="center">{item.text}</Typography>
                    </MenuItem>
                  </Link>
                ))}
              </Menu>
            </Box>
            <SvgIcon
              component={LmsLogo}
              inheritViewBox
              sx={{
                color: "#FFFFFF",
                display: { xs: "flex", md: "none" },
                mr: "10px",
              }}
            />
            <Typography
              variant="h5"
              noWrap
              href="#app-bar-with-responsive-menu"
              sx={{
                mr: 2,
                display: { xs: "flex", md: "none" },
                flexGrow: 1,
                fontSize: "20px",
                fontWeight: 500,
                color: "#FFFFFF",
              }}>
              E-Learning
            </Typography>

            <Box sx={{ flexGrow: 1, display: { xs: "none", md: "flex" } }}>
              {pages.map((item, index) => (
                <Link to={item.path} key={index}>
                  <Button
                    onClick={handleCloseNavMenu}
                    sx={{
                      my: 2,
                      color: "white",
                      display: "block",
                      fontWeight: 400,
                      fontSize: "13px",
                    }}>
                    {item.text}
                  </Button>
                </Link>
              ))}
            </Box>
            <Box sx={{ display: token ? "none" : "flex" }}>
              <Link to={"/dang-ky"}>
                <ButtonCustomize text="Đăng ký" width="100px"/>
              </Link>
              <Link to={"/dang-nhap"}>
                <ButtonCustomize
                  text="Đăng nhập"
                  width="110px"
                  backgroundColor="#0E1640"
                  variant="outlined"
                  sx={{ border: "0.5px solid #FFFFFF", marginLeft: "10px" }}
                />
              </Link>
            </Box>
            <Box sx={{ flexGrow: 0, display: token ? "flex" : "none" }}>
              <Box sx={{ marginRight: "30px" }}>
                <button className="mr-[10px]">
                  <FavoriteBorderIcon sx={{ color: "#FFFFFF" }}/>
                </button>
                <IconButton sx={{ color: "#FFFFFF" }} aria-label="cart">
                  <StyledBadge badgeContent={4} color="secondary">
                    <ShoppingCartIcon/>
                  </StyledBadge>
                </IconButton>
              </Box>
              <Tooltip title="Open settings">
                <IconButton onClick={handleOpenUserMenu} sx={{ p: 0 }}>
                  <Avatar alt="Remy Sharp" src="/static/images/avatar/2.jpg"/>
                </IconButton>
              </Tooltip>
              <Menu
                sx={{ mt: "45px" }}
                id="menu-appbar"
                anchorEl={anchorElUser}
                anchorOrigin={{
                  vertical: "top",
                  horizontal: "right",
                }}
                keepMounted
                transformOrigin={{
                  vertical: "top",
                  horizontal: "right",
                }}
                open={Boolean(anchorElUser)}
                onClose={handleCloseUserMenu}>
                {/*{settings.map((setting) => (*/}
                {/*  <MenuItem key={setting} onClick={handleCloseUserMenu}>*/}
                {/*    <Typography textAlign="center">{setting}</Typography>*/}
                {/*  </MenuItem>*/}
                {/*))}*/}
                <MenuItem onClick={handleCloseUserMenu}>
                  <Typography textAlign="center">Tài khoản</Typography>
                </MenuItem>
                <MenuItem onClick={handleCloseUserMenu}>
                  <Typography textAlign="center">Khóa học</Typography>
                </MenuItem>
                <MenuItem onClick={handleLogout}>
                  <Typography textAlign="center">Đăng xuất</Typography>
                </MenuItem>
              </Menu>
            </Box>
          </Toolbar>
        </Container>
      </AppBar>
    </Box>
  );
}

export default Header;
