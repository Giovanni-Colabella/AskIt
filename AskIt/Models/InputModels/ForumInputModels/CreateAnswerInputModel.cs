using System.ComponentModel.DataAnnotations;

namespace AskIt.Models.InputModels.ForumInputModels;

public record class CreateAnswerInputModel
{
    [Required(ErrorMessage = "La risposta deve avere un corpo.")]
    [StringLength(1000, ErrorMessage = "La risposta non può superare i 1000 caratteri.")]
    [MinLength(1, ErrorMessage = "La risposta deve avere almeno 1 carattere.")]
    public string Body { get; init; } = string.Empty;

    [Required(ErrorMessage = "Il campo ID della domanda è obbligatorio")]
    public int QuestionId { get; init; } = 0;

}
