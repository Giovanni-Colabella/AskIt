using AskIt.Models.Data.Entities;
using AskIt.Models.InputModels.Question;
using AskIt.Models.ViewModels.Question;
using AutoMapper;

namespace AskIt.Models.Mappings;

public class QuestionProfile : Profile
{
    public QuestionProfile()
    {
        CreateMap<Question, CreateQuestionInputModel>();
        CreateMap<CreateQuestionInputModel, Question>();

        CreateMap<Question, QuestionViewModel>();
        CreateMap<QuestionViewModel, Question>();

        CreateMap<Question, QuestionDetailViewModel>();
        CreateMap<QuestionDetailViewModel, Question>();
    }
}
