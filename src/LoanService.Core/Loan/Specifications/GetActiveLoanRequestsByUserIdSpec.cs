using Ardalis.Specification;

namespace LoanService.Core.Loan.Specifications;

public class GetActiveLoanRequestsByUserIdSpec : GetActiveLoanRequestsSpec
{
    public GetActiveLoanRequestsByUserIdSpec( Guid userId )
    {
        Query.Where( x => x.UserId == userId );
    }
}
