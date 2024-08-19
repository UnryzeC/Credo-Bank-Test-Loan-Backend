using LoanService.Application.Common.Exceptions;

namespace LoanService.Application.User.Commands.Register;

public class UserAlreadyExistsException : BaseException
{
    public UserAlreadyExistsException( string message ) : base( message ) { }
}
