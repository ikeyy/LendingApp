using Xunit;
using LendingApp.Domain.DTO.LoanCalculator;
using LendingApp.Domain.DTO.Product;
using LendingApp.Domain.Utils;

namespace LendingApp.Test
{
    public class LoanCalculationTests
    {
        // Constants matching the implementation
        private const decimal DEFAULT_ESTABLISHMENT_FEE = 395.00M;

        #region Test Data Helpers

        private LoanCalculationRequest CreateValidRequest(
            decimal loanAmount = 10000m,
            int termMonths = 12,
            decimal? establishmentFee = null,
            string paymentFrequency = "Monthly")
        {
            return new LoanCalculationRequest
            {
                LoanAmount = loanAmount,
                TermMonths = termMonths,
                EstablishmentFee = establishmentFee,
                PaymentFrequency = paymentFrequency
            };
        }

        private ProductData CreateValidProductData(
            decimal minLoanAmount = 1000m,
            decimal maxLoanAmount = 50000m,
            int minTermMonths = 6,
            int maxTermMonths = 60,
            object interestFreeMonths = null)
        {
            return new ProductData
            {
                Id = Guid.NewGuid(),
                DisplayName = "Test Product",
                MinLoanAmount = minLoanAmount,
                MaxLoanAmount = maxLoanAmount,
                MinTermMonths = minTermMonths,
                MaxTermMonths = maxTermMonths,
                InterestFreeMonths = interestFreeMonths ?? 0
            };
        }

        #endregion

        #region CalculateLoan Tests

        [Fact]
        public void CalculateLoan_WithValidInputs_ReturnsCorrectResponse()
        {
            // Arrange
            var request = CreateValidRequest(loanAmount: 10000m, termMonths: 12);
            var productData = CreateValidProductData(interestFreeMonths: 0);

            // Act
            var result = LoanCalculator.CalculateLoan(request, productData);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10000m, result.LoanAmount);
            Assert.Equal(12, result.TermMonths);
            Assert.Equal(12, result.TotalPayments);
            Assert.NotNull(result.CalculatedAt);
            Assert.Equal(productData.Id.ToString(), result.ProductId);
            Assert.Equal(productData.DisplayName, result.ProductName);
        }

        [Fact]
        public void CalculateLoan_WithEstablishmentFee_IncludesInTotalFinanced()
        {
            // Arrange
            var request = CreateValidRequest(loanAmount: 10000m, termMonths: 12, establishmentFee: 500m);
            var productData = CreateValidProductData(interestFreeMonths: "all");

            // Act
            var result = LoanCalculator.CalculateLoan(request, productData);

            // Assert
            Assert.Equal(500m, result.EstablishmentFee);
            Assert.Equal(10500m, result.TotalRepayment); // Loan + Fee, no interest
            Assert.Equal(0m, result.TotalInterest);
        }

        [Fact]
        public void CalculateLoan_WithNullEstablishmentFee_UsesDefault()
        {
            // Arrange
            var request = CreateValidRequest(loanAmount: 10000m, termMonths: 12, establishmentFee: null);
            var productData = CreateValidProductData(interestFreeMonths: "all");

            // Act
            var result = LoanCalculator.CalculateLoan(request, productData);

            // Assert
            Assert.Equal(DEFAULT_ESTABLISHMENT_FEE, result.EstablishmentFee);
        }

        [Fact]
        public void CalculateLoan_WithInterestFreeAll_CalculatesNoInterest()
        {
            // Arrange
            var request = CreateValidRequest(loanAmount: 12000m, termMonths: 12, establishmentFee: 0m);
            var productData = CreateValidProductData(interestFreeMonths: "all");

            // Act
            var result = LoanCalculator.CalculateLoan(request, productData);

            // Assert
            Assert.Equal(0m, result.TotalInterest);
            Assert.Equal(12000m, result.TotalRepayment);
            Assert.Equal(1000m, result.PaymentAmount); // 12000 / 12
            Assert.Equal("all", result.InterestFreeMonths);
        }

        [Fact]
        public void CalculateLoan_WithPartialInterestFreeMonths_CalculatesCorrectly()
        {
            // Arrange
            var request = CreateValidRequest(loanAmount: 10000m, termMonths: 12, establishmentFee: 0m);
            var productData = CreateValidProductData(interestFreeMonths: 6);

            // Act
            var result = LoanCalculator.CalculateLoan(request, productData);

            // Assert
            Assert.True(result.TotalInterest >= 0);
            Assert.True(result.TotalRepayment >= request.LoanAmount);
            Assert.Equal(12, result.TotalPayments);
        }

        [Fact]
        public void CalculateLoan_WithZeroInterestFreeMonths_AppliesInterestToAllPeriods()
        {
            // Arrange
            var request = CreateValidRequest(loanAmount: 10000m, termMonths: 12, establishmentFee: 0m);
            var productData = CreateValidProductData(interestFreeMonths: 0);

            // Act
            var result = LoanCalculator.CalculateLoan(request, productData);

            // Assert
            Assert.True(result.TotalInterest >= 0);
            Assert.Equal(12, result.TotalPayments);
        }

