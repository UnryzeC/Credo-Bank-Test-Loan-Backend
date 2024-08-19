using Ardalis.Specification;

namespace LoanService.Core.User.Specifications;

public class GetUserByEmailSpec : Specification<UserEntity>, ISingleResultSpecification<UserEntity>
{
    public GetUserByEmailSpec( string email )
    {
        Query.AsNoTracking( ).Where( x => x.Email.ToLower( ) == email.ToLower( ) );
    }
}
