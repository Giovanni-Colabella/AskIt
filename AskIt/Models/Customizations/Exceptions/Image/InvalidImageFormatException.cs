namespace AskIt.Models.Customizations.Exceptions.Image;

public class InvalidImageFormatException : Exception
{
    public InvalidImageFormatException() : base("Il file caricato non è un'immagine valida.")
    {
    }

}
