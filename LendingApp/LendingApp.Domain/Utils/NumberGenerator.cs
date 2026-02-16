
namespace LendingApp.Domain.Utils
{
    public class NumberGenerator
    {
        public static string GenerateConfirmationNumber()
        {
            var datePart = DateTime.UtcNow.ToString("yyyyMMdd");
            var randomPart = Guid.NewGuid().ToString("N")[..4].ToUpper();
            return $"LN-{datePart}-{randomPart}";
        }
    }
}
