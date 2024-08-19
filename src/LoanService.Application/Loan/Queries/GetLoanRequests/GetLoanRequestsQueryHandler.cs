using Ardalis.Specification;

using LoanService.Core.Loan.Specifications;

using MediatR;

namespace LoanService.Application.Loan.Queries.GetLoanRequests;

public class GetLoanRequestsQueryHandler : IRequestHandler<GetLoanRequestsQuery, GetLoanRequestsQueryResponse>
{
    private readonly IReadRepositoryBase<Core.Loan.LoanRequest> _loanReadRepository;

    public GetLoanRequestsQueryHandler( IReadRepositoryBase<Core.Loan.LoanRequest> loanReadRepository )
    {
        _loanReadRepository = loanReadRepository;
    }

    public async Task<GetLoanRequestsQueryResponse> Handle( GetLoanRequestsQuery query, CancellationToken cancellationToken)
    {
        var activeLoanRequests = await _loanReadRepository.ListAsync( new GetLoanRequestsSpec( ), cancellationToken );

        var activeLoansResponse = new GetLoanRequestsQueryResponse( activeLoanRequests );

        return activeLoansResponse;
    }
}
