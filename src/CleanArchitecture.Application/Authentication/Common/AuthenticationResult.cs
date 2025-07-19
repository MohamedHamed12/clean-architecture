using CleanArchitecture.Domain.Users;

namespace CleanArchitecture.Application.Authentication.Common;

public sealed record AuthenticationResult(
    User User);
