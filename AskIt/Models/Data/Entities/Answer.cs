using System.ComponentModel.DataAnnotations;

namespace AskIt.Models.Data.Entities;

public class Answer
{
    [Key]
    public int Id { get; set; }
    public string Body { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public int QuestionId { get; set; }
    public Question Question { get; set; } = new Question();
    public string AuthorId { get; set; } = string.Empty;
    public ApplicationUser Author { get; set; } = new ApplicationUser();
}
