using Ardalis.Specification;

using LoanService.Core.Loan.Specifications;

using MediatR;

namespace LoanService.Application.Loan.Queries.GetActiveLoanRequests;

public class GetLoanActiveRequestsQueryHandler : IRequestHandler<GetLoanActiveRequestsQuery, GetLoanActiveRequestsQueryResponse>
{
    private readonly IReadRepositoryBase<Core.Loan.LoanRequest> _loanReadRepository;

    public GetLoanActiveRequestsQueryHandler( IReadRepositoryBase<Core.Loan.LoanRequest> loanReadRepository )
    {
        _loanReadRepository = loanReadRepository;
    }

    public async Task<GetLoanActiveRequestsQueryResponse> Handle( GetLoanActiveRequestsQuery query, CancellationToken cancellationToken )
    {
        var activeLoanRequests = await _loanReadRepository.ListAsync( new GetActiveLoanRequestsSpec( ), cancellationToken );

        var activeLoansResponse = new GetLoanActiveRequestsQueryResponse( activeLoanRequests );

        return activeLoansResponse;
    }
}
