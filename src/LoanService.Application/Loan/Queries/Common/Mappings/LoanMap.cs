using LoanService.Application.Loan.Queries.Common.Dtos;

namespace LoanService.Application.Loan.Queries.Common.Mappings;

public static class LoanRequestMap
{
    public static LoanRequest MapLoanRequestEntityToLoanRequestDto( Core.Loan.LoanRequest loanRequest )
    {
        return new LoanRequest
        {
            Id = loanRequest.Id,
            Type = loanRequest.Type,
            RequestedAmount = loanRequest.RequestedAmount,
            Currency = loanRequest.Currency,
            LoanPeriod = loanRequest.LoanPeriod,
            Status = loanRequest.Status,
            User = new( loanRequest.User ),
            LoanOfficer = loanRequest.LoanOfficer is not null ? new( loanRequest.LoanOfficer ) : null
        };
    }
}
