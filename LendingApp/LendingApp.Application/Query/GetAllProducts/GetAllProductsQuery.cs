using LendingApp.Domain.DTO.Product;
using MediatR;

namespace LendingApp.Application.Query.GetAllProducts
{
    public class GetAllProductsQuery : IRequest<List<ProductData>>
    {
        public GetAllProductsQuery()
        {
        }
    }
}
