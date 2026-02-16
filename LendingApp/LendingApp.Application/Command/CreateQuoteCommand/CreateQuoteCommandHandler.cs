using LendingApp.Domain.DTO.Quote;
using LendingApp.Domain.Interfaces.Service;
using MediatR;

namespace LendingApp.Application.Command.CreateQuoteCommand
{
    public class CreateQuoteCommandHandler : IRequestHandler<CreateQuoteCommand, QuoteResponse>
    {
        private readonly IQuoteService _quoteService;

        public CreateQuoteCommandHandler(
            IQuoteService quoteService)
        {
            _quoteService = quoteService;
        }

        public async Task<QuoteResponse> Handle(CreateQuoteCommand request, CancellationToken cancellationToken)
        {

            var result = await _quoteService.CreateQuoteAsync(request.QuoteRequest);

            return result;
        }
    }
}
