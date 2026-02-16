using LendingApp.Domain.DTO.LoanRequest;
using LendingApp.Domain.Entities;

namespace LendingApp.Domain.Interfaces.Repository
{
    public interface IBorrowerRepository
    {
        Task<Guid> AddBorrower(LoanRequest loanRequest);
        Task<Guid> UpdateBorrower(LoanRequest loanRequest);
        Task<Borrower> GetBorrowerByNameAndDateOfBirth(string firstName, string lastName, DateTime dateOfbirth,CancellationToken cancellationToken = default);
        Task<Borrower> GetBorrowerById(Guid borrowerId,CancellationToken cancellationToken = default);
        Task<bool> IsBorrowerEmailExist(string email, CancellationToken cancellation = default);
    }
}
