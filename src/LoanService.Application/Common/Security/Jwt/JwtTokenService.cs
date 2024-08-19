using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using LoanService.Application.Common.Security.Jwt.Interfaces;
using LoanService.Application.Common.Security.Jwt.Options;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using static LoanService.Core.User.Enums.Roles;

namespace LoanService.Application.Common.Security.Jwt;

internal class JwtTokenService : IJwtTokenService
{
    private readonly JwtTokenOptions _options;

    public JwtTokenService( IOptions<JwtTokenOptions> options )
    {
        _options = options.Value;
    }

    public Task<string> GenerateTokenAsync( Guid userId, UserRole role )
    {
        var tokenHandler = new JwtSecurityTokenHandler( );
        var key = Encoding.ASCII.GetBytes( _options.SecretKey );

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity( GetClaims( userId, role ) ),
            Issuer = _options.Issuer,
            Audience = _options.Audience,
            Expires = DateTime.UtcNow.AddHours( _options.ExpiresIn ),
            SigningCredentials = new SigningCredentials( new SymmetricSecurityKey( key ), SecurityAlgorithms.HmacSha256Signature )
        };

        var token = tokenHandler.CreateToken( tokenDescriptor );

        return Task.FromResult( tokenHandler.WriteToken( token ) );
    }

    private static List<Claim> GetClaims( Guid userId, UserRole role )
    {
        return // new List<Claim>
        [
            new Claim( ClaimTypes.NameIdentifier, userId.ToString( ) ),
            new Claim( ClaimTypes.Role, ((int)role).ToString( ) ),
        ];
    }
}
