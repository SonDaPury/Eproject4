import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import { Swiper, SwiperSlide } from "swiper/react";
import { Navigation } from "swiper/modules";
import theme from "@eproject4/theme";
import CardCourse from "@eproject4/components/CardCourse.jsx";
import "swiper/css";
import "swiper/css/navigation";
import ButtonCustomize from "@eproject4/components/ButtonCustomize";
import { useEffect, useState } from "react";
import FilterPanel from "@eproject4/components/FilterPanel";
import { getAllCourses } from "@eproject4/services/courses.service";
import { getAllTopics } from "@eproject4/services/topic.service";
import { getSubTopics } from "@eproject4/services/subTopic.service";

function Courses() {
  const { getCoursesAction } = getAllCourses();
  const { getAllTopicsAction } = getAllTopics();
  const { getSubTopicsAction } = getSubTopics();
  const [categories, setCategories] = useState([]);
  const [subcategories, setSubcategories] = useState([]);
  const [allCourses, setAllCourses] = useState([]);
  const [newItems, setNewItems] = useState([]);

  let i = 0;
  const courseTopics = [];
  const [isShowFilter, setIsShowFilter] = useState(false);

  //lọc

  // Hàm removeDuplicate nhận vào một mảng và trả về một mảng mới với các phần tử duy nhất
  const removeDuplicate = (arr) => {
    return Array.from(new Set(arr));
  };

  // Duyệt qua từng khóa học trong mảng newItems và thêm chủ đề của nó vào mảng courseTopics
  newItems.forEach((course) => {
    courseTopics.push(course?.topics);
  });
  // Loại bỏ các chủ đề trùng lặp từ courseTopics và lưu vào mảng topics
  const topics = removeDuplicate(courseTopics);

  //===============================

  useEffect(() => {
    const fetchCoursesData = async () => {
      try {
        const res = await getCoursesAction();
        setAllCourses(res?.data);
      } catch (err) {
        throw new Error(err);
      }
    };
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
    fetchCoursesData();
  }, []);

  // Join subtopic, topics and courses
  const joinSubtopicToCourses = (courses, subCategories) => {
    return courses?.map((course) => {
      const subTopics = subCategories.find(
        (subCategory) => subCategory.id === course?.source?.subTopicId
      );
      return {
        ...course,
        subTopics: subTopics ? subTopics?.subTopicName : "Unknown",
      };
    });
  };
  const joinTopicToCourses = (courses, categories) => {
    return courses?.map((course) => {
      const topics = categories.find(
        (category) => category.id === course?.topicId
      );
      return {
        ...course,
        topics: topics ? topics?.topicName : "Unknown",
      };
    });
  };
  useEffect(() => {
    if (allCourses.length && categories.length && subcategories.length) {
      const combinedData = joinTopicToCourses(
        joinSubtopicToCourses(allCourses, subcategories),
        categories
      );
      setNewItems(combinedData);
    }
  }, [allCourses, categories, subcategories]);

  const handleClickFilter = () => {
    setIsShowFilter(!isShowFilter);
  };

  return (
    <Box sx={{ marginTop: "56px" }}>
      <Box>
        {" "}
        <div>
          <FilterPanel
            isShowFilter={isShowFilter}
            topics={topics}
            handleClickFilter={handleClickFilter}
          />
        </div>
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
                      {newItems.map((item, i) => {
                        if (
                          item?.topics === topic &&
                          item?.source?.status === 1
                        ) {
                          return (
                            <SwiperSlide key={i}>
                              <CardCourse
                                path={`/course-detail/${item?.topics}/${encodeURIComponent(item?.source?.title)}/${item?.source.id}`}
                                title={item?.source?.title}
                                category={item?.topics}
                                price={
                                  item?.source?.price === 0
                                    ? "Miễn phí"
                                    : item?.source?.price
                                }
                                image={
                                  item?.source?.thumbnail
                                    ? item?.source?.thumbnail
                                    : "https://bom.so/vV4j7x"
                                }
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
