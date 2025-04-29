using System.ComponentModel.DataAnnotations;

namespace AskIt.Models.Enums;

public enum Roles
{
    [Display(Name = "User")]
    User,
    [Display(Name = "Admin")]
    Admin,
    [Display(Name = "Docente")]
    Docente
}
