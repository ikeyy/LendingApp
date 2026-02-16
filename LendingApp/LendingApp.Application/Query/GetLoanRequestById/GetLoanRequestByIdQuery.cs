using LendingApp.Domain.DTO.LoanRequest;
using MediatR;

namespace LendingApp.Application.Query.GetLoanRequestById
{
    public class GetLoanRequestByIdQuery : IRequest<BorrowerData>
    {
        public string BorrowerId { get; set; }

        public GetLoanRequestByIdQuery(string borrowerId)
        {
            BorrowerId = borrowerId;
        }
    }
}
