using AskIt.Models.Customizations.Errors;
using AskIt.Models.Customizations.Helpers;
using AskIt.Models.InputModels.ForumInputModels;
using AskIt.Models.ViewModels.ForumViewModels;

using Microsoft.Extensions.Caching.Memory;

namespace AskIt.Models.Services.Application.ForumService;

public class MemoryCacheForumService : ICachedForumService
{
    private readonly IForumService _forumService;
    private readonly IMemoryCache _memoryCache;

    public MemoryCacheForumService(IForumService forumService, IMemoryCache memoryCache)
    {
        _forumService = forumService;
        _memoryCache = memoryCache;
    }

    public Task<Result<QuestionViewModel, AnswerError>> AddAnswerAsync(CreateAnswerInputModel model)
    {
        return _forumService.AddAnswerAsync(model);
    }

    public Task AddQuestionAsync(CreateQuestionInputModel model)
    {
        return _forumService.AddQuestionAsync(model);
    }

    public Task<bool> DeleteAnswerAsync(int id)
    {
        return _forumService.DeleteAnswerAsync(id);
    }

    public Task<bool> DeleteQuestionAsync(int questionId)
    {
        return _forumService.DeleteQuestionAsync(questionId);
    }

    public async Task<Result<QuestionViewModel, QuestionError>> GetQuestionByIdAsync(int id)
    {
        return await _forumService.GetQuestionByIdAsync(id);

    }

    public Task<Result<ForumViewModel, QuestionError>> GetQuestionsAsync(int pageNumber)
    {
        // Mettiamo in cache solo le prime 5 pagine del forum
        bool canCache = pageNumber < 5;

        if (canCache)
        {
            return _memoryCache.GetOrCreateAsync(
                "Questions",
                async entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromSeconds(60));
                    return await _forumService.GetQuestionsAsync(pageNumber);
                }
            );
        }
        return _forumService.GetQuestionsAsync(pageNumber);
    }
}
