using System.ComponentModel.DataAnnotations;

namespace AskIt.Models.InputModels.ForumInputModels;

public record class CreateQuestionInputModel
{
    [Required(ErrorMessage = "Il titolo è obbligatorio.")]
    [StringLength(100, ErrorMessage = "Il titolo non può superare i 100 caratteri.")]
    [MinLength(5, ErrorMessage = "Il titolo deve contenere almeno 5 caratteri.")]
    public string Title { get; init; } = string.Empty;

    [Required(ErrorMessage = "Il corpo della domanda è obbligatorio.")]
    [StringLength(1000, ErrorMessage = "Il corpo della domanda non può superare i 1000 caratteri.")]
    [MinLength(10, ErrorMessage = "Il corpo della domanda deve contenere almeno 10 caratteri.")]
    public string Body { get; init; } = string.Empty;

}
