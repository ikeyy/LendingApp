using LendingApp.Domain.Entities;

namespace LendingApp.Domain.Interfaces.Repository
{
    public interface IBlacklistRepository
    {
        Task<List<Blacklist>> GetBlacklistByType(string[] blacklistTypes);

        Task<bool> AddBlacklist(Blacklist blacklistData);
    }
}
