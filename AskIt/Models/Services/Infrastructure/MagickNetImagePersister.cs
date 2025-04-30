using AskIt.Models.Customizations.Exceptions.Image;
using ImageMagick;

namespace AskIt.Models.Services.Infrastructure;

public class MagickNetImagePersister : IImagePersister
{
    private readonly IWebHostEnvironment _env;
    private readonly SemaphoreSlim _semaphore;
    private const long MaxImageSize = 2 * 1024 * 1024; // 2 MB
    public MagickNetImagePersister(IWebHostEnvironment env)
    {
        _env = env;
        _semaphore = new SemaphoreSlim(2);
        ResourceLimits.Height = 4000;
        ResourceLimits.Width = 4000;
    }

    public async Task<string> SaveCourseImageAsync(int courseId, IFormFile formFile)
    {
        // Controlla se il file è valido
        if (formFile == null || formFile.Length == 0)
        {
            throw new InvalidImageFormatException();
        }

        if(formFile.Length > MaxImageSize)
        {
            throw new InvalidImageFormatException();
        }

        // Controllo MIME type
        string[] validMimeTypes = { "image/jpeg", "image/png" };
        if (!validMimeTypes.Contains(formFile.ContentType))
        {
            throw new InvalidImageFormatException();
        }

        await _semaphore.WaitAsync();
        try
        {
            // Prova a salvare l'immagine 
            string path = $"/Courses/{courseId}.jpg";
            string physicalPath = Path.Combine(_env.WebRootPath, "Courses", $"{courseId}.jpg");

            using Stream inputStream = formFile.OpenReadStream();
            using MagickImage image = new MagickImage(inputStream);

            uint width = 300;
            uint height = 300;

            MagickGeometry geometry = new(width, height)
            {
                FillArea = true,
            };
            image.Resize(geometry);
            image.Crop(width, height, Gravity.Northwest);

            image.Quality = 70;
            image.Write(physicalPath, MagickFormat.Jpg);

            // Restituisci il percorso relativo dell'immagine
            return path;
        }
        catch (Exception exc)
        {
            throw new ImagePersistenceException(exc);
        }
        finally
        {
            _semaphore.Release();
        }
    }
}
