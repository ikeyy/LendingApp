using LendingApp.Domain.DTO.LoanCalculator;
using LendingApp.Domain.DTO.Product;
using Microsoft.VisualBasic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LendingApp.Domain.Utils
{
    public class LoanCalculator
    {
        private const decimal DEFAULT_ANNUAL_INTEREST_RATE = 0.0599m;
        private const decimal DEFAULT_ESTABLISHMENT_FEE = 395.00m;

        public static LoanCalculationResponse CalculateLoan(LoanCalculationRequest request, ProductData productData)
        {
            // Validate loan amount and term against productData limits
            if (request.LoanAmount < productData.MinLoanAmount || request.LoanAmount > productData.MaxLoanAmount)
            {
                throw new ArgumentException($"Loan amount must be between {productData.MinLoanAmount} and {productData.MaxLoanAmount}");
            }

            if (request.TermMonths < productData.MinTermMonths || request.TermMonths > productData.MaxTermMonths)
            {
                throw new ArgumentException($"Loan term must be between {productData.MinTermMonths} and {productData.MaxTermMonths} months");
            }

            // Get establishment fee
            decimal establishmentFee = request.EstablishmentFee ?? DEFAULT_ESTABLISHMENT_FEE;

            // Determine payments per year
            int paymentsPerYear = 12;
            int ifm = 0;

            // Calculate interest-free months
            var interestFreeMonths = productData.InterestFreeMonths;
            var interestFreeMonthsIsInt = int.TryParse(interestFreeMonths.ToString(), out ifm);
            int interestFreeMonthsInt = interestFreeMonths.Equals("all") ? request.TermMonths :
                                       (interestFreeMonthsIsInt ? ifm : 0);

            // Calculate months with interest
            int monthsWithInterest = Math.Max(0, request.TermMonths - interestFreeMonthsInt);

            // Total financed amount (principal + establishment fee)
            decimal totalFinanced = request.LoanAmount + establishmentFee;

            // Calculate total payments
            int totalPayments = (request.TermMonths * paymentsPerYear) / 12;

            decimal paymentAmount;
            decimal totalInterest;

            if (interestFreeMonths == "all" || monthsWithInterest == 0)
            {
                // No interest - just divide total by number of payments
                paymentAmount = totalFinanced / totalPayments;
                totalInterest = 0.00m;
            }
            else
            {
                // Calculate using PMT formula for interest-bearing portion
                decimal periodicRate = DEFAULT_ANNUAL_INTEREST_RATE / paymentsPerYear;
                int paymentsWithInterest = (monthsWithInterest * paymentsPerYear) / 12;

                // Use PMT calculation
                decimal interestPayment = CalculatePMT(periodicRate, paymentsWithInterest, -totalFinanced);

                // Total paid over interest period
                decimal totalPaidWithInterest = interestPayment * paymentsWithInterest;

                // Interest is the difference
                totalInterest = totalPaidWithInterest - totalFinanced;

                // Average payment across all periods
                paymentAmount = totalPaidWithInterest / totalPayments;
            }

            decimal totalRepayment = totalFinanced + totalInterest;

            productData.InterestFreeMonths = productData.InterestFreeMonths.Equals("all") ? "all" :
                                       (interestFreeMonthsIsInt ? ifm : 0);

            return new LoanCalculationResponse
            {
                LoanAmount = request.LoanAmount,
                TermMonths = request.TermMonths,
                EstablishmentFee = establishmentFee,
                InterestRate = DEFAULT_ANNUAL_INTEREST_RATE,
                InterestFreeMonths = productData.InterestFreeMonths,
                TotalInterest = Math.Round(totalInterest, 2),
                TotalRepayment = Math.Round(totalRepayment, 2),
                PaymentAmount = Math.Round(paymentAmount, 2),
                PaymentFrequency = request.PaymentFrequency,
                PaymentsPerYear = paymentsPerYear,
                TotalPayments = totalPayments,
                ProductId = productData.Id.ToString(),
                ProductName = productData.DisplayName,
                CalculatedAt = DateTime.UtcNow
            };
        }

        public static decimal CalculatePMT(decimal rate, int nper, decimal pv, decimal fv = 0, int type = 0)
        {
            if (rate == 0)
            {
                return -(pv + fv) / nper;
            }

            decimal pvif = (decimal)Math.Pow((double)(1 + rate), nper);
            decimal payment = (rate * (pv * pvif + fv)) / (pvif - 1);

            if (type == 1)
            {
                payment /= (1 + rate);
            }

            return -payment;
        }


        public static int? CalculateAge(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > today.AddYears(-age))
            {
                age--;
            }
            return age;
        }
    }
}
