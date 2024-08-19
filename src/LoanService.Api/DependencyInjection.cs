using System.Security.Claims;
using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace LoanService.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices( this IServiceCollection services, IConfiguration configuration )
    {
        services.AddControllers( );
        services.AddEndpointsApiExplorer( );
        services.AddAuthentication( configuration );
        services.AddSwagger();
        services.AddCors( configuration );

        return services;
    }

    public static void AddAuthentication( this IServiceCollection services, IConfiguration configuration )
    {
        _ = services.AddAuthentication( options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer( options =>
            {
                var issuer = configuration[ "JwtToken:Issuer" ];
                var audience = configuration[ "JwtToken:Audience" ];

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey( Encoding.UTF8.GetBytes( configuration[ "JwtToken:SecretKey" ]! ) ),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    RoleClaimType = ClaimTypes.Role
                };
            });
    }

    public static void AddSwagger( this IServiceCollection services ) =>
        services.AddSwaggerGen( options =>
        {
            options.SwaggerDoc( "v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Loan API"
            });

            options.AddSecurityDefinition( "Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a JWT token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

    public static void AddCors( this IServiceCollection services, IConfiguration configuration ) =>
        services.AddCors( options =>
        {
            options.AddDefaultPolicy( policy =>
            {
                policy.AllowAnyOrigin( ).AllowAnyHeader( ).AllowAnyMethod( );
            });
        });
}
