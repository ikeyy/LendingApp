using LendingApp.Domain.DTO.Quote;
using MediatR;

namespace LendingApp.Application.Query.GetQuoteById
{
    public class GetQuoteByIdQuery : IRequest<QuoteResponse>
    {
        public string QuoteId { get; set; }

        public GetQuoteByIdQuery(string quoteId)
        {
            QuoteId = quoteId;
        }
    }
}
