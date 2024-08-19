using LoanService.Application.Loan.Queries.Common.Dtos;
using LoanService.Application.Loan.Queries.Common.Mappings;

namespace LoanService.Application.Loan.Queries.GetUserActiveLoans;

public class GetUserActiveLoansQueryResponse
{
    public GetUserActiveLoansQueryResponse( List<Core.Loan.LoanRequest> loanRequests )
    {
        if ( loanRequests is null ) { return; }

        LoanRequests = loanRequests.Select( LoanRequestMap.MapLoanRequestEntityToLoanRequestDto ).ToArray( );
    }

    public LoanRequest[] LoanRequests { get; set; } = Array.Empty<LoanRequest>( );
}
