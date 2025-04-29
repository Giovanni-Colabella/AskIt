using System.ComponentModel.DataAnnotations;
using ImageMagick;

namespace AskIt.Models.Customizations.DataAnnotations;

public class ValidImageAttribute : ValidationAttribute
{
    private readonly long _maxFileSizeInBytes = 2 * 1024 * 1024; // 2 MB 
    private readonly string[] _allowedMimeTypes = ["image/jpeg", "image/png"];

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var file = value as IFormFile;
        if(file == null || file.Length == 0)
            return ValidationResult.Success;
        
        if(file.Length > _maxFileSizeInBytes)
            return new ValidationResult($"La dimensione dell'immagine non può superare i 2 MB");
        if(!_allowedMimeTypes.Contains(file.ContentType))
            return new ValidationResult($"Il formato dell'immagine non è supportato.");

        try 
        {
            using var image = new MagickImage(file.OpenReadStream());
            if(image.Format != MagickFormat.Jpeg && image.Format != MagickFormat.Png)
                return new ValidationResult($"Il formato dell'immagine non è supportato.");

        }
        catch (MagickMissingDelegateErrorException)
        {
            return new ValidationResult($"Il formato dell'immagine non è supportato.");
        }
        catch (Exception)
        {
            // Se si verifica un errore durante la lettura dell'immagine, è molto probabile che il file non sia valida o contenga codice malevolo.
            // In questo caso, restituiamo un errore di validazione generico.
            return new ValidationResult($"Si è verificato un errore durante la validazione dell'immagine.");
        }

        return ValidationResult.Success;
    }
}
