using LoanService.Core.Loan;

namespace LoanService.Application.Loan.Queries.Common.Dtos;

public class LoanRequest
{
    public Guid Id { get; set; } = Guid.Empty;
    public LoanType Type { get; set; } = default!;
    public decimal RequestedAmount { get; set; } = default!;
    public CurrencyCode Currency { get; set; } = default!;
    public int LoanPeriod { get; set; } = default!; // Months 
    public LoanStatus Status { get; set; } = default!;
    public User User { get; set; } = null!;
    public User? LoanOfficer { get; set; } = null!;
}
