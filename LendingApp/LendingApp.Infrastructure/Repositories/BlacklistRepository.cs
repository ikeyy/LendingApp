using LendingApp.Domain.DTO.Product;
using LendingApp.Domain.Entities;
using LendingApp.Domain.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace LendingApp.Infrastructure.Repositories
{
    public class BlacklistRepository : IBlacklistRepository
    {
        private readonly ApplicationDbContext _context;

        public BlacklistRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Blacklist>> GetBlacklistByType(string[] blacklistTypes)
        {
            var result = await _context.Blacklist.Where(x => blacklistTypes.Contains(x.Type)).ToListAsync();
            return result == null ? new List<Blacklist>() : result;
        }

        public async Task<bool> AddBlacklist(Blacklist blacklistData)
        {
            Blacklist blacklist = new Blacklist()
            {
                Id = Guid.NewGuid(),
                Type = blacklistData.Type,
                Value = blacklistData.Value
            };
            _context.Blacklist.Add(blacklist);
            var isSaved = await _context.SaveChangesAsync() > 0 ? true : false;
            return isSaved;
        }
    }
}
