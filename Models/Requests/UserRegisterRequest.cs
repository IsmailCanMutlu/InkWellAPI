namespace RestApiChallenge.Models.Requests;

public class UserRegisterRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
}