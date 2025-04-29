namespace AskIt.Models.ViewModels.CourseViewModels;

public class CourseListViewModel
{
    public List<CourseViewModel> Courses { get; set; } = new();
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; } = 1;
}
