using LendingApp.Domain.Constants;
using LendingApp.Domain.DTO.LoanRequest;
using LendingApp.Domain.Interfaces;
using LendingApp.Domain.Interfaces.Repository;
using LendingApp.Domain.Interfaces.Service;
using LendingApp.Domain.Utils;

namespace LendingApp.Domain.Services
{
    public class LoanRequestService: ILoanRequestService
    {
        private readonly IBorrowerRepository _borrowerRepository;
        private readonly ILoanDetailsRepository _loanDetailsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LoanRequestService(
            IBorrowerRepository borrowerRepository,
            ILoanDetailsRepository loanDetailsRepository,
            IUnitOfWork unitOfWork)
        {
            _borrowerRepository = borrowerRepository;
            _loanDetailsRepository = loanDetailsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BorrowerData> GetBorrowerByNameAndDateOfBirth(LoanRequest loanRequest, CancellationToken cancellation)
        {
            var result = await _borrowerRepository.GetBorrowerByNameAndDateOfBirth(loanRequest.FirstName,loanRequest.LastName,loanRequest.DateOfBirth,cancellation);

            if (result != null)
            {
                var borrowerData = new BorrowerData()
                {
                    Title = result.Title,
                    FirstName = result.FirstName,
                    LastName = result.LastName,
                    DateOfBirth = result.DateOfBirth,
                    Email = result.Email,
                    Mobile = result.Mobile
                };
                return borrowerData;
            }
            else
            {
                return null;
            }
        }

        public async Task<BorrowerData> GetBorrowerById(Guid borrowerId, CancellationToken cancellation)
        {
            var borrower = await _borrowerRepository.GetBorrowerById(borrowerId, cancellation);
            var loanDetails = await _loanDetailsRepository.GetLoanDetailsById(borrowerId, cancellation);

            var borrowerData = new BorrowerData()
            {             
                Title = borrower.Title,
                FirstName = borrower.FirstName,
                LastName = borrower.LastName,
                DateOfBirth = borrower.DateOfBirth,
                Email = borrower.Email,
                Mobile = borrower.Mobile,
                LoanAmount = loanDetails.Amount,
                Term = loanDetails.Term
            };

            return borrowerData;
        }

        public async Task SaveBorrowerData(LoanRequest loanRequest)
        {
            var borrowerId = await _borrowerRepository.AddBorrower(loanRequest);
            await _loanDetailsRepository.AddLoanDetails(loanRequest,borrowerId);
            await _unitOfWork.SaveChangesAsync();           
        }

        public async Task UpdateBorrowerData(LoanRequest loanRequest)
        {
            var borrowerId = await _borrowerRepository.UpdateBorrower(loanRequest);
            await _loanDetailsRepository.UpdateLoanDetails(loanRequest, borrowerId);
            await _unitOfWork.SaveChangesAsync();
        }


        public async Task<LoanResponse> CreateRedirectURL(LoanRequest loanRequest, CancellationToken cancellationToken = default)
        {
            var result = await _borrowerRepository.GetBorrowerByNameAndDateOfBirth(loanRequest.FirstName, loanRequest.LastName, loanRequest.DateOfBirth, cancellationToken);
            return new LoanResponse() { redirectURL = $"{URLConstants.DomainRedirectURL}?Id={StringCipher.Encrypt(result.Id.ToString())}"};
        }

        public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _borrowerRepository.IsBorrowerEmailExist(email, cancellationToken);
        }
        
    }
}
