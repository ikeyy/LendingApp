using LendingApp.Domain.DTO.LoanCalculator;
using LendingApp.Domain.Interfaces.Service;
using LendingApp.Domain.Utils;
using MediatR;

namespace LendingApp.Application.Query.GetLoanCalculation
{
    public class GetLoanCalculationQueryHandler : IRequestHandler<GetLoanCalculationQuery, LoanCalculationResponse>
    {
        private readonly ILoanCalculationService _loanCalculationService;

        public GetLoanCalculationQueryHandler(ILoanCalculationService loanCalculationService)
        {
            _loanCalculationService = loanCalculationService;
        }

        public async Task<LoanCalculationResponse> Handle(GetLoanCalculationQuery request, CancellationToken cancellationToken)
        {
            var loanCalculation = await _loanCalculationService.CalculateLoan(request.LoanCalculationRequest);

            return loanCalculation;
        }
    }
}
