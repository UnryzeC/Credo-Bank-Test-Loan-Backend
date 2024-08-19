using Ardalis.Specification;

using LoanService.Application.Loan.Command.ChangeLoanRequestStatus;
using LoanService.Application.Loan.Command.CreateLoanRequest;
using LoanService.Core.Loan;
using LoanService.Core.User;

using MediatR;

namespace LoanService.Application.Loan.Command.UpdateLoanRequest;

public class LoanUpdateRequestCommandHandler : IRequestHandler<LoanRequestUpdateCommand, Unit>
{
    private readonly IRepositoryBase<LoanRequest> _loanRepository;
    private readonly IReadRepositoryBase<UserEntity> _userReadRepository;

    public LoanUpdateRequestCommandHandler( IRepositoryBase<LoanRequest> loanRepository, IReadRepositoryBase<UserEntity> userReadRepository )
    {
        _loanRepository = loanRepository;
        _userReadRepository = userReadRepository;
    }

    public async Task<Unit> Handle( LoanRequestUpdateCommand request, CancellationToken cancellationToken )
    {
        var loanRequest = await _loanRepository.GetByIdAsync( request.LoanRequestId, cancellationToken );

        if ( loanRequest is null )
        {
            throw new LoanRequestNotFoundException( $"Loan request #{request.LoanRequestId} does not exist!" );
        }

        var user = await _userReadRepository.GetByIdAsync( request.UserId, cancellationToken );

        if ( user is null )
        {
            throw new UserDoesNotExistException( $"User #{request.UserId} does not exist!" );
        }

        loanRequest.Type = request.Type;
        loanRequest.RequestedAmount = request.RequestedAmount;
        loanRequest.Currency = request.Currency;
        loanRequest.LoanPeriod = request.LoanPeriod;

        if ( loanRequest.Status == LoanStatus.Approved )
        { 
            loanRequest.Status = LoanStatus.Processing;
        }

        await _loanRepository.SaveChangesAsync( cancellationToken );

        return Unit.Value;
    }
}

