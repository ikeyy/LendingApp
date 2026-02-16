using LendingApp.Domain.DTO.LoanRequest;
using MediatR;

namespace LendingApp.Application.Command.CreateLoanRequest
{
    public class CreateLoanRequestCommand : IRequest<LoanResponse>
    {
        public LoanRequest LoanRequest { get; set; }

        public CreateLoanRequestCommand(LoanRequest loanRequest)
        {
            LoanRequest = loanRequest;
        }
    }
}
