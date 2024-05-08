using AutoMapper;
using backend.Dtos;
using backend.Entities;

namespace backend.Extentions
{
    public class CreateAutoMapper : Profile
    {
        public CreateAutoMapper() { 

            CreateMap<Answer, AnswerDto>().ReverseMap();

            CreateMap<Attemp, AttempDto>().ReverseMap();

            CreateMap<Chapter, ChapterDto>().ReverseMap();

            CreateMap<Exam, ExamDto>().ReverseMap();

            CreateMap<Lesson, LessonDto>().ReverseMap();

            CreateMap<Option, OptionDto>().ReverseMap();

            CreateMap<Question, QuestionDto>().ReverseMap();

            CreateMap<QuizQuestion, QuizQuestionDto>().ReverseMap();

            CreateMap<Source, SourceDto>().ReverseMap();

            CreateMap<SubTopic, SubTopicDto>().ReverseMap();

            CreateMap<Topic, TopicDto>().ReverseMap();


        }
    }
}
