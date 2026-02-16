using FluentValidation;
using LendingApp.Domain.Interfaces.Service;

namespace LendingApp.Application.Command.CreateLoanRequest
{
    public class CreateLoanRequestCommandValidator
        : AbstractValidator<CreateLoanRequestCommand>
    {
        private readonly ILoanRequestService _loanRequestService;

        public CreateLoanRequestCommandValidator(
            ILoanRequestService loanRequestService)
        {
            _loanRequestService = loanRequestService;

            // Basic validation
            RuleFor(x => x.LoanRequest.FirstName)
                .NotEmpty()
                .WithMessage("First name is required")
                .MaximumLength(100)
                .WithMessage("First name must not exceed 100 characters");

            RuleFor(x => x.LoanRequest.AmountRequired)
                .GreaterThan(0)
                .WithMessage("Loan amount must be greater than zero")
                .LessThanOrEqualTo(15000)
                .WithMessage("Loan amount cannot exceed $15,000");

            RuleFor(x => x.LoanRequest.Term)
                .InclusiveBetween(2, 360)
                .WithMessage("Loan term must be between 2 and 360 months");

            RuleFor(x => x.LoanRequest.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Valid email is required");

            //Accepts 09XXXXXXXXX, +639XXXXXXXXX, or +[country code][number]
            RuleFor(x => x.LoanRequest.Mobile)
            .NotEmpty()
            .Matches(@"^(09\d{9}|\+63\d{10}|\+\d{1,3}\d{7,14})$")
            .WithMessage("Valid mobile number is required");

            // Database validations
            RuleFor(x => x.LoanRequest.Email)
                .MustAsync(BeUniqueEmailIfNewBorrower)
                .WithMessage("Email is already registered with a different borrower");
        }

               
        private async Task<bool> BeUniqueEmailIfNewBorrower(
            CreateLoanRequestCommand command,
            string email,
            CancellationToken cancellationToken)
        {
            var borrower = await _loanRequestService.GetBorrowerByNameAndDateOfBirth(
                command.LoanRequest,
                cancellationToken);

            if (borrower != null)
                return true; // Existing borrower, email check not needed

            return !await _loanRequestService.EmailExistsAsync(email, cancellationToken);
        }
    }
}
