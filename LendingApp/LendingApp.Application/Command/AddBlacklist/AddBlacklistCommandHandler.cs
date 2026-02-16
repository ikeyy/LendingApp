using LendingApp.Application.Command.AddProduct;
using LendingApp.Domain.Interfaces.Service;
using MediatR;

namespace LendingApp.Application.Command.AddBlacklist
{
    public class AddBlacklistCommandHandler : IRequestHandler<AddBlacklistCommand, bool>
    {
        private readonly IBlacklistService _blacklistService;

        public AddBlacklistCommandHandler(
            IBlacklistService blacklistService)
        {
            _blacklistService = blacklistService;
        }

        public async Task<bool> Handle(AddBlacklistCommand request, CancellationToken cancellationToken)
        {
            var result = await _blacklistService.AddBlacklist(request.BlacklistData);

            return result;
        }
    }
}
