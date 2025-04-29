using System.ComponentModel.DataAnnotations;

namespace AskIt.Models.InputModels.AccountModels;

public record class LoginInputModel
{
    [Required(ErrorMessage = "Il campo 'Email' è obbligatorio")]
    [EmailAddress(ErrorMessage = "Il campo 'Email' non è un indirizzo valido")]
    public string Email { get; init; } = "";
    [Required(ErrorMessage = "Il campo 'Password' è obbligatorio")]
    public string Password { get; init; } = "";
}
