using FluentValidation;

using MediatR;

namespace LoanService.Application.User.Commands.Login;

public record UserLoginCommand : IRequest<UserLoginCommandResponse>
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;

}

public class UserLoginCommandValidator : AbstractValidator<UserLoginCommand>
{
    public UserLoginCommandValidator( )
    {
        RuleFor( x => x.Email ).NotEmpty( );
        RuleFor( x => x.Password ).NotEmpty( );
    }
}
