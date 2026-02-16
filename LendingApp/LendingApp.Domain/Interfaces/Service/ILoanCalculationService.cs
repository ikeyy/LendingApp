using LendingApp.Domain.DTO.LoanCalculator;

namespace LendingApp.Domain.Interfaces.Service
{
    public interface ILoanCalculationService
    {
        Task<LoanCalculationResponse> CalculateLoan(LoanCalculationRequest request);
    }
}
