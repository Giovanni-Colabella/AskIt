using AskIt.Models.InputModels.ForumInputModels;

namespace AskIt.Models.ViewModels.ForumViewModels;

public class QuestionViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public int Likes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string AuthorId { get; set; } = string.Empty;
    public string AuthorName { get; set; } = string.Empty;
    public List<AnswerViewModel> Answers { get; set; } = new List<AnswerViewModel>();
    public CreateAnswerInputModel AnswerInputModel { get; set; } = new();
}
