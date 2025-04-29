namespace AskIt.Models.ViewModels.ForumViewModels
{
    public class ForumViewModel
    {
        public List<QuestionViewModel> Questions { get; set; } = new();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; } 
    }
}
