using LendingApp.Domain.DTO.LoanRequest;
using LendingApp.Domain.DTO.Product;
using MediatR;

namespace LendingApp.Application.Command.AddProduct
{
    public class AddProductCommand : IRequest<bool>
    {
        public AddProductData ProductData { get; set; }

        public AddProductCommand(AddProductData productData)
        {
            ProductData = productData;
        }
    }
}
