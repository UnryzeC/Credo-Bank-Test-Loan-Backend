namespace LoanService.Application.User.Commands.Register;

public record UserRegisterCommandResponse( User user, string token );

public record User( Guid userId, string fullName );
