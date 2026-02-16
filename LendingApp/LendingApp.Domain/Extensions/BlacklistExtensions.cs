using LendingApp.Domain.Enums;

namespace LendingApp.Domain.Extensions
{
    public static class BlacklistTypeExtensions
    {
        public static string[] GetAllValuesToArray()
        {
            return Enum.GetValues<BlacklistType>()
                .Select(x => x.ToString())
                .ToArray();
        }
    }
}
