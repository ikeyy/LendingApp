using LendingApp.Application.Command.CreateQuoteCommand;
using LendingApp.Application.Query.GetQuoteById;
using LendingApp.Domain.DTO.Quote;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LendingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuotesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public QuotesController(IMediator mediator) => _mediator = mediator;


        [HttpPost("create")]
        public async Task<ActionResult<QuoteResponse>> CreateQuote(
            [FromBody] QuoteRequest request)
        {
            try
            {
                var query = new CreateQuoteCommand(request);

                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the quote.", error = ex.Message });
            }
        }


        [HttpGet("{quoteId}")]
        public async Task<ActionResult<QuoteResponse>> GetQuote(string quoteId)
        {
            try
            {
                var query = new GetQuoteByIdQuery(quoteId);

                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the quote.", error = ex.Message });
            }
        }
    }
}
