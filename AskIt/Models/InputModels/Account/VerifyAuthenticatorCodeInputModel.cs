using System.ComponentModel.DataAnnotations;

namespace AskIt.Models.InputModels;

public record class VerifyAuthenticatorCodeInputModel
{
    [Required(ErrorMessage = "Il codice di verifica è obbligatorio")]
    [Display(Name = "Codice di verifica")]
    public string Code { get; set; } = string.Empty;
}
