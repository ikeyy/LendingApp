using LendingApp.Domain.Entities;
using LendingApp.Domain.Interfaces.Repository;

namespace LendingApp.Infrastructure.Repositories
{
    public class LoanApplicationRepository : ILoanApplicationRepository
    {
        private readonly ApplicationDbContext _context;

        public LoanApplicationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveLoanApplication(LoanApplication loanApplication,CancellationToken cancellationToken = default)
        {          
            _context.LoanApplication.Add(loanApplication);
            return await _context.SaveChangesAsync(cancellationToken) > 0 ? true : false;
        }
    }
}
