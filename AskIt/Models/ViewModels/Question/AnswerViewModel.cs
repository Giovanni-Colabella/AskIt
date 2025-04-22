namespace AskIt.Models.ViewModels.Question
{
    public class AnswerViewModel
    {
        public int Id { get; set; }
        public string Body { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string AuthorId { get; set; } = string.Empty;
    }
}
