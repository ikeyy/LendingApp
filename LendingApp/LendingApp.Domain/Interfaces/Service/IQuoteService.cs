using LendingApp.Domain.DTO.Quote;

namespace LendingApp.Domain.Interfaces.Service
{
    public interface IQuoteService
    {
        Task<QuoteResponse> CreateQuoteAsync(QuoteRequest request);

        Task<QuoteResponse> GetQuoteByIdAsync(Guid quoteId);
    }
}
