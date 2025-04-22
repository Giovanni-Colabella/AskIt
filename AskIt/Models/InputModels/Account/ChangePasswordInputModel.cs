using System.ComponentModel.DataAnnotations;

namespace AskIt.Models.InputModels;

public record class ChangePasswordInputModel
{
    [Required(ErrorMessage = "Il campo Password è obbligatorio")]
    public string OldPassword { get; init; } = string.Empty;


    [Required(ErrorMessage = "Il campo Nuova Password è obbligatorio")]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]).{8,}$",
            ErrorMessage = "La nuova password deve contenere almeno 8 caratteri, una lettera maiuscola, un numero e un carattere speciale.")]
    public string NewPassword { get; init; } = string.Empty;


    [Required(ErrorMessage = "Il campo Conferma Password è obbligatorio")]
    [Compare(nameof(NewPassword), ErrorMessage = "Le password non coincidono")]
    public string ConfirmPassword { get; init; } = string.Empty;
}
