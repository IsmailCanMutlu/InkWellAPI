namespace RestApiChallenge.Models;

public class Book
{
    public int Id { get; set; }
    public string? BookName { get; set; }
    public string? AuthorName { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public ICollection<UserBook> UserBooks { get; set; } = new List<UserBook>();
}