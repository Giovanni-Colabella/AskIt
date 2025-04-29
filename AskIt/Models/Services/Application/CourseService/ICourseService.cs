using AskIt.Models.Customizations.Errors;
using AskIt.Models.Customizations.Helpers;
using AskIt.Models.InputModels.CourseInputModels;
using AskIt.Models.ViewModels.CourseViewModels;

namespace AskIt.Models.Services.Application.CourseService;

public interface ICourseService
{
    Task<Result<CourseListViewModel, CourseError>> GetCoursesAsync(int pageNumber);
    Task<Result<CourseViewModel, CourseError>> CreateCourseAsync(CreateCourseInputModel inputModel);
}
