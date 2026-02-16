using LendingApp.Application.Command.AddBlacklist;
using LendingApp.Domain.DTO.Blacklist;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LendingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlacklistController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BlacklistController(IMediator mediator) => _mediator = mediator;

        [HttpPost("create")]
        public async Task<bool> Post([FromBody] BlacklistData blacklistData)
        {
            var command = new AddBlacklistCommand(blacklistData);
            var result = await _mediator.Send(command);
            return result;
        }
    }
}
