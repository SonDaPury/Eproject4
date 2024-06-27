import { getCourseById } from "@eproject4/services/courses.service";
import { yupResolver } from "@hookform/resolvers/yup";
import { Box, Modal, Tab, Tabs } from "@mui/material";
import PropTypes from "prop-types";
import { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import * as yup from "yup";

const schema = yup.object().shape({
  courseName: yup.string().required("Tên khóa học không được để trống"),
  category: yup.string().required("Danh mục không được để trống"),
  language: yup.string().required("Ngôn ngữ không được để trống"),
  status: yup.string().required("Trạng thái không được để trống"),
  paymentType: yup.string().required("Lựa chọn giá không được để trống"),
  price: yup.number().when("paymentType", {
    is: "paid",
    then: (schema) =>
      schema.typeError("Giá phải là số").required("Giá không được để trống"),
    otherwise: (schema) => schema.nullable(),
  }),
});

function UpdateCourse({
  openUpdateCourseModal,
  handleUpdateCourseModalClose,
  idQuery,
}) {
  const [value, setValue] = useState(0);
  const [courseDetail, setCourseDetail] = useState({});
  const { getCourseByIdAction } = getCourseById();
  const {
    control,
    handleSubmit,
    watch,
    formState: { errors },
  } = useForm({
    resolver: yupResolver(schema),
    defaultValues: {
      courseName: courseDetail.title || "",
      category: courseDetail.topicId || "",
      subcategory: courseDetail.subTopicId || "",
      language: courseDetail.language || "Tiếng Việt",
      status: courseDetail.status || "",
      paymentType: courseDetail.paymentType || "free",
      price: courseDetail.price || null,
    },
  });

  // Get course detail
  useEffect(() => {
    const fetchCourseDetail = async () => {
      const res = await getCourseByIdAction(idQuery);
      setCourseDetail(res.data);
    };

    fetchCourseDetail();
  }, []);

  const handleChange = (event, newValue) => {
    setValue(newValue);
  };

  return (
    <Box>
      <Modal
        open={openUpdateCourseModal}
        onClose={handleUpdateCourseModalClose}
        aria-labelledby="modal-modal-title"
        aria-describedby="modal-modal-description">
        <Box
          sx={{
            position: "absolute",
            top: "50%",
            left: "50%",
            transform: "translate(-50%, -50%)",
            width: 700,
            bgcolor: "background.paper",
            boxShadow: 24,
          }}>
          <Box sx={{ width: "100%" }}>
            <Box sx={{ borderBottom: 1, borderColor: "divider" }}>
              <Tabs
                value={value}
                onChange={handleChange}
                aria-label="basic tabs example">
                <Tab label="Thông tin cơ bản" {...a11yProps(0)} />
                <Tab label="Thông tin nâng cao" {...a11yProps(1)} />
              </Tabs>
            </Box>
            <CustomTabPanel value={value} index={0}>
              Item One
            </CustomTabPanel>
            <CustomTabPanel value={value} index={1}>
              Item Two
            </CustomTabPanel>
          </Box>
        </Box>
      </Modal>
    </Box>
  );
}

export default UpdateCourse;

function CustomTabPanel(props) {
  const { children, value, index, ...other } = props;

  return (
    <div
      role="tabpanel"
      hidden={value !== index}
      id={`simple-tabpanel-${index}`}
      aria-labelledby={`simple-tab-${index}`}
      {...other}>
      {value === index && <Box sx={{ p: 3 }}>{children}</Box>}
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
