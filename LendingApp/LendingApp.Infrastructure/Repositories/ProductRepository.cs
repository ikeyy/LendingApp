using LendingApp.Domain.DTO.LoanRequest;
using LendingApp.Domain.DTO.Product;
using LendingApp.Domain.Entities;
using LendingApp.Domain.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace LendingApp.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllProduct()
        {
            var result = await _context.Product.ToListAsync();
            return result == null ? throw new ArgumentException($"No records on product") : result;
        }

        public async Task<Product> GetProductById(Guid productId)
        {
            var result = await _context.Product.Where(x => x.Id == productId).FirstOrDefaultAsync();
            return result == null ? throw new ArgumentException($"Product with ID {productId} not found.") : result;
        }

        public async Task<bool> AddProduct(AddProductData productData)
        {
            Product product = new Product()
            {
                Id = Guid.NewGuid(),
                DisplayName = productData.DisplayName,
                InterestFreeMonths = productData.InterestFreeMonths.ToString(),
                InterestRate = productData.InterestRate,
                MinTermMonths = productData.MinTermMonths,
                MaxTermMonths = productData.MaxTermMonths,
                MinLoanAmount = productData.MinLoanAmount,
                MaxLoanAmount = productData.MaxLoanAmount,
                DefaultTermMonths = productData.DefaultTermMonths,
                Description = productData.Description
            };
            _context.Product.Add(product);
            var isSaved = await _context.SaveChangesAsync() > 0 ? true : false;
            return isSaved;
        }
    }
}
