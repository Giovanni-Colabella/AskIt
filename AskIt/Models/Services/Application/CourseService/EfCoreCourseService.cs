using AskIt.Models.Customizations.Errors;
using AskIt.Models.Customizations.Helpers;
using AskIt.Models.Data;
using AskIt.Models.Data.Entities;
using AskIt.Models.InputModels.CourseInputModels;
using AskIt.Models.Mappings;
using AskIt.Models.Services.Infrastructure;
using AskIt.Models.ViewModels.CourseViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AskIt.Models.Services.Application.CourseService;

public class EfCoreCourseService : ICourseService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<EfCoreCourseService> _logger;
    private readonly IImagePersister _imagePersister;
    public EfCoreCourseService(ApplicationDbContext dbContext,
        UserManager<ApplicationUser> userManager,
        IHttpContextAccessor httpContextAccessor,
        ILogger<EfCoreCourseService> logger,
        IImagePersister imagePersister)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        _imagePersister = imagePersister;
    }

    public async Task<Result<CourseListViewModel, CourseError>> GetCoursesAsync(int pageNumber = 1)
    {
        if (pageNumber < 1) pageNumber = 1;
        var user = await _userManager.GetUserFromHttpContextAsync(_httpContextAccessor);
        var totalCourses = await _dbContext.Courses.CountAsync();
        var coursesQuery = _dbContext.Courses;
        var courses = await coursesQuery
            .Skip((pageNumber - 1) * 10)
            .Take(10)
            .ToListAsync();

        List<CourseViewModel> listCourseViewModel = courses.Select(c => c.ToModel()).ToList();
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

        // Inizia una transazione
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            Course? course = inputModel.ToEntity(user.Id);
            await _dbContext.Courses.AddAsync(course);
            var result = await _dbContext.SaveChangesAsync();

            if (result <= 0)
                return Result<CourseViewModel, CourseError>.Failure(CourseError.CreationFailed);

            // Se è presente l'immagine, la salva
            if (inputModel.CourseImage is not null)
            {
                string imagePath = await _imagePersister.SaveCourseImageAsync(course.Id, inputModel.CourseImage);
                course.CourseImage = imagePath;

                _dbContext.Courses.Update(course);
                await _dbContext.SaveChangesAsync();
            }

            // Conferma la transazione se tutto è andato a buon fine
            await transaction.CommitAsync();

            return Result<CourseViewModel, CourseError>.Success(course.ToModel());
        }
        catch
        {
            // Rollback automatico allo scope della transazione
            await transaction.RollbackAsync();
            return Result<CourseViewModel, CourseError>.Failure(CourseError.CreationFailed);
        }
    }

}
