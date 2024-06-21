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

function Courses() {
  const { getCourseActionGroupByTopic } = getAllCourses();
  const [allCourses, setAllCourses] = useState([]);

  let i = 0;
  const [isShowFilter, setIsShowFilter] = useState(false);

  useEffect(() => {
    const fetchCoursesData = async () => {
      try {
        const res = await getCourseActionGroupByTopic();
        setAllCourses(res?.data);
      } catch (err) {
        throw new Error(err);
      }
    };
       fetchCoursesData();
  }, []);
  useEffect(() => {
    console.log(allCourses);
  }, [allCourses]);

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
             topics={allCourses}
            handleClickFilter={handleClickFilter}
          />
        </div>
        <Box>
        {allCourses.length > 0 && allCourses.map((topic, index) => {
  return (
    <Box
      key={index}
      sx={{
        margin: "auto",
        backgroundColor: index % 2 === 0 ? "#F5F7FA" : "white",
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
          {topic.topicName}
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
            {topic.source.map((item, i) => {
              if (item?.status === 1) {
                return (
                  <SwiperSlide key={i}>
                    <CardCourse
                      path={`/course-detail/${topic.topicName}/${encodeURIComponent(item?.title)}/${item.id}`}
                      title={item?.title}
                      category={item?.topics}
                      price={item?.price === 0 ? "Miễn phí" : item?.price}
                      image={item?.thumbnail ? item?.thumbnail : "https://bom.so/vV4j7x"}
                    />
                  </SwiperSlide>
                );
              }
            })}
          </Swiper>
          <ButtonCustomize text="Xem tất cả" navigateTo={`/category/${topic.topicName}`} />
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