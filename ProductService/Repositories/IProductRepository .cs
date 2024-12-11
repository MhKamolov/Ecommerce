using ProductService.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProductService.Repositories 
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProducts(CancellationToken cancellationToke);
        Task<Product> GetProductById(int id, CancellationToken cancellationToke);
        Task<bool> NewProduct(Product product, CancellationToken cancellationToke);
        Task<bool> UpdateProduct(Product product, CancellationToken cancellationToke);
        Task<bool> DeleteProduct(int id, CancellationToken cancellationToke);
    }
}
