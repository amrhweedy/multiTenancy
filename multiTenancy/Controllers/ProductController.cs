using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using multiTenancy.Dtos;
using multiTenancy.Models;
using multiTenancy.Services;

namespace multiTenancy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = await productService.GetProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> getProductById(int id)
        {
            var product = await productService.GetProductByIdAsync(id);

            return product is null ? NotFound() : Ok(product);
        }
        [HttpPost]
        public async Task<IActionResult> createdProduct(AddProductDto productDto)
        {
            Product product = new Product()
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Rate = productDto.Rate,
            };

            var createdproduct =    await productService.AddProduct(product);
            return Ok(createdproduct);
        } 
    }
}
