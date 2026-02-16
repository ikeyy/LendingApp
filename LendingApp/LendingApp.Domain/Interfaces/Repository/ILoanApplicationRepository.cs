using LendingApp.Domain.Entities;

namespace LendingApp.Domain.Interfaces.Repository
{
    public interface ILoanApplicationRepository
    {
        Task<bool> SaveLoanApplication(LoanApplication loanApplication, CancellationToken cancellationToken = default);
    }
}
