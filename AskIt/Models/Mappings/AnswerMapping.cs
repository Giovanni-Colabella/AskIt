using AskIt.Models.Data.Entities;
using AskIt.Models.InputModels.ForumInputModels;
using AskIt.Models.ViewModels.ForumViewModels;

namespace AskIt.Models.Mappings;

public static class AnswerMapping
{
    public static AnswerViewModel ToModel(this Answer answer)
    {
        return new AnswerViewModel{
            Id = answer.Id,
            Body = answer.Body,
            CreatedAt = answer.CreatedAt,
            UpdatedAt = answer.UpdatedAt,
            AuthorId = answer.AuthorId
        };
    }

    public static Answer ToEntity(this AnswerViewModel model)
    {
        return new Answer
        {
            Id = model.Id,
            Body = model.Body,
            CreatedAt = model.CreatedAt,
            UpdatedAt = model.UpdatedAt,
            AuthorId = model.AuthorId,
            QuestionId = model.QuestionId
        };
    }

    public static Answer ToEntity(this CreateAnswerInputModel model, string authorId = "Anonimo")
    {
        return new Answer
        {
            Body = model.Body,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            AuthorId = authorId,
            QuestionId = model.QuestionId
        };
    }
}
