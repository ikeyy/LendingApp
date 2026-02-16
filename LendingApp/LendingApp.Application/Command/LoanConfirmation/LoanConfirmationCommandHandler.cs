using LendingApp.Domain.DTO.LoanConfirmation;
using LendingApp.Domain.Interfaces.Service;
using MediatR;

namespace LendingApp.Application.Command.LoanConfirmation
{
    public class LoanConfirmationCommandHandler : IRequestHandler<LoanConfirmationCommand, LoanConfirmationResponse>
    {
        private readonly ILoanConfirmationService _loanConfirmationService;

        public LoanConfirmationCommandHandler(
            ILoanConfirmationService loanConfirmationService
            )
        {
            _loanConfirmationService = loanConfirmationService;
        }

        public async Task<LoanConfirmationResponse> Handle(LoanConfirmationCommand request, CancellationToken cancellationToken)
        {
            var result = await _loanConfirmationService.SaveApplicationData(request.LoanConfirmationRequest);

            return result;
        }
    }
}
