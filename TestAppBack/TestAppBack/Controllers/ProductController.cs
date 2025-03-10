using CurbeCorporativo.Api.Middlewares;
using Microsoft.AspNetCore.Mvc;
using TestAppBack.Models;
using TestAppBack.Repositories;
using TestAppBack.Services;

namespace TestAppBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpPost, Route("save-product")]
        public IActionResult addAssing([FromBody] ProductModel product)
        {
            try
            {
                return Ok(new ResponseResult<ProductModel>(_productService.saveProduct(product)));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPut, Route("change-status-product")]
        public IActionResult changeStatusCustomer([FromBody] ProductModel product)
        {
            try
            {
                return Ok(new ResponseResult<ProductModel>(_productService.changeStatusProduct(product)));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpGet, Route("get-products")]
        public IActionResult getAllProducts([FromQuery] string? description = null, [FromQuery] int? idProduct = null)
        {
            try
            {
                return Ok(new ResponseResult<List<ProductModel>>(_productService.getAllProducts(description, idProduct)));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
    }
}
