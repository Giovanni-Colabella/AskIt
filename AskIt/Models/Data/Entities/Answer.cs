using System.ComponentModel.DataAnnotations;

namespace AskIt.Models.Data.Entities;

public class Answer
{
    [Key]
    public int Id { get; set; }
    public string Body { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public required int QuestionId { get; set; }
    public  Question Question { get; set; } = null!;
    public required string AuthorId { get; set; } 
    public ApplicationUser Author { get; set; } = null!;
}
