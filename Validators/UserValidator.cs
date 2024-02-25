using FluentValidation;
using RestApiChallenge.Models;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(user => user.Username).NotEmpty().WithMessage("Username is required.");
        RuleFor(user => user.Password).MinimumLength(8).WithMessage("Password must be at least 8 characters.");
        RuleFor(user => user.Name).NotEmpty().WithMessage("Name is required.");
        RuleFor(user => user.Surname).NotEmpty().WithMessage("Surname is required.");
    }
}
