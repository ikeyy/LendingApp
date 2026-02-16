using LendingApp.Domain.DTO.LoanRequest;

namespace LendingApp.Domain.Interfaces.Service
{
    public interface ILoanRequestService
    {
        Task SaveBorrowerData(LoanRequest loanRequest);
        Task<BorrowerData> GetBorrowerByNameAndDateOfBirth(LoanRequest loanRequest,CancellationToken cancellation = default);
        Task<LoanResponse> CreateRedirectURL(LoanRequest loanRequest, CancellationToken cancellationToken = default);
        Task<BorrowerData> GetBorrowerById(Guid borrowerId, CancellationToken cancellation = default);
        Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);

    }
}
