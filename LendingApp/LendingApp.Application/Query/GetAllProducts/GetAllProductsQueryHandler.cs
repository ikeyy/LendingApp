using LendingApp.Domain.DTO.Product;
using LendingApp.Domain.Interfaces.Service;
using LendingApp.Domain.Utils;
using MediatR;

namespace LendingApp.Application.Query.GetAllProducts
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<ProductData>>
    {
        private readonly IProductService _productService;

        public GetAllProductsQueryHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<List<ProductData>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var productList = await _productService.GetProductList();

            return productList;
        }
    }
}
