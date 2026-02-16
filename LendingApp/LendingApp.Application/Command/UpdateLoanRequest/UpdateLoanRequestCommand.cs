using LendingApp.Domain.DTO.LoanRequest;
using MediatR;

namespace LendingApp.Application.Command.UpdateLoanRequest
{
    public class UpdateLoanRequestCommand : IRequest<LoanResponse>
    {
        public LoanRequest LoanRequest { get; set; }

        public UpdateLoanRequestCommand(LoanRequest loanRequest)
        {
            LoanRequest = loanRequest;
        }
    }
}
