using System.ComponentModel.DataAnnotations;

namespace AskIt.Models.InputModels.CourseInputModels;

public record class CreateCourseInputModel
{
    [Required(ErrorMessage = "Il titolo del corso è obbligatorio.")]
    [MaxLength(100, ErrorMessage = "Il titolo del corso non può superare i 100 caratteri.")]
    [MinLength(5, ErrorMessage = "Il titolo del corso deve contenere almeno 5 caratteri.")]
    [RegularExpression(@"\S.*", ErrorMessage = "Il titolo del corso non può essere vuoto.")]
    [Display(Name = "Titolo del Corso")]
    public string CourseName { get; set; } = string.Empty;

    [Required(ErrorMessage = "La descrizione del corso è obbligatoria.")]
    [MaxLength(5000, ErrorMessage = "La descrizione del corso non può superare i 5000 caratteri.")]
    [MinLength(10, ErrorMessage = "La descrizione del corso deve contenere almeno 10 caratteri.")]
    [RegularExpression(@"\S.*", ErrorMessage = "Il titolo del corso non può essere vuoto.")]
    [Display(Name = "Descrizione del Corso")]
    public string CourseDescription { get; set; } = string.Empty;


    [Required(ErrorMessage = "La data di creazione del corso è obbligatoria.")]
    [DataType(DataType.DateTime, ErrorMessage = "La data di creazione del corso non è valida.")]
    [Display(Name = "Data di creazione")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required(ErrorMessage = "Il prezzo del corso è obbligatorio.")]
    [Range(3, 5000, ErrorMessage = "Il prezzo del corso deve essere compreso tra 3 e 5000 euro.")]
    [DataType(DataType.Currency, ErrorMessage = "Il prezzo del corso non è valido.")]
    [Display(Name = "Prezzo del Corso")]
    public decimal Price { get; set; } = 0.0m;

    
    public IFormFile? CourseImage { get; set; } 
}
