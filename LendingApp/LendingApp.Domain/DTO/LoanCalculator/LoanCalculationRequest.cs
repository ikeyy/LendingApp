using System.ComponentModel.DataAnnotations;

namespace LendingApp.Domain.DTO.LoanCalculator
{
    public class LoanCalculationRequest
    {
        [Required]
        public string ProductId { get; set; }

        [Required]
        [Range(500, 50000)]
        public decimal LoanAmount { get; set; }

        [Required]
        [Range(2, 60)]
        public int TermMonths { get; set; }

        public decimal? EstablishmentFee { get; set; }

        public string PaymentFrequency { get; set; } = "Monthly"; 
    }
}
