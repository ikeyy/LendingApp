using LendingApp.Domain.DTO.LoanRequest;
using LendingApp.Domain.Entities;

namespace LendingApp.Domain.Interfaces.Repository
{
    public interface ILoanDetailsRepository
    {
        Task AddLoanDetails(LoanRequest loanRequest, Guid borrowerId);

        Task UpdateLoanDetails(LoanRequest loanRequest, Guid borrowerId);

        Task<LoanDetails> GetLoanDetailsById(Guid borrowerId, CancellationToken cancellation = default);
    }
}
