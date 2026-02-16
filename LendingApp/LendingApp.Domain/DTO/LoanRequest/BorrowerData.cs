namespace LendingApp.Domain.DTO.LoanRequest
{
    public class BorrowerData
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public decimal LoanAmount { get; set; }
        public int Term {  get; set; }
    }
}
