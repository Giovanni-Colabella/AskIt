namespace AskIt.Models.Customizations.Exceptions.Image;

public class ImagePersistenceException : Exception
{
    public ImagePersistenceException(Exception? innerException) : base("Impossibile salvare l'immagine.", innerException)
    {
    }
}
