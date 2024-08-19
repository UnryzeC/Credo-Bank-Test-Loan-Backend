using static LoanService.Core.User.Enums.Roles;

namespace LoanService.Application.Common.Security.Jwt.Interfaces;

public interface IJwtTokenService
{
    Task<string> GenerateTokenAsync( Guid userId, UserRole role );
}
