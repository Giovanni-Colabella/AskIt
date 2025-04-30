using AskIt.Models.Data.Entities;
using AskIt.Models.InputModels.CourseInputModels;
using AskIt.Models.ViewModels.CourseViewModels;

namespace AskIt.Models.Mappings;

public static class CourseMapping
{
    public static CourseViewModel ToModel(this Course course)
    {
        return new CourseViewModel
        {
            CourseId = course.Id,
            CourseName = course.CourseName,
            CourseDescription = course.CourseDescription,
            CourseImage = course.CourseImage,
            CreatedAt = course.CreatedAt,
            CourseStatus = course.CourseStatus,
            Price = course.Price
        };
    }

    public static Course ToEntity(this CreateCourseInputModel inputModel, string authordId)
    {
        return new Course(inputModel.CourseName, authordId, inputModel.Price)
        {
            CourseDescription = inputModel.CourseDescription,
            CreatedAt = DateTime.UtcNow,
        };
    }
}
