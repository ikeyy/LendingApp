using System.ComponentModel.DataAnnotations;

namespace LendingApp.Domain.DTO.Quote
{
    public class QuoteRequest
    {
        [Required]
        public string ProductId { get; set; }

        [Required]
        public string LoanRequestId { get; set; }

        [Required]
        [Range(500, 50000)]
        public decimal LoanAmount { get; set; }

        [Required]
        [Range(2, 60)]
        public int TermMonths { get; set; }

        [Required]
        public BorrowerDetails BorrowerDetails { get; set; }
    }
}
