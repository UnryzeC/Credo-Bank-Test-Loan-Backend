using LoanService.Application.Loan.Queries.Common.Dtos;
using LoanService.Application.Loan.Queries.Common.Mappings;

namespace LoanService.Application.Loan.Queries.GetLoanRequests;

public class GetLoanRequestsQueryResponse
{
    public GetLoanRequestsQueryResponse( List<Core.Loan.LoanRequest> loanRequests )
    {
        if ( loanRequests is null ) { return; }

        LoanRequests = loanRequests.Select( LoanRequestMap.MapLoanRequestEntityToLoanRequestDto ).ToArray( );
    }

    public LoanRequest[] LoanRequests { get; set; } = Array.Empty<LoanRequest>( );
}
