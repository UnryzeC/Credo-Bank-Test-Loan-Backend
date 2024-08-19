using LoanService.Application.Common.Exceptions;

namespace LoanService.Application.User.Commands.Login;

public class InvalidEmailOrPasswordException : BaseException
{
    public InvalidEmailOrPasswordException( string message ) : base( message ) { }
}
