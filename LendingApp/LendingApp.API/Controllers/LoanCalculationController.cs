using LendingApp.Application.Query.GetLoanCalculation;
using LendingApp.Domain.DTO.LoanCalculator;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LendingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanCalculationController : ControllerBase
    {
        private readonly IMediator _mediator;
        public LoanCalculationController(IMediator mediator) => _mediator = mediator;


        [HttpPost("calculate")]
        public async Task<ActionResult<LoanCalculationResponse>> CalculateLoan(
            [FromBody] LoanCalculationRequest request)
        {
            try
            {
                var command = new GetLoanCalculationQuery(request);

                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while calculating the loan.", error = ex.Message });
            }
        }
    }
}
