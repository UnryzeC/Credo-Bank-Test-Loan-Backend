using MediatR;

namespace LoanService.Application.Loan.Queries.GetLoanRequests;

public record GetLoanRequestsQuery : IRequest<GetLoanRequestsQueryResponse> { }
