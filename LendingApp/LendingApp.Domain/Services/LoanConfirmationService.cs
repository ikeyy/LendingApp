using LendingApp.Domain.DTO.LoanConfirmation;
using LendingApp.Domain.Entities;
using LendingApp.Domain.Enums;
using LendingApp.Domain.Interfaces.Repository;
using LendingApp.Domain.Interfaces.Service;
using LendingApp.Domain.Utils;

namespace LendingApp.Domain.Services
{
    public class LoanConfirmationService : ILoanConfirmationService
    {
        private readonly ILoanApplicationRepository _loanApplicationRepository;

        public LoanConfirmationService(
            ILoanApplicationRepository loanApplicationRepository)
        {
            _loanApplicationRepository = loanApplicationRepository;
        }


        public async Task<LoanConfirmationResponse> SaveApplicationData(LoanConfirmationRequest request)
        {

            var application = new LoanApplication
            {
                ConfirmationNumber = NumberGenerator.GenerateConfirmationNumber(),
                QuoteId = request.QuoteId,
                ProductId = request.ProductId,
                ProductName = request.ProductName,

                // Borrower Details
                Title = request.BorrowerDetails.Title,
                FirstName = request.BorrowerDetails.FirstName,
                LastName = request.BorrowerDetails.LastName,
                DateOfBirth = request.BorrowerDetails.DateOfBirth,
                Email = request.BorrowerDetails.Email,
                MobileNumber = request.BorrowerDetails.MobileNumber,

                // Loan Details
                LoanAmount = request.LoanAmount,
                TermMonths = request.TermMonths,
                MonthlyPayment = request.MonthlyPayment,
                TotalRepayment = request.TotalRepayment,
                EstablishmentFee = request.EstablishmentFee,
                TotalInterest = request.TotalInterest,
                Frequency = request.Frequency,
                InterestFreeMonths = request.InterestFreeMonths.ToString(),

                ApplicationTimestamp = request.ApplicationTimestamp,
                Status = ApplicationStatus.Submitted,
                CreatedAt = DateTime.UtcNow,
                UserAgent = request.UserAgent
            };

            await _loanApplicationRepository.SaveLoanApplication(application);

            return new LoanConfirmationResponse
            {
                Success = true,
                ConfirmationNumber = application.ConfirmationNumber,
                ApplicationId = application.Id.ToString(),
                Message = "Your loan application has been successfully submitted!",
                Timestamp = DateTime.UtcNow,
                NextSteps = new List<string>
                    {
                        "We'll review your application within 1-2 business days",
                        "You'll receive an email confirmation shortly",
                        "Our team may contact you for additional information",
                        "Check your email for application updates"
                    },
                EstimatedProcessingTime = "1-2 business days"
            };
        }



    }
}
