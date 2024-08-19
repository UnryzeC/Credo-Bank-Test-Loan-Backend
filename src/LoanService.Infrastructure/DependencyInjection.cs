using Ardalis.Specification;

using LoanService.Infrastructure.Persistance;
using LoanService.Infrastructure.Persistance.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LoanService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices( this IServiceCollection services, IConfiguration configuration )
    {
        services.AddDbContext<AppDbContext>( options =>
        {
            options.UseSqlite( configuration.GetConnectionString( "AppDbConnection" ) );
        });

        services.AddScoped( typeof(IRepositoryBase<>), typeof( EfRepository<> ) );
        services.AddScoped( typeof(IReadRepositoryBase<>), typeof( EfRepository<> ) );

        return services;
    }
}
