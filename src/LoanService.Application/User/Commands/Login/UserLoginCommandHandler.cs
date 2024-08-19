using Ardalis.Specification;

using LoanService.Application.Common.Security.Jwt.Interfaces;
using LoanService.Application.Common.Security.Password;
using LoanService.Core.User;
using LoanService.Core.User.Specifications;

using MediatR;

using static LoanService.Core.User.Enums.Roles;

namespace LoanService.Application.User.Commands.Login;

public class UserLoginCommandHandler : IRequestHandler<UserLoginCommand, UserLoginCommandResponse>
{
    private readonly IReadRepositoryBase<UserEntity> _userReadRepository;
    private readonly IJwtTokenService _tokenService;

    public UserLoginCommandHandler( IReadRepositoryBase<UserEntity> userReadRepository, IJwtTokenService tokenService )
    {
        _userReadRepository = userReadRepository;
        _tokenService = tokenService;
    }

    public async Task<UserLoginCommandResponse> Handle( UserLoginCommand request, CancellationToken cancellationToken )
    {
        var user = await _userReadRepository.SingleOrDefaultAsync( new GetUserByEmailSpec( request.Email ), cancellationToken );

        if ( user is null || !PasswordManager.IsValidPassword( request.Password, user.Password, user.Salt ) )
        {
            throw new InvalidEmailOrPasswordException( $"Invalid email and password combination for ({request.Email})" );
        }

        var token = await _tokenService.GenerateTokenAsync( user.Id, UserRole.Customer );

        return new UserLoginCommandResponse( new( user.Id, $"{user.Firstname} {user.Lastname}" ), token );
    }
}
