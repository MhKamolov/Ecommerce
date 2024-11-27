using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GatewayService;
using System.Collections.Generic;
//using ProductService.Protos;


namespace GatewayService.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    //public class ProductsController : ControllerBase
    //{
    //    private readonly ProductService_proto.ProductService_protoClient _productServiceClient_proto;
    //    public ProductsController(GrpcChannel grpcChannel)
    //    {
    //        _productServiceClient_proto = new ProductService_proto.ProductService_protoClient(grpcChannel);
    //    }

    //    // GET /products
    //    [HttpGet]
    //    public async Task<IActionResult> GetProducts()
    //    {
    //        var response = await _productServiceClient_proto.GetProductsAsync(new Empty());
    //        return Ok(response.Products);
    //    }

    //    // GET /products/{id}
    //    [HttpGet("{id}")]
    //    public async Task<IActionResult> GetProduct(int id)
    //    {
    //        var response = await _productServiceClient_proto.GetProductAsync(new ProductRequest { Id = id });
    //        if (response == null)
    //            return NotFound();

    //        return Ok(response);
    //    }

    //    // POST /products
    //    [HttpPost]
    //    public async Task<IActionResult> CreateProduct([FromBody] Product product)
    //    {
    //        var response = await _productServiceClient_proto.CreateProductAsync(product);
    //        return CreatedAtAction(nameof(GetProduct), new { id = response.Id }, response);
    //    }

    //    // PUT /products/{id}
    //    [HttpPut("{id}")]
    //    public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
    //    {
    //        if (id != product.Id)
    //            return BadRequest();

    //        var response = await _productServiceClient_proto.UpdateProductAsync(product);
    //        return Ok(response);
    //    }

    //    // DELETE /products/{id}
    //    [HttpDelete("{id}")]
    //    public async Task<IActionResult> DeleteProduct(int id)
    //    {
    //        await _productServiceClient_proto.DeleteProductAsync(new ProductRequest { Id = id });
    //        return NoContent();
    //    }


    //}
}
