using Ecommerce;
using FluentValidation.Results;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Logging;
using ProductService_gRPC;
using ProductService_gRPC.Repositories;
using ProductService_gRPC.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService_gRPC.Services
{
    public class ProductService : ProductService_proto.ProductService_protoBase
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public override Task<ProductList> GetProducts(Empty request, ServerCallContext context)
        {
            var products = _productRepository.GetAllProducts().Select(p => new Product
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock
            }).ToList();
            
            return Task.FromResult(new ProductList { Products = { products } });
        }

        public override Task<Product> GetProduct(ProductRequest request, ServerCallContext context)
        {
            var product = _productRepository.GetProductById(request.Id);

            return Task.FromResult(product != null ? new Product
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock
            } : null);
        }

        public override Task<Product> CreateProduct(Product request, ServerCallContext context)
        {
            var product = new Models.Product
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Stock = request.Stock
            };


            ProductValidator validator = new ProductValidator();
            ValidationResult result = validator.Validate(product);
            //if (result.Errors.Count > 0) { throw new Exception(String.Join("\n", result.Errors)); }

            _productRepository.NewProduct(product);
            return Task.FromResult(new Product()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock
            });
        }

        public override Task<Product> UpdateProduct(Product request, ServerCallContext context)
        {
            var product = new Models.Product
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Stock = request.Stock
            };

            ProductValidator validator = new ProductValidator();
            ValidationResult result = validator.Validate(product);
            //if (result.Errors.Count > 0) { throw new Exception(String.Join("\n", result.Errors)); }

            _productRepository.UpdateProduct(product);
            return Task.FromResult(new Product()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock
            });
        }

        public override Task<Empty> DeleteProduct(ProductRequest request, ServerCallContext context)
        {
            _productRepository.DeleteProduct(request.Id);
            return Task.FromResult(new Empty());
        }



    }



}
