import { getCourseById } from "@eproject4/services/courses.service";
import { yupResolver } from "@hookform/resolvers/yup";
import {
  Box,
  Button,
  FormControl,
  FormHelperText,
  Grid,
  InputLabel,
  MenuItem,
  Modal,
  Select,
  Tab,
  Tabs,
  TextField,
} from "@mui/material";
import PropTypes from "prop-types";
import { useEffect, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import * as yup from "yup";
import { getAllTopics } from "@eproject4/services/topic.service.js";
import { getSubTopics } from "@eproject4/services/subTopic.service.js";

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
  const [categories, setCategories] = useState([]);
  const [subcategories, setSubcategories] = useState([]);
  const { getAllTopicsAction } = getAllTopics();
  const { getSubTopicsAction } = getSubTopics();
  const [basicInfo, setBasicInfo] = useState({});
  const {
    control,
    handleSubmit,
    watch,
    formState: { errors },
    reset,
  } = useForm({
    resolver: yupResolver(schema),
    defaultValues: {
      courseName: "",
      category: "",
      subcategory: "",
      language: "Tiếng Việt",
      status: "",
      paymentType: "free",
      price: null,
    },
  });

  const paymentType = watch("paymentType");
  const categorySelected = watch("category");

  // Get categories and subcatergories
  useEffect(() => {
    const fetchTopicData = async () => {
      const res = await getAllTopicsAction();
      setCategories(res?.data?.items);
    };

    const fetchSubTopicData = async () => {
      const res = await getSubTopicsAction();
      setSubcategories(res?.data);
    };

    fetchTopicData();
    fetchSubTopicData();
  }, []);

  // Get course detail
  useEffect(() => {
    const fetchCourseDetail = async () => {
      const res = await getCourseByIdAction(idQuery);
      setCourseDetail(res.data);
      reset({
        courseName: res.data?.title || "",
        category: res.data?.topicId || "",
        subcategory: res.data?.subTopicId || "",
        language: res.data?.language || "Tiếng Việt",
        status: res.data?.status == 1 ? true : false,
        paymentType: res.data?.price > 0 ? "paid" : "free",
        price: res.data?.price || null,
      });
    };

    fetchCourseDetail();
  }, []);

  const handleChange = (event, newValue) => {
    setValue(newValue);
  };

  const onSubmit = async (data) => {
    const basicInfoData = {
      courseName: data.courseName,
      categoryId: data.category,
      subCategoryId: data.subcategory,
      language: data.language,
      status: data.status,
      price: data.price,
      paymentType: data.paymentType,
    };
    if (!data.price) {
      basicInfoData.price = 0;
    }
    if (!data.subcategory) {
      basicInfoData.subCategoryId = null;
    }

    setBasicInfo(basicInfoData);
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
              <form onSubmit={handleSubmit(onSubmit)}>
                <Grid container spacing={2}>
                  <Grid item xs={12}>
                    <Controller
                      name="courseName"
                      control={control}
                      render={({ field, fieldState: { error } }) => (
                        <TextField
                          {...field}
                          id="outlined-basic"
                          defaultValue=""
                          label="Nhập tiêu đề"
                          variant="outlined"
                          error={!!error}
                          helperText={error ? error.message : null}
                          sx={{ marginTop: "10px", width: "100%" }}
                        />
                      )}
                    />
                  </Grid>
                  <Grid item xs={6}>
                    <Controller
                      name="category"
                      control={control}
                      render={({ field, fieldState: { error } }) => (
                        <FormControl fullWidth error={!!error}>
                          <InputLabel>Danh mục</InputLabel>
                          <Select {...field} label="Danh mục" defaultValue="">
                            {categories.map((option, index) => (
                              <MenuItem key={index} value={option?.id}>
                                {option.topicName}
                              </MenuItem>
                            ))}
                          </Select>
                          <FormHelperText>{error?.message}</FormHelperText>
                        </FormControl>
                      )}
                    />
                  </Grid>
                  <Grid item xs={6}>
                    <Controller
                      name="subcategory"
                      control={control}
                      render={({ field, fieldState: { error } }) => (
                        <FormControl
                          fullWidth
                          error={!!error}
                          disabled={!categorySelected}>
                          <InputLabel>Danh mục con</InputLabel>
                          <Select
                            {...field}
                            label="Danh mục con"
                            defaultValue="">
                            {categorySelected &&
                              subcategories.map((option, index) => {
                                if (
                                  option?.topicId === Number(categorySelected)
                                ) {
                                  return (
                                    <MenuItem key={index} value={option.id}>
                                      {option.subTopicName}
                                    </MenuItem>
                                  );
                                }
                              })}
                          </Select>
                          <FormHelperText>{error?.message}</FormHelperText>
                        </FormControl>
                      )}
                    />
                  </Grid>
                  <Grid item xs={12}>
                    <Controller
                      name="language"
                      control={control}
                      render={({ field }) => (
                        <TextField
                          {...field}
                          defaultValue=""
                          select
                          label="Ngôn ngữ"
                          fullWidth
                          error={!!errors.language}
                          helperText={errors.language?.message}>
                          <MenuItem value="Tiếng Việt">Tiếng Việt</MenuItem>
                        </TextField>
                      )}
                    />
                  </Grid>
                  <Grid item xs={12}>
                    <Controller
                      name="status"
                      control={control}
                      render={({ field }) => (
                        <TextField
                          {...field}
                          select
                          defaultValue=""
                          label="Trạng thái"
                          fullWidth
                          error={!!errors.status}
                          helperText={errors.status?.message}>
                          <MenuItem value="false">Private</MenuItem>
                          <MenuItem value="true">Public</MenuItem>
                        </TextField>
                      )}
                    />
                  </Grid>
                  <Grid item xs={12}>
                    <Controller
                      name="paymentType"
                      control={control}
                      render={({ field }) => (
                        <TextField
                          {...field}
                          select
                          label="Miễn phí hoặc trả phí"
                          defaultValue=""
                          fullWidth
                          error={!!errors.paymentType}
                          helperText={errors.paymentType?.message}>
                          <MenuItem value="free">Miễn phí</MenuItem>
                          <MenuItem value="paid">Trả phí</MenuItem>
                        </TextField>
                      )}
                    />
                  </Grid>
                  <Grid item xs={12}>
                    <Controller
                      name="price"
                      control={control}
                      render={({ field }) => (
                        <TextField
                          {...field}
                          defaultValue=""
                          label="Giá"
                          fullWidth
                          disabled={paymentType !== "paid"}
                          error={!!errors.price}
                          helperText={errors.price?.message}
                        />
                      )}
                    />
                  </Grid>
                  <Grid
                    item
                    xs={12}
                    sx={{
                      display: "flex",
                      justifyContent: "end",
                      marginTop: "15px",
                    }}>
                    <Button
                      sx={{ boxShadow: "none", borderRadius: 0, color: "#FFF" }}
                      type="submit"
                      variant="contained">
                      Lưu và tiếp tục
                    </Button>
                  </Grid>
                </Grid>
              </form>
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
