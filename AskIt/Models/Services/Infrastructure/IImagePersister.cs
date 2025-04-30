namespace AskIt.Models.Services.Infrastructure;

public interface IImagePersister
{
    /// <returns>URL dell'immagine</returns>
    Task<string> SaveCourseImageAsync(int courseId, IFormFile formFile);
}
