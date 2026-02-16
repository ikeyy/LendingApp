using LendingApp.Domain.Interfaces.Service;
using MediatR;

namespace LendingApp.Application.Command.AddProduct
{
    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, bool>
    {
        private readonly IProductService _productService;

        public AddProductCommandHandler(
            IProductService productService)
        {
            _productService = productService;
        }

        public async Task<bool> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var isSaved = await _productService.AddProduct(request.ProductData);

            return isSaved;
        }
    }
}
