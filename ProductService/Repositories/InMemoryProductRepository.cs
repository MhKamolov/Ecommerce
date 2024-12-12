using ProductService.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace ProductService.Repositories
{
    public class InMemoryProductRepository : IProductRepository
    {
        private readonly ConcurrentDictionary<int, Product> _products = new();
        private readonly SemaphoreSlim _semaphore = new(1, 1);
        private int _currentId = 0;

        public Task<IEnumerable<Product>> GetAllProducts(CancellationToken cancellationToken)
        {
            return Task.FromResult(_products.Values.AsEnumerable());
        }

        public Task<Product> GetProductById(int id, CancellationToken cancellationToken)
        {
            _products.TryGetValue(id, out var product);
            return Task.FromResult(product);
        }

        public async Task<bool> NewProduct(Product product, CancellationToken cancellationToken)
        {
            await _semaphore.WaitAsync(cancellationToken);
            try
            {
                if (_products.Values.Any(x => x.Name == product.Name)) { return false; }

                int newId = ++_currentId;
                product.Id = newId;
                if (!_products.ContainsKey(newId))
                {
                    product.Id = _currentId;
                    _products[_currentId] = product;
                    return true;
                }
                return false;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<bool> UpdateProduct(Product product, CancellationToken cancellationToken)
        {
            await _semaphore.WaitAsync(cancellationToken);
            try
            {
                if (_products.Values.Any(x => x.Name == product.Name)) { return false; }

                if (_products.ContainsKey(product.Id))
                {
                    _products[product.Id] = product;
                    return true;
                }
                return false;
            }
            finally
            {
                _semaphore.Release();
            }

        }

        public async Task<bool> DeleteProduct(int id, CancellationToken cancellationToken)
        {
            await _semaphore.WaitAsync(cancellationToken);
            try
            {
                return _products.TryRemove(id, out _);
            }
            finally
            {
                _semaphore.Release();
            }
        }


    }
}
