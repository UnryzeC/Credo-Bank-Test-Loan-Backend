using LoanService.Api;
using LoanService.Api.Middleware;
using LoanService.Application;
using LoanService.Application.RabbitMQ;
using LoanService.Infrastructure;

var builder = WebApplication.CreateBuilder( args );

builder.Services.AddApiServices( builder.Configuration );
builder.Services.AddApplicationServices( builder.Configuration );
builder.Services.AddInfrastructureServices( builder.Configuration );
builder.Services.AddHostedService<RabbitConsumer>( );

var app = builder.Build( );

app.UseCors( );

app.UseSwagger( );
app.UseSwaggerUI( );

app.UseHttpsRedirection( );

app.UseMiddleware<ExceptionMiddleware>( );

app.UseAuthentication( );
app.UseAuthorization( );

app.MapControllers( );

app.Run( );
