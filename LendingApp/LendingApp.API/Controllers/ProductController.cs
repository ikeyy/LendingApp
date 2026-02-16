using LendingApp.Application.Command.AddProduct;
using LendingApp.Application.Query.GetAllProducts;
using LendingApp.Domain.DTO.Product;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LendingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<List<ProductData>> Get()
        {
            var query = new GetAllProductsQuery();

            var result = await _mediator.Send(query);
            return result;
        }

        [HttpPost("create")]
        public async Task<bool> Post([FromBody] AddProductData productData)
        {
            var command = new AddProductCommand(productData);
            var result = await _mediator.Send(command);
            return result;
        }
    }
}
