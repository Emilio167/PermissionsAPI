using Application.CQRS.Commands;
using Application.CQRS.Handlers;
using Application.CQRS.Queries;
using Application.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PermissionsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PermissionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("request")]
        public async Task<IActionResult> RequestPermission([FromBody] RequestPermissionCommand command)
        {
            await _mediator.Send(command);
            return Ok("Permission requested successfully.");
        }

        [HttpPut("modify/{id}")]
        public async Task<IActionResult> ModifyPermission(int id, [FromBody] ModifyPermissionCommand command)
        {
            command.Id = id;
            await _mediator.Send(command);
            return Ok("Permission modified successfully.");
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetPermissions()
        {
            var permissions = await _mediator.Send(new GetPermissionsQuery());
            return Ok(permissions);
        }
    }
}
