using Ardalis.Specification;

using LoanService.Core.Loan.Specifications;

using MediatR;

namespace LoanService.Application.Loan.Queries.GetUserActiveLoans;

public class GetUserActiveLoansQueryHandler
 : IRequestHandler<GetUserActiveLoansQuery, GetUserActiveLoansQueryResponse>
{
    private readonly IReadRepositoryBase<Core.Loan.LoanRequest> _loanReadRepository;

    public GetUserActiveLoansQueryHandler( IReadRepositoryBase<Core.Loan.LoanRequest> loanReadRepository )
    {
        _loanReadRepository = loanReadRepository;
    }

    public async Task<GetUserActiveLoansQueryResponse> Handle( GetUserActiveLoansQuery query, CancellationToken cancellationToken )
    {
        var activeLoanRequests = await _loanReadRepository.ListAsync( new GetActiveLoanRequestsByUserIdSpec( query.userId ), cancellationToken );

        var activeLoansResponse = new GetUserActiveLoansQueryResponse( activeLoanRequests );

        return activeLoansResponse;
    }
}
