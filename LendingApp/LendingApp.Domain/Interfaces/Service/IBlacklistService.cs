using LendingApp.Domain.DTO.Blacklist;
using LendingApp.Domain.DTO.LoanRequest;

namespace LendingApp.Domain.Interfaces.Service
{
    public interface IBlacklistService
    {
        Task<bool> CheckIfBlacklisted(LoanRequest loanRequest);
        Task<bool> AddBlacklist(BlacklistData blacklistData);
    }
}
