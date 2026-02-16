using LendingApp.Domain.DTO.LoanRequest;
using LendingApp.Domain.Entities;
using LendingApp.Domain.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace LendingApp.Infrastructure.Repositories
{
    public class LoanDetailsRepository : ILoanDetailsRepository
    {
        private readonly ApplicationDbContext _context;

        public LoanDetailsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddLoanDetails(LoanRequest loanRequest,Guid borrowerId)
        {
            LoanDetails loanDetails = new LoanDetails()
            {
                Id = Guid.NewGuid(),
                Term = loanRequest.Term,
                Amount = loanRequest.AmountRequired,
                BorrowerId = borrowerId
            };
            _context.LoanDetails.Add(loanDetails);
            await Task.CompletedTask;
        }

        public async Task UpdateLoanDetails(LoanRequest loanRequest,Guid borrowerId)
        {
            LoanDetails loanDetails = new LoanDetails()
            {
                Id = Guid.NewGuid(),
                Term = loanRequest.Term,
                Amount = loanRequest.AmountRequired,
                BorrowerId = borrowerId
            };

            _context.LoanDetails.Update(loanDetails);
            await Task.CompletedTask;
        }

        public async Task<LoanDetails> GetLoanDetailsById(Guid borrowerId, CancellationToken cancellation = default)
        {
            var result = await _context.LoanDetails.Where(x => x.BorrowerId == borrowerId).FirstOrDefaultAsync(cancellation);
            return result == null ? new LoanDetails() : result;
        }
    }
}
