using LendingApp.Domain.DTO.Blacklist;
using LendingApp.Domain.DTO.Product;
using MediatR;

namespace LendingApp.Application.Command.AddBlacklist
{
    public class AddBlacklistCommand : IRequest<bool>
    {
        public BlacklistData BlacklistData { get; set; }

        public AddBlacklistCommand(BlacklistData blacklistData)
        {
            BlacklistData = blacklistData;
        }
    }
}
