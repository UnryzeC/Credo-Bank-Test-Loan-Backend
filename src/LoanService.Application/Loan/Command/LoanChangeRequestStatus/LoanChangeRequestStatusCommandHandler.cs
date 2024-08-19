using Ardalis.Specification;

using LoanService.Application.Loan.Command.CreateLoanRequest;
using LoanService.Core.Loan;
using LoanService.Core.User;

using MediatR;

namespace LoanService.Application.Loan.Command.ChangeLoanRequestStatus;

public class LoanChangeRequestStatusCommandHandler : IRequestHandler<LoanChangeRequestStatusCommand, Unit>
{
    private readonly IRepositoryBase<LoanRequest> _loanRepository;
    private readonly IReadRepositoryBase<UserEntity> _userReadRepository;

    public LoanChangeRequestStatusCommandHandler( IRepositoryBase<LoanRequest> loanRepository, IReadRepositoryBase<UserEntity> userReadRepository )
    {
        _loanRepository = loanRepository;
        _userReadRepository = userReadRepository;
    }

    public async Task<Unit> Handle( LoanChangeRequestStatusCommand request, CancellationToken cancellationToken )
    {
        var loanRequest = await _loanRepository.GetByIdAsync( request.LoanRequestId, cancellationToken );

        if ( loanRequest is null )
        {
            throw new LoanRequestNotFoundException( $"Loan request #{request.LoanRequestId} does not exist!" );
        }

        var loanOfficer = await _userReadRepository.GetByIdAsync( request.LoanOfficerId, cancellationToken );

        if ( loanOfficer is null )
        {
            throw new UserDoesNotExistException( $"Loan Officer #{request.LoanOfficerId} does not exist!" );
        }

        if ( loanRequest.Status == request.Status && ( loanOfficer != null && loanRequest.LoanOfficer == loanOfficer ) )
        {
            return Unit.Value;
        }

        loanRequest.Status = request.Status;
        loanRequest.LoanOfficer = loanOfficer;

        await _loanRepository.SaveChangesAsync( cancellationToken );

        return Unit.Value;
    }
}
