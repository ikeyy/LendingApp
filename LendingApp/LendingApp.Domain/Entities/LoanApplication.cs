using LendingApp.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LendingApp.Domain.Entities
{
    public class LoanApplication
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string ConfirmationNumber { get; set; } = string.Empty;

        [MaxLength(50)]
        public Guid? QuoteId { get; set; }

        [Required]
        [MaxLength(50)]
        public Guid ProductId { get; set; }

        [Required]
        [MaxLength(200)]
        public string ProductName { get; set; } = string.Empty;

        #region Borrower Details

        [Required]
        [MaxLength(10)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Borrower first name
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Borrower last name
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Borrower date of birth
        /// </summary>
        [Required]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Borrower mobile number
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string MobileNumber { get; set; } = string.Empty;

        /// <summary>
        /// Borrower email address
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        #endregion

        #region Loan Details

        /// <summary>
        /// Principal loan amount
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal LoanAmount { get; set; }

        /// <summary>
        /// Loan term in months
        /// </summary>
        [Required]
        public int TermMonths { get; set; }

        /// <summary>
        /// Monthly payment amount
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MonthlyPayment { get; set; }

        /// <summary>
        /// Total repayment amount
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalRepayment { get; set; }

        /// <summary>
        /// Establishment fee
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal EstablishmentFee { get; set; }

        /// <summary>
        /// Total interest
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalInterest { get; set; }

        /// <summary>
        /// Payment frequency
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Frequency { get; set; } = "Monthly";

        /// <summary>
        /// Interest-free months (stored as JSON or string)
        /// </summary>
        [MaxLength(50)]
        public string? InterestFreeMonths { get; set; }

        #endregion
        #region Status and Audit

        /// <summary>
        /// Current application status
        /// </summary>
        [Required]
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Submitted;

        /// <summary>
        /// Timestamp when application was submitted by user
        /// </summary>
        [Required]
        public DateTime ApplicationTimestamp { get; set; }

        /// <summary>
        /// Timestamp when record was created in database
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Timestamp when record was last updated
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// User agent string
        /// </summary>
        [MaxLength(500)]
        public string? UserAgent { get; set; }

        #endregion
    }
}