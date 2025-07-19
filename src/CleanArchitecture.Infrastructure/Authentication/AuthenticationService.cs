using CleanArchitecture.Application.Authentication.Common;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Users;

using ErrorOr;

namespace CleanArchitecture.Infrastructure.Authentication;

public class AuthenticationService(IUsersRepository usersRepository, IJwtTokenGenerator jwtTokenGenerator) : IAuthenticationService
{
    private readonly IUsersRepository _usersRepository = usersRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;

    public async Task<AuthenticationResult> Register(User user, string password)
    {
        // Here you would typically hash the password before storing it.
        // For simplicity, we're just storing the plain password (NOT recommended in production).
        user.SetPassword(password);
        CancellationToken cancellationToken = CancellationToken.None;

        await _usersRepository.AddAsync(user, cancellationToken);

        var token = _jwtTokenGenerator.GenerateToken(
                                                    user.Id,
                                                    user.FirstName,
                                                    user.LastName,
                                                    user.Email,
                                                    user.Subscription.SubscriptionType, // Still need SubscriptionType from the User object
                                                    GetUserPermissions(user.Id), // Get permissions using placeholder method
                                                    GetUserRoles(user.Id)); // Get roles using placeholder method

        return new AuthenticationResult(user);
    }

    public Task<ErrorOr<AuthenticationResult>> Register(User user)
    {
        throw new NotImplementedException();
    }

    // Placeholder method to get user permissions
    private static List<string> GetUserPermissions(Guid userId)
    {
        // TODO: Implement logic to retrieve user permissions based on userId
        return [];
    }

    // Placeholder method to get user roles
    private static List<string> GetUserRoles(Guid userId)
    {
        // TODO: Implement logic to retrieve user roles based on userId
        return [];
    }
}