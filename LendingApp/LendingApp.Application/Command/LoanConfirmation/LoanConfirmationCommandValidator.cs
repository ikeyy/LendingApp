using FluentValidation;
using LendingApp.Domain.DTO.LoanRequest;
using LendingApp.Domain.Interfaces.Service;

namespace LendingApp.Application.Command.LoanConfirmation
{
    public class LoanConfirmationCommandValidator
        : AbstractValidator<LoanConfirmationCommand>
    {
        private readonly ILoanRequestService _loanRequestService;
        private readonly IBlacklistService _blacklistService;

        public LoanConfirmationCommandValidator(
            ILoanRequestService loanRequestService,
            IBlacklistService blacklistService
            )
        {
            _loanRequestService = loanRequestService;
            _blacklistService = blacklistService;

            // Basic validation
            RuleFor(x => x.LoanConfirmationRequest.BorrowerDetails.FirstName)
                .NotEmpty()
                .WithMessage("First name is required")
                .MaximumLength(100)
                .WithMessage("First name must not exceed 100 characters");

            RuleFor(x => x.LoanConfirmationRequest.LoanAmount)
                .GreaterThan(0)
                .WithMessage("Loan amount must be greater than zero")
                .LessThanOrEqualTo(15000)
                .WithMessage("Loan amount cannot exceed $15,000");

            RuleFor(x => x.LoanConfirmationRequest.TermMonths)
                .InclusiveBetween(2, 360)
                .WithMessage("Loan term must be between 2 and 360 months");

            RuleFor(x => x.LoanConfirmationRequest.BorrowerDetails.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Valid email is required");

            //Accepts 09XXXXXXXXX, +639XXXXXXXXX, or +[country code][number]
            RuleFor(x => x.LoanConfirmationRequest.BorrowerDetails.MobileNumber)
            .NotEmpty()
            .Matches(@"^(09\d{9}|\+63\d{10}|\+\d{1,3}\d{7,14})$")
            .WithMessage("Valid mobile number is required");

            // Database validations
            RuleFor(x => x.LoanConfirmationRequest.BorrowerDetails.Email)
                .MustAsync(CheckIfEmailDomainIsBlacklisted)
                .WithMessage($"Email domain is currently blacklisted");

            RuleFor(x => x.LoanConfirmationRequest.BorrowerDetails.MobileNumber)
                .MustAsync(CheckIfMobileNumberIsBlacklisted)
                .WithMessage("Mobile number is currently blacklisted");

            RuleFor(x => x.LoanConfirmationRequest.BorrowerDetails.DateOfBirth)
                .Must(BeAtLeast18YearsOld)
                .WithMessage("Applicant must be at least 18 years old");
        }

        private async Task<bool> CheckIfEmailDomainIsBlacklisted(
            string email,
            CancellationToken cancellationToken)
        {
            return !await _blacklistService.CheckIfBlacklisted(new LoanRequest { Email = email });
        }

        private async Task<bool> CheckIfMobileNumberIsBlacklisted(
            string mobileNumber,
            CancellationToken cancellationToken)
        {
            return !await _blacklistService.CheckIfBlacklisted(new LoanRequest { Mobile = mobileNumber });
        }

        private bool BeAtLeast18YearsOld(DateTime dateOfBirth)
        {
            return dateOfBirth <= DateTime.Today.AddYears(-18);
        }
    }
}
