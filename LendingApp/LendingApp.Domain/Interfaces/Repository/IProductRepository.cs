using LendingApp.Domain.DTO.Product;
using LendingApp.Domain.Entities;

namespace LendingApp.Domain.Interfaces.Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProduct();

        Task<Product> GetProductById(Guid productId);

        Task<bool> AddProduct(AddProductData productData);
    }
}
