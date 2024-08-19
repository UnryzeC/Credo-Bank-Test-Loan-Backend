using Ardalis.Specification;

namespace LoanService.Core.Loan.Specifications;

public class GetActiveLoanRequestsSpec : Specification<LoanRequest>
{
    public GetActiveLoanRequestsSpec( )
    {
        Query
            .AsNoTracking( )
            .Include( x => x.User )
            .Include( x => x.LoanOfficer )
            .Where( x => x.Status != LoanStatus.Rejected && x.Status != LoanStatus.Approved )
            .OrderByDescending( x => x.CreatedDateTime );
    }
}
