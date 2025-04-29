using System.ComponentModel.DataAnnotations;

namespace AskIt.Models.InputModels
{
    public record class RegisterInputModel
    {
        [Required(ErrorMessage = "Il campo Username è obbligatorio")]
        [MinLength(3, ErrorMessage = "Il campo Username deve contenere minimo 3 caratteri")]
        [MaxLength(20, ErrorMessage = "Il campo Username non può contenere più di 20 caratteri")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Il campo Username deve contenere solo lettere")]
        public string Username { get; init; } = "";


        [Required(ErrorMessage = "Il campo Email è obbligatorio")]
        [EmailAddress(ErrorMessage = "Il campo Email deve essere un indirizzo valido")]
        public string Email { get; init; } = "";


        [Required(ErrorMessage = "Il campo Password è obbligatorio")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]).{8,}$",
            ErrorMessage = "La password deve contenere almeno 8 caratteri, una lettera maiuscola, un numero e un carattere speciale.")]
        public string Password { get; init; } = "";


        [Required(ErrorMessage = "Il campo Conferma Password è obbligatorio")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage = "Le password non coincidono")]
        public string ConfermaPassword { get; init; } = "";
    }
}
