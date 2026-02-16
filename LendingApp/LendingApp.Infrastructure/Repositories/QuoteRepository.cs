using Azure.Core;
using LendingApp.Domain.DTO.LoanCalculator;
using LendingApp.Domain.DTO.LoanRequest;
using LendingApp.Domain.DTO.Quote;
using LendingApp.Domain.Entities;
using LendingApp.Domain.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace LendingApp.Infrastructure.Repositories
{
    public class QuoteRepository : IQuoteRepository
    {
        private readonly ApplicationDbContext _context;

        public QuoteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Quote> GetQuoteById(Guid quoteId)
        {
            var result = await _context.Quote
                        .Include(q => q.Product)
                        .FirstOrDefaultAsync(q => q.Id == quoteId);
            return result == null ? throw new ArgumentException($"Quote with ID {quoteId} not found."): result;
        }

        public async Task<Quote> SaveQuote(QuoteRequest request, LoanCalculationResponse calculation)
        {
            // Create quote entity and save to database
            var quote = new Quote
            {
                Id = Guid.NewGuid(),
                BorrowerId = request.LoanRequestId,
                ProductId = Guid.Parse(request.ProductId),
                LoanAmount = request.LoanAmount,
                TermMonths = request.TermMonths,
                Interest = calculation.TotalInterest,
                TotalAmount = calculation.TotalRepayment,
                MonthlyPayment = calculation.PaymentAmount,
                EstablishmentFee = calculation.EstablishmentFee,
                InterestRate = calculation.InterestRate,
                CustomerTitle = request.BorrowerDetails.Title,
                CustomerFirstName = request.BorrowerDetails.FirstName,
                CustomerLastName = request.BorrowerDetails.LastName,
                CustomerDateOfBirth = request.BorrowerDetails.DateOfBirth,
                CustomerEmail = request.BorrowerDetails.Email,
                CustomerMobileNumber = request.BorrowerDetails.MobileNumber,
                CreatedAt = DateTime.UtcNow
            };

            _context.Quote.Add(quote);
            await _context.SaveChangesAsync();
            return quote;
        }
    }
}
