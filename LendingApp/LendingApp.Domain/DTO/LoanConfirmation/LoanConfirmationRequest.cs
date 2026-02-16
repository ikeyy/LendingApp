using LendingApp.Domain.DTO.Quote;
using System.ComponentModel.DataAnnotations.Schema;

namespace LendingApp.Domain.DTO.LoanConfirmation
{
    public class LoanConfirmationRequest
    {
        public Guid QuoteId { get; set; }

        public Guid ProductId { get; set; }

        public string ProductName { get; set; }

        public BorrowerDetails BorrowerDetails { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal LoanAmount { get; set; }

        public int TermMonths { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal MonthlyPayment { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalRepayment { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal EstablishmentFee { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalInterest { get; set; }

        public string Frequency { get; set; }

        public object InterestFreeMonths { get; set; }

        public DateTime ApplicationTimestamp { get; set; }

        public string UserAgent { get; set; }

    }
}
