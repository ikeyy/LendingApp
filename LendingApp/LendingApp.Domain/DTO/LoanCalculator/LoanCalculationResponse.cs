namespace LendingApp.Domain.DTO.LoanCalculator
{
    public class LoanCalculationResponse
    {
        public decimal LoanAmount { get; set; }
        public int TermMonths { get; set; }
        public decimal EstablishmentFee { get; set; }
        public decimal InterestRate { get; set; }
        public object InterestFreeMonths { get; set; } // Can be int or "all"
        public decimal TotalInterest { get; set; }
        public decimal TotalRepayment { get; set; }
        public decimal PaymentAmount { get; set; }
        public string PaymentFrequency { get; set; }
        public int PaymentsPerYear { get; set; }
        public int TotalPayments { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public DateTime CalculatedAt { get; set; }
    }
}
