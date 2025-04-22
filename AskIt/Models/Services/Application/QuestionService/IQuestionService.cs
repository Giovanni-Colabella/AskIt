using AskIt.Models.Customizations.Errors;
using AskIt.Models.Customizations.Helpers;
using AskIt.Models.InputModels.Question;
using AskIt.Models.ViewModels.Question;

namespace AskIt.Models.Services.Application.QuestionService;

public interface IQuestionService
{
    Task<Result<CreateQuestionInputModel, QuestionError>> CreateQuestionAsync(CreateQuestionInputModel model);
    Task<Result<IEnumerable<QuestionViewModel>, QuestionError>> GetQuestionsAsync();
    Task<Result<QuestionError>> DeleteQuestionAsync(int id);
    Task<Result<QuestionDetailViewModel, QuestionError>> GetQuestionDetailAsync(int id);
}
