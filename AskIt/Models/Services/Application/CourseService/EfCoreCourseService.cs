using AskIt.Models.Customizations.Errors;
using AskIt.Models.Customizations.Helpers;
using AskIt.Models.Data;
using AskIt.Models.Data.Entities;
using AskIt.Models.InputModels.CourseInputModels;
using AskIt.Models.Mappings;
using AskIt.Models.ViewModels.CourseViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AskIt.Models.Services.Application.CourseService;

public class EfCoreCourseService : ICourseService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public EfCoreCourseService(ApplicationDbContext dbContext,
        UserManager<ApplicationUser> userManager,
        IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<CourseListViewModel, CourseError>> GetCoursesAsync(int pageNumber = 1)
    {
        if(pageNumber < 1) pageNumber = 1;
        var user = await _userManager.GetUserFromHttpContextAsync(_httpContextAccessor);
        var totalCourses = await _dbContext.Courses.CountAsync();
        var coursesQuery =  _dbContext.Courses;
        var courses = await coursesQuery
            .Skip((pageNumber - 1) * 10)
            .Take(10)
            .ToListAsync();
        
        List<CourseViewModel> listCourseViewModel = courses.Select( c => c.ToModel()).ToList();
        var totalPages = (int)Math.Ceiling((double)totalCourses / 10);

        var response = new CourseListViewModel
        {
            Courses = listCourseViewModel,
            CurrentPage = pageNumber,
            TotalPages = totalPages
        };

        return Result<CourseListViewModel, CourseError>.Success(response);
    }

    public async Task<Result<CourseViewModel, CourseError>> CreateCourseAsync(CreateCourseInputModel inputModel)
    {
        var user = await _userManager.GetUserFromHttpContextAsync(_httpContextAccessor);
        Course? course = inputModel.ToEntity();

        await _dbContext.Courses.AddAsync(course);
        var result = await _dbContext.SaveChangesAsync();

        if(result <= 0)
            return Result<CourseViewModel, CourseError>.Failure(CourseError.CreationFailed);
        return Result<CourseViewModel, CourseError>.Success(course.ToModel());
    }
}
