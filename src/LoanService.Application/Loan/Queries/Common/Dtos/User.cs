using LoanService.Core.User;

namespace LoanService.Application.Loan.Queries.Common.Dtos;

public record User
{
    public Guid UserId { get; set; }
    public string Fullname { get; set; }

    public User( UserEntity userEntity )
    {
        UserId = userEntity.Id;
        Fullname = $"{userEntity.Firstname} {userEntity.Lastname}";
    }
};
