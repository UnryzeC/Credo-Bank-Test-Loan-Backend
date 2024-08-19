using LoanService.Application.Common.Exceptions;

namespace LoanService.Application.Loan.Command.CreateLoanRequest;

public class UserDoesNotExistException : BaseException
{
    public UserDoesNotExistException( string message ) : base( message ) { }
}
