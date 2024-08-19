using Ardalis.Specification;

namespace LoanService.Core.Loan.Specifications;

public class GetLoanRequestsSpec : Specification<LoanRequest>
{
    public GetLoanRequestsSpec( )
    {
        Query
            .AsNoTracking( )
            .Include( x => x.User )
            .Include( x => x.LoanOfficer )
            .OrderBy( x => x.Status );
    }
}
