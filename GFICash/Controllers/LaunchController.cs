using Domain.Commands.Requests;
using Domain.Commands.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    [ApiController]
    [Route("v1/lancamentos")]
    public class LaunchController : ControllerBase
    {
        [HttpPost]
        [Route("")]
        public Task<CreateLaunchResponse> Create(
            [FromServices] IMediator mediator,
            [FromBody] CreateLaunchRequest request
        )
        {
            return mediator.Send(request);
        }
    }
}