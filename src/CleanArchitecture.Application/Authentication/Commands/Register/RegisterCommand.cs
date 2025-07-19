using CleanArchitecture.Application.Authentication.Common;
using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Authentication.Commands.Register;

public record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password) : IRequest<ErrorOr<AuthenticationResult>>;