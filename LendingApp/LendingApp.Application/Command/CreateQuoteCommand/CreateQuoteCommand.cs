using LendingApp.Domain.DTO.Quote;
using MediatR;

namespace LendingApp.Application.Command.CreateQuoteCommand
{
    public class CreateQuoteCommand : IRequest<QuoteResponse>
    {
        public QuoteRequest QuoteRequest { get; set; }

        public CreateQuoteCommand(QuoteRequest quoteRequest)
        {
            QuoteRequest = quoteRequest;
        }
    }
}
