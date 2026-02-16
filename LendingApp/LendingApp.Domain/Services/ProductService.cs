using LendingApp.Domain.DTO.Product;
using LendingApp.Domain.Entities;
using LendingApp.Domain.Interfaces.Repository;
using LendingApp.Domain.Interfaces.Service;

namespace LendingApp.Domain.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(
            IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<List<ProductData>> GetProductList()
        {
            List<Product> result = await _productRepository.GetAllProduct();

            var productList = result.Select(p => new ProductData
            {
                Id = p.Id,
                DisplayName = p.DisplayName,
                InterestRate = p.InterestRate,
                InterestFreeMonths = p.InterestFreeMonths,
                MinTermMonths = p.MinTermMonths,
                MaxTermMonths = p.MaxTermMonths,
                DefaultTermMonths = p.DefaultTermMonths,
                Description = p.Description,
                MaxLoanAmount = p.MaxLoanAmount,
                MinLoanAmount = p.MinLoanAmount          
            }).ToList();

            return productList;
        }

        public async Task<bool> AddProduct(AddProductData product)
        {
            var isSaved = await _productRepository.AddProduct(product);
            return isSaved;
        }
    }
}
