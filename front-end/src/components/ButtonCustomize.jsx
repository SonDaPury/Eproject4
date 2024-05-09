import Box from "@mui/material/Box";
import Button from "@mui/material/Button";

const ButtonCustomize = ({
  text = "Text mặc định",
  width = "130px",
  height = "40px",
  fontSize = "12px",
}) => {
  return (
    <Box>
      <Button
        variant="contained"
        sx={{
          color: "white",
          borderRadius: 0,
          boxShadow: "none",
          width: width,
          height: height,
          fontSize: fontSize,
        }}>
        {text}
      </Button>
    </Box>
  );
};

export default ButtonCustomize;
