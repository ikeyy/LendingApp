using LendingApp.Domain.DTO.LoanRequest;
using LendingApp.Domain.Entities;
using LendingApp.Domain.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace LendingApp.Infrastructure.Repositories
{
    public class BorrowerRepository : IBorrowerRepository
    {
        private readonly ApplicationDbContext _context;

        public BorrowerRepository(ApplicationDbContext context) {
            _context = context;    
        }

        public async Task<Borrower> GetBorrowerById(Guid borrowerId,CancellationToken cancellation = default)
        {
            var result = await _context.Borrower.Where(x => x.Id == borrowerId).FirstOrDefaultAsync(cancellation);
            return result;
        }

        public async Task<Borrower> GetBorrowerByNameAndDateOfBirth(string firstName, string lastName, DateTime dateOfbirth, CancellationToken cancellation)
        {
            var result = await _context.Borrower
                            .Where(x => x.FirstName == firstName 
                            && x.LastName == lastName
                            && x.DateOfBirth == dateOfbirth
                            ).FirstOrDefaultAsync(cancellation);
            return result;
        }

        public async Task<Guid> AddBorrower(LoanRequest loanRequest)
        {
            Borrower borrower = new Borrower()
            {
                Id = Guid.NewGuid(),
                Title = loanRequest.Title,
                DateOfBirth = loanRequest.DateOfBirth,
                FirstName = loanRequest.FirstName,
                LastName = loanRequest.LastName,
                Email = loanRequest.Email,
                Mobile = loanRequest.Mobile,
            };
            _context.Borrower.Add(borrower);
            await Task.CompletedTask;
            return borrower.Id;
        }

        public async Task<Guid> UpdateBorrower(LoanRequest loanRequest)
        {
            Borrower borrower = new Borrower()
            {
                Id = Guid.NewGuid(),
                Title = loanRequest.Title,
                DateOfBirth = loanRequest.DateOfBirth,
                FirstName = loanRequest.FirstName,
                LastName = loanRequest.LastName,
                Email = loanRequest.Email,
                Mobile = loanRequest.Mobile,
            };
            _context.Borrower.Update(borrower);
            await Task.CompletedTask;
            return borrower.Id;
        }

        public async Task<bool> IsBorrowerEmailExist(string email, CancellationToken cancellation = default)
        {
            var result = await _context.Borrower.Where(x => x.Email == email).FirstOrDefaultAsync(cancellation);
            return result != null ? true : false;
        }
    }
}
