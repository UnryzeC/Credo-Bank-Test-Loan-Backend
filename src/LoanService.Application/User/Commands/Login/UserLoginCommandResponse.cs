namespace LoanService.Application.User.Commands.Login;

public record UserLoginCommandResponse( User user, string token );

public record User( Guid userId, string fullName );
