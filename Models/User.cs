namespace RestApiChallenge.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty; 
    public string Password { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public ICollection<UserBook> UserBooks { get; set; } = new List<UserBook>();
}