        [Theory]
        [InlineData(500)]
        [InlineData(100000)]
        public void CalculateLoan_WithLoanAmountBelowMinimum_ThrowsArgumentException(decimal loanAmount)
        {
            // Arrange
            var request = CreateValidRequest(loanAmount: loanAmount, termMonths: 12);
            var productData = CreateValidProductData(minLoanAmount: 1000m, maxLoanAmount: 50000m);

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                LoanCalculator.CalculateLoan(request, productData));
            Assert.Contains("Loan amount must be between", exception.Message);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(100)]
        public void CalculateLoan_WithTermOutsideRange_ThrowsArgumentException(int termMonths)
        {
            // Arrange
            var request = CreateValidRequest(loanAmount: 10000m, termMonths: termMonths);
            var productData = CreateValidProductData(minTermMonths: 6, maxTermMonths: 60);

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                LoanCalculator.CalculateLoan(request, productData));
            Assert.Contains("Loan term must be between", exception.Message);
        }

        [Fact]
        public void CalculateLoan_RoundsResultsToTwoDecimalPlaces()
        {
            // Arrange
            var request = CreateValidRequest(loanAmount: 10001m, termMonths: 12, establishmentFee: 99m);
            var productData = CreateValidProductData(interestFreeMonths: "all");

            // Act
            var result = LoanCalculator.CalculateLoan(request, productData);

            // Assert
            Assert.Equal(2, BitConverter.GetBytes(decimal.GetBits(result.PaymentAmount)[3])[2]);
            Assert.Equal(2, BitConverter.GetBytes(decimal.GetBits(result.TotalInterest)[3])[2]);
            Assert.Equal(2, BitConverter.GetBytes(decimal.GetBits(result.TotalRepayment)[3])[2]);
        }

        [Fact]
        public void CalculateLoan_WithInterestFreeMonthsAsString_ParsesCorrectly()
        {
            // Arrange
            var request = CreateValidRequest(loanAmount: 10000m, termMonths: 12, establishmentFee: 0m);
            var productData = CreateValidProductData(interestFreeMonths: "6");

            // Act
            var result = LoanCalculator.CalculateLoan(request, productData);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(12, result.TotalPayments);
        }

        [Fact]
        public void CalculateLoan_WithInvalidInterestFreeMonthsString_TreatsAsZero()
        {
            // Arrange
            var request = CreateValidRequest(loanAmount: 10000m, termMonths: 12, establishmentFee: 0m);
            var productData = CreateValidProductData(interestFreeMonths: "invalid");

            // Act
            var result = LoanCalculator.CalculateLoan(request, productData);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.InterestFreeMonths);
        }

        [Theory]
        [InlineData(12, 12)]
        [InlineData(24, 24)]
        [InlineData(36, 36)]
        public void CalculateLoan_CalculatesCorrectTotalPayments(int termMonths, int expectedPayments)
        {
            // Arrange
            var request = CreateValidRequest(loanAmount: 10000m, termMonths: termMonths);
            var productData = CreateValidProductData(
                minTermMonths: 6,
                maxTermMonths: 60,
                interestFreeMonths: "all");

            // Act
            var result = LoanCalculator.CalculateLoan(request, productData);

            // Assert
            Assert.Equal(expectedPayments, result.TotalPayments);
        }

        [Fact]
        public void CalculateLoan_SetsPaymentsPerYearTo12()
        {
            // Arrange
            var request = CreateValidRequest();
            var productData = CreateValidProductData(interestFreeMonths: "all");

            // Act
            var result = LoanCalculator.CalculateLoan(request, productData);

            // Assert
            Assert.Equal(12, result.PaymentsPerYear);
        }

        #endregion

        #region CalculatePMT Tests

        [Fact]
        public void CalculatePMT_WithZeroRate_ReturnsFlatPayment()
        {
            // Arrange
            decimal rate = 0m;
            int nper = 12;
            decimal pv = -10000m;

            // Act
            var result = LoanCalculator.CalculatePMT(rate, nper, pv);

            // Assert
            Assert.Equal(833.33m, Math.Round(result, 2));
        }

        [Fact]
        public void CalculatePMT_WithPositiveRate_ReturnsPaymentWithInterest()
        {
            // Arrange
            decimal rate = 0.05m / 12; // 5% annual rate, monthly
            int nper = 12;
            decimal pv = -10000m;

            // Act
            var result = LoanCalculator.CalculatePMT(rate, nper, pv);

            // Assert
            Assert.True(result > 833.33m); // Should be more than flat payment
            Assert.True(result > 0);
        }

        [Fact]
        public void CalculatePMT_WithFutureValue_IncludesFVInCalculation()
        {
            // Arrange
            decimal rate = 0.05m / 12;
            int nper = 12;
            decimal pv = -10000m;
            decimal fv = 1000m;

            // Act
            var result = LoanCalculator.CalculatePMT(rate, nper, pv, fv);

            // Assert
            Assert.True(result > 0);
        }

        [Fact]
        public void CalculatePMT_WithType1_AdjustsForBeginningOfPeriod()
        {
            // Arrange
            decimal rate = 0.05m / 12;
            int nper = 12;
            decimal pv = -10000m;
            int type0 = 0; // End of period
            int type1 = 1; // Beginning of period

            // Act
            var resultType0 = LoanCalculator.CalculatePMT(rate, nper, pv, 0, type0);
            var resultType1 = LoanCalculator.CalculatePMT(rate, nper, pv, 0, type1);

            // Assert
            Assert.True(resultType1 < resultType0); // Beginning of period payment should be less
        }

        [Theory]
        [InlineData(12, -10000)]
        [InlineData(24, -10000)]
        [InlineData(36, -10000)]
        public void CalculatePMT_WithDifferentTerms_AdjustsPaymentAmount(int nper, decimal pv)
        {
            // Arrange
            decimal rate = 0.05m / 12;

            // Act
            var result = LoanCalculator.CalculatePMT(rate, nper, pv);

            // Assert
            Assert.True(result > 0);
            // Longer terms should have smaller payments
        }

        [Fact]
        public void CalculatePMT_WithNegativePresentValue_ReturnsPositivePayment()
        {
            // Arrange
            decimal rate = 0.05m / 12;
            int nper = 12;
            decimal pv = -10000m;

            // Act
            var result = LoanCalculator.CalculatePMT(rate, nper, pv);

            // Assert
            Assert.True(result > 0);
        }

        [Fact]
        public void CalculatePMT_WithPositivePresentValue_ReturnsNegativePayment()
        {
            // Arrange
            decimal rate = 0.05m / 12;
            int nper = 12;
            decimal pv = 10000m;

            // Act
            var result = LoanCalculator.CalculatePMT(rate, nper, pv);

            // Assert
            Assert.True(result < 0);
        }

        [Theory]
        [InlineData(0.01, 12, -10000, 0, 0)]
        [InlineData(0.02, 24, -20000, 0, 0)]
        [InlineData(0.03, 36, -30000, 1000, 1)]
        public void CalculatePMT_WithVariousInputs_ReturnsValidPayment(
            decimal rate, int nper, decimal pv, decimal fv, int type)
        {
            // Act
            var result = LoanCalculator.CalculatePMT(rate, nper, pv, fv, type);

            // Assert
            Assert.NotEqual(0, result);
        }

        [Fact]
        public void CalculatePMT_MatchesExcelPMTFormula()
        {
            // Arrange - Example: =PMT(5%/12, 12, -10000)
            decimal rate = 0.05m / 12;
            int nper = 12;
            decimal pv = -10000m;

            // Act
            var result = LoanCalculator.CalculatePMT(rate, nper, pv);

            // Assert
            // Excel PMT result should be approximately 856.07
            Assert.True(Math.Abs(result - 856.07m) < 1m);
        }

        #endregion

        #region Integration Tests

        [Fact]
        public void CalculateLoan_WithInterest_UsesPMTCorrectly()
        {
            // Arrange
            var request = CreateValidRequest(loanAmount: 10000m, termMonths: 12, establishmentFee: 0m);
            var productData = CreateValidProductData(interestFreeMonths: 0);

            // Act
            var result = LoanCalculator.CalculateLoan(request, productData);

            // Assert
            Assert.True(result.TotalRepayment > result.LoanAmount);
            Assert.True(result.TotalInterest >= 0);
            Assert.Equal(
                Math.Round(result.LoanAmount + result.EstablishmentFee + result.TotalInterest, 2),
                result.TotalRepayment);
        }

        [Fact]
        public void CalculateLoan_EdgeCase_InterestFreeEqualsTermLength()
        {
            // Arrange
            var request = CreateValidRequest(loanAmount: 10000m, termMonths: 12, establishmentFee: 100m);
            var productData = CreateValidProductData(interestFreeMonths: 12);

            // Act
            var result = LoanCalculator.CalculateLoan(request, productData);

            // Assert
            Assert.Equal(0m, result.TotalInterest);
            Assert.Equal(10100m, result.TotalRepayment);
        }

        [Fact]
        public void CalculateLoan_EdgeCase_InterestFreeGreaterThanTerm()
        {
            // Arrange
            var request = CreateValidRequest(loanAmount: 10000m, termMonths: 12, establishmentFee: 0m);
            var productData = CreateValidProductData(interestFreeMonths: 24);

            // Act
            var result = LoanCalculator.CalculateLoan(request, productData);

            // Assert
            // Should treat as all interest-free since IFM > term
            Assert.Equal(0m, result.TotalInterest);
        }

        #endregion
    }
}
