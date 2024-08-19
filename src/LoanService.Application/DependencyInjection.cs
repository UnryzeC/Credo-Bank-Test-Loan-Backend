using System.Reflection;

using FluentValidation;

using LoanService.Application.Common.Behaviours;
using LoanService.Application.Common.Security.Jwt;
using LoanService.Application.Common.Security.Jwt.Interfaces;
using LoanService.Application.Common.Security.Jwt.Options;

using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LoanService.Application;

public static class DependencyInjection
{
    public static void AddApplicationServices( this IServiceCollection services, IConfiguration configuration )
    {
        services.AddValidatorsFromAssembly( Assembly.GetExecutingAssembly( ) );
        services.AddMediatR( cfg => cfg.RegisterServicesFromAssembly( Assembly.GetExecutingAssembly( ) ) );

        services.AddTransient( typeof(IPipelineBehavior<,>), typeof( ValidationBehaviour<,> ) );
        services.AddTransient( typeof(IPipelineBehavior<,>), typeof( LoggingBehaviour<,> ) );

        services.Configure<JwtTokenOptions>( options => configuration.GetSection( JwtTokenOptions.Key ).Bind( options ) );

        services.AddScoped<IJwtTokenService, JwtTokenService>( );
        services.AddSingleton<RabbitProducer>( );
    }
}
