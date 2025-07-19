using CleanArchitecture.Application.Authentication.Common;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Users;

using ErrorOr;

using MediatR;

namespace CleanArchitecture.Application.Authentication.Commands.Register;

public class RegisterCommandHandler(IUsersRepository usersRepository) : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IUsersRepository _usersRepository = usersRepository;
    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = new User(
            Guid.NewGuid(),
            request.FirstName,
            request.LastName,
            request.Email,
            null,
            null);

        await _usersRepository.AddAsync(user, cancellationToken);

        return new AuthenticationResult(user);
    }
}
