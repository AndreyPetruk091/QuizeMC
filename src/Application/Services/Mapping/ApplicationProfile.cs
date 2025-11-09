using AutoMapper;
using QuizeMC.Application.Models.Admin;
using QuizeMC.Application.Models.Answer;
using QuizeMC.Application.Models.Category;
using QuizeMC.Application.Models.Question;
using QuizeMC.Application.Models.Quiz;
using QuizeMC.Domain.Entities;
using QuizeMC.Domain.ValueObjects;

namespace QuizeMC.Application.Services.Mapping
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            // Admin mappings
            CreateMap<Admin, AdminModel>()
                .ForMember(dest => dest.CreatedQuizzesCount, opt => opt.MapFrom(src => src.CreatedQuizzes.Count))
                .ForMember(dest => dest.CreatedCategoriesCount, opt => opt.MapFrom(src => src.CreatedCategories.Count));

            // Category mappings
            CreateMap<Category, CategoryModel>()
                .ForMember(dest => dest.QuizzesCount, opt => opt.MapFrom(src => src.Quizzes.Count))
                .ForMember(dest => dest.CreatedByAdminEmail, opt => opt.MapFrom(src => src.CreatedByAdmin.Email.Value));

            // Quiz mappings
            CreateMap<Quiz, QuizModel>()
                .ForMember(dest => dest.QuestionsCount, opt => opt.MapFrom(src => src.Questions.Count))
                .ForMember(dest => dest.CreatedByAdminEmail, opt => opt.MapFrom(src => src.CreatedByAdmin.Email.Value))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name.Value));

            // Question mappings
            CreateMap<Question, QuestionModel>()
                .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Answers));

            CreateMap<Question, QuestionWithAnswersModel>()
                .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Answers));

            // Answer mappings
            CreateMap<Answer, AnswerModel>();
            CreateMap<Answer, AnswerCreateModel>();

            // Reverse mappings for updates
            CreateMap<CategoryUpdateModel, Category>();
            CreateMap<QuizUpdateModel, Quiz>();
            CreateMap<QuestionUpdateModel, Question>();
            CreateMap<AnswerUpdateModel, Answer>();
        }
    }
}