using LendingApp.Domain.DTO.Product;

namespace LendingApp.Domain.Interfaces.Service
{
    public interface IProductService
    {
        Task<List<ProductData>> GetProductList();
        Task<bool> AddProduct(AddProductData product);
    }
}
