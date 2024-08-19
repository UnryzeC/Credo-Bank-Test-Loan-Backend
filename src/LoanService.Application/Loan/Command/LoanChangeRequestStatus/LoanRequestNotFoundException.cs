using LoanService.Application.Common.Exceptions;

namespace LoanService.Application.Loan.Command.ChangeLoanRequestStatus;

public class LoanRequestNotFoundException : BaseException
{
    public LoanRequestNotFoundException( string message ) : base( message ) { }
}
