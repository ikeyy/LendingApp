using LendingApp.Domain.Constants;
using LendingApp.Domain.DTO.Blacklist;
using LendingApp.Domain.DTO.LoanRequest;
using LendingApp.Domain.Entities;
using LendingApp.Domain.Extensions;
using LendingApp.Domain.Interfaces.Repository;
using LendingApp.Domain.Interfaces.Service;
using System.ComponentModel.DataAnnotations;

namespace LendingApp.Domain.Services
{
    public class BlacklistService : IBlacklistService
    {

        private readonly IBlacklistRepository _blacklistRepository;

        public BlacklistService(
            IBlacklistRepository blacklistRepository)
        {
            _blacklistRepository = blacklistRepository;
        }
        public async Task<bool> CheckIfBlacklisted(LoanRequest loanRequest)
        {
            var blacklist = await _blacklistRepository.GetBlacklistByType(BlacklistTypeExtensions.GetAllValuesToArray());

            var emailDomain = GetDomainFromEmail(loanRequest.Email);
            var isBlackListed = blacklist != null 
                                ? blacklist
                                    .Where(x => x.Value == loanRequest.Mobile 
                                    || x.Value == emailDomain)
                                    .Count() > 0 
                                : false;
            return isBlackListed;
        }

        private string? GetDomainFromEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            var atIndex = email.LastIndexOf('@');
            if (atIndex < 0 || atIndex == email.Length - 1)
                return null;

            return email.Substring(atIndex + 1).ToLowerInvariant();
        }

        public async Task<bool> AddBlacklist(BlacklistData blacklistData)
        {
            Blacklist blacklist = new Blacklist()
            {
                Id = Guid.NewGuid(),
                Type = blacklistData.Type,
                Value = blacklistData.Value
            };

            return await _blacklistRepository.AddBlacklist(blacklist);         
        }
    }
}
