namespace LendingApp.Domain.DTO.Quote
{
    public class QuoteResponse
    {
        public string QuoteId { get; set; }
        public string LoanRequestId { get; set; }
        public string Product { get; set; }
        public string ProductId { get; set; }
        public decimal LoanAmount { get; set; }
        public int Term { get; set; }
        public decimal Interest { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal MonthlyPayment { get; set; }
        public decimal EstablishmentFee { get; set; }
        public decimal InterestRate { get; set; }
        public object InterestFreeMonths { get; set; }
        public decimal MinLoan { get; set; }
        public decimal MaxLoan { get; set; }
        public int MinTerm { get; set; }
        public int MaxTerm { get; set; }
        public QuoteBorrowerDetails BorrowerDetails { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
