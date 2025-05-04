using Domain.Entities;
using Models.Paticipiant;
using Models.Question;
using Models.Quiz;

namespace Services.Mapping
{
    public class ApplicationProfile : Profile //++
    {
        public ApplicationProfile()
        {
            CreateMap<Quiz, QuizModel>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions));

            CreateMap<Question, QuestionModel>()
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Answers.Select(a => a.Text)))
                .ForMember(dest => dest.CorrectAnswerIndex, opt => opt.MapFrom(src => src.CorrectAnswerIndex));

            CreateMap< Paticipiant, ParticipantModel>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username));
        }
    }
}