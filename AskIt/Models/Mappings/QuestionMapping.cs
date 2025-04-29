using AskIt.Models.Data.Entities;
using AskIt.Models.InputModels.ForumInputModels;
using AskIt.Models.ViewModels.ForumViewModels;

namespace AskIt.Models.Mappings;

public static class QuestionMapping
{
    public static QuestionViewModel ToModel(this Question question, string authorName)
    {
        return new QuestionViewModel
        {
            Id = question.Id,
            Title = question.Title,
            Body = question.Body,
            Likes = question.Likes,
            CreatedAt = question.CreatedAt,
            UpdatedAt = question.UpdatedAt,
            AuthorId = question.AuthorId,
            AuthorName = authorName,
            Answers = question.Answers?.Select(a => a.ToModel()).ToList() ?? new List<AnswerViewModel>()
        };
    }

    public static Question ToEntity(this QuestionViewModel model)
    {
        return new Question
        {
            Id = model.Id,
            Title = model.Title,
            Body = model.Body,
            Likes = model.Likes,
            CreatedAt = model.CreatedAt,
            UpdatedAt = model.UpdatedAt,
            AuthorId = model.AuthorId,
            Answers = model.Answers?.Select(a => a.ToEntity()).ToList() ?? new List<Answer>()
        };
    }

    public static Question ToEntity(this CreateQuestionInputModel model, string authorId)
    {
        return new Question
        {
            Title = model.Title,
            Body = model.Body,
            Likes = 0,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            AuthorId = authorId
        };
    }
}
