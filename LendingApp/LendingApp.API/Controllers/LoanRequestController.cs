using Azure.Core;
using LendingApp.Application.Command.CreateLoanRequest;
using LendingApp.Application.Command.LoanConfirmation;
using LendingApp.Application.Command.UpdateLoanRequest;
using LendingApp.Application.Query.GetLoanRequestById;
using LendingApp.Domain.DTO.LoanRequest;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LendingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanRequestController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LoanRequestController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<LoanResponse>> Post([FromBody] LoanRequest loanRequest)
        {
            try
            {
                var command = new CreateLoanRequestCommand(loanRequest);

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

        [HttpGet]
        public async Task<ActionResult<BorrowerData>> GetLoanRequest(string id)
        {
            try
            {
                var query = new GetLoanRequestByIdQuery(id);

                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<LoanResponse> UpdateLoanRequest([FromBody] LoanRequest loanRequest)
        {
            var command = new UpdateLoanRequestCommand(loanRequest);

            var result = await _mediator.Send(command);
            return result;
        }
    }
}
