using LendingApp.Domain.DTO.LoanCalculator;
using MediatR;

namespace LendingApp.Application.Query.GetLoanCalculation
{
    public class GetLoanCalculationQuery : IRequest<LoanCalculationResponse>
    {
        public LoanCalculationRequest LoanCalculationRequest { get; set; }

        public GetLoanCalculationQuery(LoanCalculationRequest loanCalculationRequest)
        {
            LoanCalculationRequest = loanCalculationRequest;
        }
    }
}
