using Ardalis.Specification;

using LoanService.Core.Loan;
using LoanService.Core.User;

using MediatR;

namespace LoanService.Application.Loan.Command.CreateLoanRequest;

public class LoanCreateRequestCommandHandler : IRequestHandler<LoanCreateRequestCommand, Unit>
{
    private readonly IRepositoryBase<LoanRequest> _loanRepository;
    private readonly IReadRepositoryBase<UserEntity> _userReadRepository;

    public LoanCreateRequestCommandHandler( IRepositoryBase<LoanRequest> loanRepository, IReadRepositoryBase<UserEntity> userReadRepository )
    {
        _loanRepository = loanRepository;
        _userReadRepository = userReadRepository;
    }

    public async Task<Unit> Handle( LoanCreateRequestCommand request, CancellationToken cancellationToken )
    {
        var userExists = await _userReadRepository.GetByIdAsync( request.UserId, cancellationToken );

        if ( userExists is null )
        {
            throw new UserDoesNotExistException( $"User #{request.UserId} does not exist!" );
        }

        var loanRequest = new LoanRequest
        {
            Type = request.Type,
            RequestedAmount = request.RequestedAmount,
            Currency = request.Currency,
            LoanPeriod = request.LoanPeriod,
            Status = LoanStatus.Sent,
            UserId = request.UserId
        };

        await _loanRepository.AddAsync( loanRequest, cancellationToken );
        await _loanRepository.SaveChangesAsync( cancellationToken );

        return Unit.Value;
    }
}
