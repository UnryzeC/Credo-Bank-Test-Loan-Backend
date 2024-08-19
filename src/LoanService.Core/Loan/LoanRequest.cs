using LoanService.Core.Common;
using LoanService.Core.User;

namespace LoanService.Core.Loan;

public class LoanRequest : BaseEntity
{
    public Guid Id { get; set; } = Guid.Empty;
    public LoanType Type { get; set; } = default!;
    public decimal RequestedAmount { get; set; } = default!;
    public CurrencyCode Currency { get; set; } = default!;
    public int LoanPeriod { get; set; } = default!; // months 
    public LoanStatus Status { get; set; } = default!;

    // Foreign key to the User who made the request
    public Guid UserId { get; set; } = Guid.Empty!;
    public UserEntity User { get; set; } = null!;

    // Foreign key to the User who processed the request
    public Guid? LoanOfficerId { get; set; } = null!;
    public UserEntity? LoanOfficer { get; set; } = null!;

}

public enum LoanType
{
    Fast = 1,
    Auto = 2,
    Installment = 3
}

public enum LoanStatus
{
    Sent = 1,
    Processing = 2,
    Approved = 3,
    Rejected = 4
}

public enum CurrencyCode
{
    GEL = 100,
    USD = 101,
    EUR = 102
}
