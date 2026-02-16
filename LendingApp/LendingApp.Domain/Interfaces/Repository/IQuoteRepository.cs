using LendingApp.Domain.DTO.LoanCalculator;
using LendingApp.Domain.DTO.Quote;
using LendingApp.Domain.Entities;

namespace LendingApp.Domain.Interfaces.Repository
{
    public interface IQuoteRepository
    {
        Task<Quote> GetQuoteById(Guid quoteId);
        Task<Quote> SaveQuote(QuoteRequest request, LoanCalculationResponse calculation);
    }
}
