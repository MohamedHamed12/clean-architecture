using CleanArchitecture.Application.Authentication.Commands.Register;
using CleanArchitecture.Contracts.Authentication;

using ErrorOr;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(ISender _mediator) : ApiController
    {
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var command = new RegisterCommand(
                request.FirstName,
                request.LastName,
                request.Email,
                request.Password);

            var result = await _mediator.Send(command);

            return result.Match(
                authResult => Ok(authResult),
                errors => Problem(errors));
        }
    }
}