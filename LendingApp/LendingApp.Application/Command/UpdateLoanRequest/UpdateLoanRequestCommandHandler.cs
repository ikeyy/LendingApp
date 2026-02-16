using LendingApp.Application.Command.CreateLoanRequest;
using LendingApp.Domain.DTO.LoanRequest;
using LendingApp.Domain.Interfaces.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LendingApp.Application.Command.UpdateLoanRequest
{
    public class UpdateLoanRequestCommandHandler : IRequestHandler<UpdateLoanRequestCommand, LoanResponse>
    {
        private readonly ILoanRequestService _loanRequestService;

        public UpdateLoanRequestCommandHandler(
            ILoanRequestService loanRequestService)
        {
            _loanRequestService = loanRequestService;
        }

        public async Task<LoanResponse> Handle(UpdateLoanRequestCommand request, CancellationToken cancellationToken)
        {
            var borrowerData = await _loanRequestService.GetBorrowerByNameAndDateOfBirth(request.LoanRequest);

            if (borrowerData == null)
                await _loanRequestService.SaveBorrowerData(request.LoanRequest);

            var loanResponse = await _loanRequestService.CreateRedirectURL(request.LoanRequest);

            return loanResponse;
        }
    }
}
