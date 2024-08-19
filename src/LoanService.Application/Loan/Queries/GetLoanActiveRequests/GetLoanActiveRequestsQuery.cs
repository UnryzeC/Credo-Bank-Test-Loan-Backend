using MediatR;

namespace LoanService.Application.Loan.Queries.GetActiveLoanRequests;

public record GetLoanActiveRequestsQuery : IRequest<GetLoanActiveRequestsQueryResponse> { }
