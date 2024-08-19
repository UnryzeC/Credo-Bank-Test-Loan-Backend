using LoanService.Application.User.Commands.Login;
using LoanService.Application.User.Commands.Register;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanService.Api.Controllers;

[Route("api/users")]
public class UserController : ApiControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IResult> Login( UserLoginCommand command, CancellationToken cancellationToken )
    {
        var accessToken = await Mediator.Send( command, cancellationToken );
        return Results.Ok( accessToken );
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IResult> CreateUser( UserRegisterCommand command, CancellationToken cancellationToken )
    {
        var accessToken = await Mediator.Send( command, cancellationToken );
        return Results.Ok( accessToken );
    }
}
