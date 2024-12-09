using ProductService_gRPC.Models;
using System.Collections.Generic;

namespace ProductService_gRPC.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAllProducts();
        Product GetProductById(int id);
        void NewProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int id);
    }
}
