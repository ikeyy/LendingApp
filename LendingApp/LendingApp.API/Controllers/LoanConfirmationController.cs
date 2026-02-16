using LendingApp.Application.Command.LoanConfirmation;
using LendingApp.Domain.DTO.LoanConfirmation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LendingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanConfirmationController : Controller
    {
        private readonly IMediator _mediator;
        public LoanConfirmationController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<LoanConfirmationResponse>> Post([FromBody] LoanConfirmationRequest request)
        {
            try
            {
                var command = new LoanConfirmationCommand(request);

                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (FluentValidation.ValidationException ex)
            {
                var errors = ex.Errors
                    .Select(e => new { Property = e.PropertyName, Error = e.ErrorMessage })
                    .ToList();

                return BadRequest(new { Message = "Validation failed", Errors = errors });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while submitting the loan application.", error = ex.Message });
            }
        }
    }
}
