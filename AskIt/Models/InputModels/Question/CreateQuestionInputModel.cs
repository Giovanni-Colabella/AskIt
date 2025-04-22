using System.ComponentModel.DataAnnotations;

namespace AskIt.Models.InputModels.Question;

public record class CreateQuestionInputModel
{
    [Required(ErrorMessage = "Il titolo è obbligatorio")]
    [StringLength(100, ErrorMessage = "Il titolo non può superare i 100 caratteri")]
    [MinLength(5, ErrorMessage = "Il titolo deve avere almeno 5 caratteri")]
    public string Title { get; init;} = string.Empty;

    [Required(ErrorMessage = "Il corpo della domanda è obbligatorio")]
    [StringLength(5000, ErrorMessage = "Il corpo della domanda non può superare i 5000 caratteri")]
    [MinLength(10, ErrorMessage = "Il corpo della domanda deve avere almeno 10 caratteri")]
    public string Body { get; init;} = string.Empty;

    [Required(ErrorMessage = "Le etichette sono obbligatorie")]
    [MinLength(1, ErrorMessage = "Devi fornire almeno un'etichetta")]
    public string[] Tags { get; init;} = Array.Empty<string>();
}
