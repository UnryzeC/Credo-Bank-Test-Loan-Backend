using System.Net;
using System.Text.Json;

using LoanService.Application.Common.Exceptions;
using LoanService.Application.User.Commands.Login;
using LoanService.Application.User.Commands.Register;

namespace LoanService.Api.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware( RequestDelegate next, ILogger<ExceptionMiddleware> logger )
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync( HttpContext context )
    {
        try
        {
            await _next( context );
        }
        catch ( ValidationException ex )
        {
            await HandleValidationException( context, ex, HttpStatusCode.BadRequest );
        }
        catch ( InvalidEmailOrPasswordException ex )
        {
            await HandleExceptionAsync( context, ex, HttpStatusCode.BadRequest );
        }
        catch ( UserAlreadyExistsException ex )
        {
            await HandleExceptionAsync( context, ex, HttpStatusCode.Conflict );
        }
        catch ( Exception ex )
        {
            await HandleExceptionAsync( context, ex, "Internal error, please try again later", HttpStatusCode.InternalServerError );
        }
        // Add more catch blocks for other custom exceptions if needed
    }

    private Task HandleValidationException( HttpContext context, ValidationException exception, HttpStatusCode statusCode )
    {
        LogError( string.Join( ", ", exception.Errors ), statusCode );

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var result = JsonSerializer.Serialize( new { error = exception.Errors } );

        return context.Response.WriteAsync( result );
    }

    private Task HandleExceptionAsync( HttpContext context, Exception exception, HttpStatusCode statusCode )
    {
        LogError( exception.Message, statusCode );

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var result = JsonSerializer.Serialize( new { error = exception.Message } );

        return context.Response.WriteAsync( result );
    }

    private Task HandleExceptionAsync( HttpContext context, Exception exception, string message, HttpStatusCode statusCode )
    {
        LogError( exception.Message, statusCode );

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var result = JsonSerializer.Serialize( new { error = message } );

        return context.Response.WriteAsync( result );
    }

    private void LogError( string message, HttpStatusCode statusCode )
    {
        _logger.LogError( "Exception was thrown [StatusCode: {0}]: {1}", statusCode, message );
    }
}
