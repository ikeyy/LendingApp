using LendingApp.Domain.DTO.LoanConfirmation;

namespace LendingApp.Domain.Interfaces.Service
{
    public interface ILoanConfirmationService
    {
        Task<LoanConfirmationResponse> SaveApplicationData(LoanConfirmationRequest request);
    }
}
