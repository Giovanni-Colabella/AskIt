using AskIt.Models.Customizations.Errors;
using AskIt.Models.Customizations.Helpers;
using AskIt.Models.InputModels.ForumInputModels;
using AskIt.Models.ViewModels.ForumViewModels;

namespace AskIt.Models.Services.Application.ForumService;

public interface IForumService
{
    Task<Result<ForumViewModel, QuestionError>> GetQuestionsAsync(int pageNumber);
    Task<Result<QuestionViewModel, QuestionError>> GetQuestionByIdAsync(int id);
    Task AddQuestionAsync(CreateQuestionInputModel model);
    Task<Result<QuestionViewModel, AnswerError>> AddAnswerAsync(CreateAnswerInputModel model);
    Task<bool> DeleteAnswerAsync(int id);
    Task<bool> DeleteQuestionAsync(int questionId);
}   
