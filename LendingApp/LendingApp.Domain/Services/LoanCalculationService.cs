using LendingApp.Domain.DTO.LoanCalculator;
using LendingApp.Domain.DTO.Product;
using LendingApp.Domain.Interfaces.Repository;
using LendingApp.Domain.Interfaces.Service;
using LendingApp.Domain.Utils;

namespace LendingApp.Domain.Services
{
    public class LoanCalculationService : ILoanCalculationService
    {
        private readonly IProductRepository _productRepository;

        public LoanCalculationService(
            IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }


        public async Task<LoanCalculationResponse> CalculateLoan(LoanCalculationRequest request)
        {
            // Get product configuration from database
            var product = await _productRepository.GetProductById(Guid.Parse(request.ProductId));

            if (product == null)
            {
                throw new ArgumentException($"Product with ID {request.ProductId} not found.");
            }

            var loanCalculationResponse = LoanCalculator.CalculateLoan(new LoanCalculationRequest
            {
                ProductId = request.ProductId,
                LoanAmount = request.LoanAmount,
                TermMonths = request.TermMonths,
                PaymentFrequency = "Monthly"
            },
            new ProductData()
            {
                Id = product.Id,
                DisplayName = product.DisplayName,
                DefaultTermMonths = product.DefaultTermMonths,
                InterestFreeMonths = product.InterestFreeMonths,
                MinTermMonths = product.MinTermMonths,
                MaxTermMonths = product.MaxTermMonths,
                MinLoanAmount = product.MinLoanAmount,
                MaxLoanAmount = product.MaxLoanAmount,
                Description = product.Description,
            });

            return loanCalculationResponse;

        }

    }
}
