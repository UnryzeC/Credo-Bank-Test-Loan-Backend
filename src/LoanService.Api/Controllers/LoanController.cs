using LoanService.Application.Loan.Command.ChangeLoanRequestStatus;
using LoanService.Application.Loan.Command.CreateLoanRequest;
using LoanService.Application.Loan.Command.UpdateLoanRequest;
using LoanService.Application.Loan.Queries.GetActiveLoanRequests;
using LoanService.Application.Loan.Queries.GetLoanRequests;
using LoanService.Application.Loan.Queries.GetUserActiveLoans;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanService.Api.Controllers;

[Authorize]
[Route("api/loans")]
public class LoanController : ApiControllerBase
{
    private readonly RabbitProducer _rabbitProducer;

    public LoanController( RabbitProducer rabbitProducer )
    {
        _rabbitProducer = rabbitProducer;
    }

    [HttpPost]
    public IResult CreateLoanRequest( LoanCreateRequestCommand command, CancellationToken cancellationToken )
    {
        _rabbitProducer.CreateRequest( command, cancellationToken );
        return Results.Ok();
    }

    [HttpPut("status")]
    public IResult ChangeLoanRequestStatus( LoanChangeRequestStatusCommand command, CancellationToken cancellationToken )
    {
        _rabbitProducer.ChangeStatusRequest( command, cancellationToken );
        return Results.Ok();
    }

    [HttpPut]
    public IResult UpdateLoanRequest( LoanRequestUpdateCommand command, CancellationToken cancellationToken )
    {
        _rabbitProducer.UpdateRequest( command, cancellationToken );
        return Results.Ok();
    }

    [HttpGet("active")]
    public async Task<IResult> GetActiveLoans( CancellationToken cancellationToken )
    {
        var query = new GetLoanActiveRequestsQuery();
        var response = await Mediator.Send( query, cancellationToken );
        return Results.Ok(response);
    }

    [HttpGet("{id:guid}/active")]
    public async Task<IResult> GetUserActriveLoans( Guid id, CancellationToken cancellationToken )
    {
        var query = new GetUserActiveLoansQuery( id );
        var response = await Mediator.Send( query, cancellationToken );
        return Results.Ok( response );
    }

    [HttpGet]
    public async Task<IResult> GetLoans( CancellationToken cancellationToken )
    {
        var query = new GetLoanRequestsQuery( );
        var response = await Mediator.Send( query, cancellationToken );
        return Results.Ok( response );
    }
}
