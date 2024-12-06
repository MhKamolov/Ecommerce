using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GatewayService;
using System.Collections.Generic;
using Ecommerce;

namespace GatewayService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService_proto.ProductService_protoClient _productServiceClient_proto;
        public ProductsController(ProductService_proto.ProductService_protoClient productServiceClient_proto)
        {
            _productServiceClient_proto = productServiceClient_proto;
        }

        // GET /products
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var response = await _productServiceClient_proto.GetProductsAsync(new Google.Protobuf.WellKnownTypes.Empty());
            return Ok(response.Products);
        }

        // GET /products/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var response = await _productServiceClient_proto.GetProductAsync(new ProductRequest { Id = id });

            switch (response.ResultCase)
            {
                case ProductResponse.ResultOneofCase.Product:
                    return Ok(response.Product);

                case ProductResponse.ResultOneofCase.Error:
                    return NotFound(new { Message = response.Error });

                default:
                    return StatusCode(500, "Unexpected response from the service.");
            }
        }

        // POST /products
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            var response = await _productServiceClient_proto.CreateProductAsync(product);

            switch (response.ResultCase)
            {
                case ProductResponse.ResultOneofCase.Product:
                    return CreatedAtAction(nameof(GetProduct), new { id = response.Product.Id }, response);

                case ProductResponse.ResultOneofCase.Error:
                    return BadRequest(new { Message = response.Error });

                default:
                    return StatusCode(500, "Unexpected response from the service.");
            }
        }

        // PUT /products/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            if (id != product.Id)
                return BadRequest();

            var response = await _productServiceClient_proto.UpdateProductAsync(product);

            switch (response.ResultCase)
            {
                case ProductResponse.ResultOneofCase.Product:
                    return Ok(response.Product);

                case ProductResponse.ResultOneofCase.Error:
                    return BadRequest(new { Message = response.Error });

                default:
                    return StatusCode(500, "Unexpected response from the service.");
            }

        }

        // DELETE /products/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productServiceClient_proto.DeleteProductAsync(new ProductRequest { Id = id });
            return NoContent();
        }


    }
}
