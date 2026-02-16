using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LendingApp.Domain.Entities
{
    public class Quote
    {
        [Key]
        public Guid Id { get; set; }

        public string BorrowerId { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal LoanAmount { get; set; }

        [Required]
        public int TermMonths { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Interest { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal MonthlyPayment { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal EstablishmentFee { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal InterestRate { get; set; }

        // Customer Information
        [Required]
        [MaxLength(10)]
        public string CustomerTitle { get; set; }

        [Required]
        [MaxLength(100)]
        public string CustomerFirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string CustomerLastName { get; set; }

        [Required]
        public DateTime CustomerDateOfBirth { get; set; }

        [Required]
        [MaxLength(255)]
        [EmailAddress]
        public string CustomerEmail { get; set; }

        [Required]
        [MaxLength(20)]
        [Phone]
        public string CustomerMobileNumber { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
