namespace LendingApp.Domain.DTO.Quote
{
    public class QuoteBorrowerDetails
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int? Age { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
    }
}
