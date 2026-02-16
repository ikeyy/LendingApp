namespace LendingApp.Domain.Entities
{
    public class LoanDetails
    {
        public Guid Id { get; set; }
        public Guid BorrowerId { get; set; }
        public decimal Amount { get; set; }
        public int Term { get; set; }
    }
}
