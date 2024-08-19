using FluentValidation;

using MediatR;

namespace LoanService.Application.User.Commands.Register;

public record UserRegisterCommand : IRequest<UserRegisterCommandResponse>
{
    public string Firstname { get; set; } = null!;
    public string Lastname { get; set; } = null!;
    public string IdNumber { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime DateOfBirth { get; set; } = default!;
    public string Password { get; set; } = null!;
}

public class CreateUserCommandValidator : AbstractValidator<UserRegisterCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor( u => u.Firstname ).NotEmpty( ).MaximumLength( 50 );

        RuleFor( u => u.Lastname ).NotEmpty( ).MaximumLength( 50 );

        RuleFor( u => u.IdNumber ).NotEmpty( ).Length( 11 );

        RuleFor( u => u.Email ).NotEmpty( ).EmailAddress( );

        RuleFor( u => u.DateOfBirth ).LessThan( DateTime.Today.AddYears( -18 ) ).NotEmpty( );

        RuleFor( u => u.Password )
            .NotEmpty( )
            .Matches( "^(?=.*[A-Za-z])(?=.*[^A-Za-z0-9])(?=.*[a-z])(?=.*[A-Z]).{8,}$" )
            .WithMessage( "Password must be at least 8 characters long and include uppercase, lowercase, and special character." )
            .NotEqual( u => u.Email )
            .WithMessage( "Password cannot be the same as the email." )
            .NotEqual( u => u.Firstname )
            .WithMessage( "Password cannot be the same as the firstname." )
            .NotEqual( u => u.Lastname )
            .WithMessage( "Password cannot be the same as the lastname." );
    }
}
