using FluentValidation;

using LoanService.Core.Loan;

using MediatR;

namespace LoanService.Application.Loan.Command.CreateLoanRequest;

public record LoanCreateRequestCommand : IRequest<Unit>
{
    public LoanType Type { get; set; } = default!;
    public decimal RequestedAmount { get; set; } = default!;
    public CurrencyCode Currency { get; set; } = default!;
    public int LoanPeriod { get; set; } = default!; // months 
    public Guid UserId { get; set; } = Guid.Empty;
}

public class CreateLoanRequestCommandValidator : AbstractValidator<LoanCreateRequestCommand>
{
    public CreateLoanRequestCommandValidator( )
    {
        RuleFor( command => command.Type ).NotEmpty( ).IsInEnum( );

        RuleFor( command => command.RequestedAmount ).NotEmpty( ).GreaterThan( 0 );

        RuleFor( command => command.Currency ).NotEmpty( ).IsInEnum( );

        RuleFor( command => command.LoanPeriod ).NotEmpty( ).GreaterThan( 0 );

        RuleFor( command => command.UserId ).NotEmpty( );
    }
}
