using LendingApp.Domain.DTO.LoanConfirmation;
using MediatR;

namespace LendingApp.Application.Command.LoanConfirmation
{
    public class LoanConfirmationCommand : IRequest<LoanConfirmationResponse>
    {
        public LoanConfirmationRequest LoanConfirmationRequest { get; set; }

        public LoanConfirmationCommand(LoanConfirmationRequest loanConfirmationRequest)
        {
            LoanConfirmationRequest = loanConfirmationRequest;
        }
    }
}
