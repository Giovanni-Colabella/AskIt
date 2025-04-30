using System.ComponentModel.DataAnnotations;
using AskIt.Models.Customizations.DataAnnotations;

namespace AskIt.Models.InputModels.CourseInputModels;

public record class EditCourseInputModel
{
    [Required(ErrorMessage = "L'Id del corso è obbligatorio.")]
    public int CourseId { get; init; }

    [Required(ErrorMessage = "Il titolo del corso è obbligatorio.")]
    [MaxLength(100, ErrorMessage = "Il titolo del corso non può superare i 100 caratteri.")]
    [MinLength(5, ErrorMessage = "Il titolo del corso deve contenere almeno 5 caratteri.")]
    [Display(Name = "Titolo del Corso")]
    public string CourseName { get; init; } = string.Empty;

    [Required(ErrorMessage = "La descrizione del corso è obbligatoria.")]
    [MaxLength(5000, ErrorMessage = "La descrizione del corso non può superare i 5000 caratteri.")]
    [MinLength(10, ErrorMessage = "La descrizione del corso deve contenere almeno 10 caratteri.")]
    [Display(Name = "Descrizione del Corso")]
    public string CourseDescription { get; init; } = string.Empty;

    [Required(ErrorMessage = "Il prezzo del corso è obbligatorio.")]
    [Range(3, 5000, ErrorMessage = "Il prezzo del corso deve essere compreso tra 3 e 5000 euro.")]
    [DataType(DataType.Currency, ErrorMessage = "Il prezzo del corso non è valido.")]
    [Display(Name = "Prezzo del Corso")]
    public decimal Price { get; init; } = 0.0m;

    [ValidImage(ErrorMessage = "Il file caricato non è un'immagine valida")]
    public IFormFile? CourseImage { get; init; }
}
