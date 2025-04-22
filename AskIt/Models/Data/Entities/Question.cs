using System.ComponentModel.DataAnnotations;
using AskIt.Models.Enums;

namespace AskIt.Models.Data.Entities;

public class Question
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public int Likes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string AuthorId { get; set; } = string.Empty;
    public ApplicationUser Author { get; set; } = new ApplicationUser();
    public ICollection<Answer> Answers { get; set; } = new List<Answer>();
    public QuestionStatus Status { get; set; }
}
