using LendingApp.Domain.DTO.LoanRequest;
using LendingApp.Domain.Interfaces.Service;
using MediatR;

namespace LendingApp.Application.Command.CreateLoanRequest
{
    public class CreateLoanRequestCommandHandler : IRequestHandler<CreateLoanRequestCommand, LoanResponse>
    {
        private readonly ILoanRequestService _loanRequestService;

        public CreateLoanRequestCommandHandler(
            ILoanRequestService loanRequestService)
        {
            _loanRequestService = loanRequestService;
        }

        public async Task<LoanResponse> Handle(CreateLoanRequestCommand request, CancellationToken cancellationToken)
        {
            var borrowerData = await _loanRequestService.GetBorrowerByNameAndDateOfBirth(request.LoanRequest);

            if (borrowerData == null)
                await _loanRequestService.SaveBorrowerData(request.LoanRequest);

            var loanResponse = await _loanRequestService.CreateRedirectURL(request.LoanRequest);

            return loanResponse;
        }
    }
}
