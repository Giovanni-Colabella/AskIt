using System.ComponentModel.DataAnnotations;
using AskIt.Models.Enums;

namespace AskIt.Models.Data.Entities;

public class Course
{
    [Key]
    public int Id { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public string CourseDescription { get; set; } = string.Empty;
    public string? CourseImage { get; set; } 
    [Range(0, 5000, ErrorMessage = "Il prezzo deve essere compreso tra 3 e 5000 euro.")]
    public decimal Price { get; set; } = 0;
    public string AuthorId { get; set; } 
    public ApplicationUser Author { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public CourseStatus CourseStatus { get; private set; } 


    public Course(string CourseName, string AuthorId, decimal Price)
    {
        this.CourseName = CourseName;
        this.Price = Price;
        this.AuthorId = AuthorId;
    }

    /// <summary>
    /// Metodo per cambiare lo stato di un corso.
    /// /// Permette di cambiare lo stato del corso in base al nuovo stato passato come parametro.
    /// /// E' necessario aggiornare il db dopo aver chiamato questo metodo per persistere la modifica.
    /// </summary>
    /// <param name="newCourseStatus"></param>
    public void ChangeCourseStatus(CourseStatus newCourseStatus)
    {
        CourseStatus = newCourseStatus;
    }
}
