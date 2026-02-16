using LendingApp.Domain.DTO.LoanRequest;
using LendingApp.Domain.Interfaces.Service;
using LendingApp.Domain.Utils;
using MediatR;
using System.Web;

namespace LendingApp.Application.Query.GetLoanRequestById
{
    public class GetLoanRequestByIdQueryHandler : IRequestHandler<GetLoanRequestByIdQuery, BorrowerData>
    {
        private readonly ILoanRequestService _loanRequestService;

        public GetLoanRequestByIdQueryHandler(ILoanRequestService loanRequestService)
        {
            _loanRequestService = loanRequestService;
        }

        public async Task<BorrowerData> Handle(GetLoanRequestByIdQuery request, CancellationToken cancellationToken)
        {
            var borrowerId = StringCipher.Decrypt(request.BorrowerId);
            var borrowerData = await _loanRequestService.GetBorrowerById(Guid.Parse(borrowerId),cancellationToken); 

            return borrowerData;
        }
    }
}
