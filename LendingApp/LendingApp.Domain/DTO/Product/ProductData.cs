using System.ComponentModel.DataAnnotations.Schema;

namespace LendingApp.Domain.DTO.Product
{
    public class ProductData
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }

        [Column(TypeName = "decimal(18, 5)")]
        public decimal InterestRate { get; set; }
        public object InterestFreeMonths { get; set; }
        public int MinTermMonths { get; set; }
        public int MaxTermMonths { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal MinLoanAmount { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal MaxLoanAmount { get; set; }
        public int DefaultTermMonths { get; set; }
        public string Description { get; set; }
        
    }
}
