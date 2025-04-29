using AskIt.Models.Enums;

namespace AskIt.Models.ViewModels.CourseViewModels;

public class CourseViewModel
{
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public string CourseDescription { get; set; } = string.Empty;
    public string? CourseImage { get; set; } 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public CourseStatus CourseStatus { get; set; } = CourseStatus.Privato;
    public decimal Price { get; set; } = 0.0m;

}
