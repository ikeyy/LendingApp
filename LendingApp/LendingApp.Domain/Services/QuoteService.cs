using LendingApp.Domain.DTO.LoanCalculator;
using LendingApp.Domain.DTO.Product;
using LendingApp.Domain.DTO.Quote;
using LendingApp.Domain.Entities;
using LendingApp.Domain.Interfaces.Repository;
using LendingApp.Domain.Interfaces.Service;
using LendingApp.Domain.Utils;

namespace LendingApp.Domain.Services
{
    public class QuoteService : IQuoteService
    {
        private readonly IQuoteRepository _quoteRepository;
        private readonly IProductRepository _productRepository;

        public QuoteService(
            IQuoteRepository quoteRepository,
            IProductRepository productRepository)
        {           
            _quoteRepository = quoteRepository;
            _productRepository = productRepository;
        }


        public async Task<QuoteResponse> CreateQuoteAsync(QuoteRequest request)
        {
            // Get productData configuration from database
            var product = await _productRepository.GetProductById(Guid.Parse(request.ProductId));

            if (product == null)
            {
                throw new ArgumentException($"Product with ID {request.ProductId} not found.");
            }

            var productData = new ProductData()
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
            };


            var calculation = LoanCalculator.CalculateLoan(new LoanCalculationRequest
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
          
            int? age = LoanCalculator.CalculateAge(request.BorrowerDetails.DateOfBirth);


            request.LoanRequestId = StringCipher.Decrypt(request.LoanRequestId);


            var quote = await _quoteRepository.SaveQuote(request,calculation);

            return new QuoteResponse
            {
                QuoteId = quote.Id.ToString(),
                LoanRequestId = quote.BorrowerId,
                Product = calculation.ProductName,
                ProductId = request.ProductId,
                LoanAmount = request.LoanAmount,
                Term = request.TermMonths,
                Interest = calculation.TotalInterest,
                TotalAmount = calculation.TotalRepayment,
                MonthlyPayment = calculation.PaymentAmount,
                EstablishmentFee = calculation.EstablishmentFee,
                InterestRate = calculation.InterestRate,
                InterestFreeMonths = calculation.InterestFreeMonths,
                MinLoan = product.MinLoanAmount,
                MaxLoan = product.MaxLoanAmount,
                MinTerm = product.MinTermMonths,
                MaxTerm = product.MaxTermMonths,
                BorrowerDetails = new QuoteBorrowerDetails
                {
                    Title = request.BorrowerDetails.Title,
                    FirstName = request.BorrowerDetails.FirstName,
                    LastName = request.BorrowerDetails.LastName,
                    DateOfBirth = request.BorrowerDetails.DateOfBirth,
                    Age = age,
                    Email = request.BorrowerDetails.Email,
                    MobileNumber = request.BorrowerDetails.MobileNumber
                },
                CreatedAt = quote.CreatedAt
            };
        }

        public async Task<QuoteResponse> GetQuoteByIdAsync(Guid quoteId)
        {
            var quote = await _quoteRepository.GetQuoteById(quoteId);

            int? age = LoanCalculator.CalculateAge(quote.CustomerDateOfBirth);

            return new QuoteResponse
            {
                QuoteId = quote.Id.ToString(),
                LoanRequestId = quote.BorrowerId,
                Product = quote.Product.DisplayName,
                ProductId = quote.ProductId.ToString(),
                LoanAmount = quote.LoanAmount,
                Term = quote.TermMonths,
                Interest = quote.Interest,
                TotalAmount = quote.TotalAmount,
                MonthlyPayment = quote.MonthlyPayment,
                EstablishmentFee = quote.EstablishmentFee,
                InterestRate = quote.InterestRate,
                InterestFreeMonths = quote.Product.InterestFreeMonths,
                MinLoan = quote.Product.MinLoanAmount,
                MaxLoan = quote.Product.MaxLoanAmount,
                MinTerm = quote.Product.MinTermMonths,
                MaxTerm = quote.Product.MaxTermMonths,
                BorrowerDetails = new QuoteBorrowerDetails
                {
                    Title = quote.CustomerTitle,
                    FirstName = quote.CustomerFirstName,
                    LastName = quote.CustomerLastName,
                    DateOfBirth = quote.CustomerDateOfBirth,
                    Age = age,
                    Email = quote.CustomerEmail,
                    MobileNumber = quote.CustomerMobileNumber
                },
                CreatedAt = quote.CreatedAt
            };
        }

    }
}
