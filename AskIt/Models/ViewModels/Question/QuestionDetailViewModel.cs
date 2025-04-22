namespace AskIt.Models.ViewModels.Question
{
    public class QuestionDetailViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public int Likes { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string AuthorId { get; set; } = string.Empty;
        public List<AnswerViewModel> Answers { get; set; } = new();

    }
}
