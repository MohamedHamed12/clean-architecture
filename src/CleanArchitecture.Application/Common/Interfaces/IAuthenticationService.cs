using CleanArchitecture.Application.Authentication.Common;
using CleanArchitecture.Domain.Users;

using ErrorOr;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IAuthenticationService
{
    Task<ErrorOr<AuthenticationResult>> Register(User user);
}
