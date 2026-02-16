using LendingApp.Domain.DTO.Quote;
using LendingApp.Domain.Interfaces.Service;
using MediatR;

namespace LendingApp.Application.Query.GetQuoteById
{
    public class GetQuoteByIdQueryHandler : IRequestHandler<GetQuoteByIdQuery, QuoteResponse>
    {
        private readonly IQuoteService _quoteService;

        public GetQuoteByIdQueryHandler(IQuoteService quoteService)
        {
            _quoteService = quoteService;
        }

        public async Task<QuoteResponse> Handle(GetQuoteByIdQuery request, CancellationToken cancellationToken)
        {
            var quoteData = await _quoteService.GetQuoteByIdAsync(Guid.Parse(request.QuoteId));

            return quoteData;
        }
    }
}
