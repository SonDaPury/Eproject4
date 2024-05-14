import React from "react";
import { useNavigate } from "react-router-dom";
import Button from "@mui/material/Button";
import Box from "@mui/material/Box";

const ButtonCustomize = ({
  text = "Text mặc định",
  width = "130px",
  height = "40px",
  fontSize = "12px",
  backgroundColor = "main.primary",
  variant = "contained",
  sx = {},
  navigateTo = "/", // Thêm prop này để xác định URL điều hướng
}) => {
  const navigate = useNavigate();

  const handleClick = () => {
    navigate(navigateTo); // Sử dụng navigate khi nhấn vào nút
  };

  return (
    <Box>
      <Button
        variant={variant}
        sx={{
          color: "white",
          borderRadius: 0,
          boxShadow: "none",
          width: width,
          height: height,
          fontSize: fontSize,
          backgroundColor: backgroundColor,
          ...sx,
        }}
        onClick={handleClick}>
        {text}
      </Button>
    </Box>
  );
};

export default ButtonCustomize;
