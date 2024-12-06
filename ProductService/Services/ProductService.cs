using Ecommerce;
using FluentValidation.Results;
using Google.Protobuf.WellKnownTypes;
using Google.Type;
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

        public override Task<ProductResponse> GetProduct(ProductRequest request, ServerCallContext context)
        {
            var product = _productRepository.GetProductById(request.Id);

            if (product != null)
            {
                return Task.FromResult(new ProductResponse
                {
                    Product = new Product
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        Stock = product.Stock
                    }
                });
            }
            else
            {
                return Task.FromResult(new ProductResponse
                {
                    Error = $"Product with ID {request.Id} not found."
                });
            }
        }

        public override Task<ProductResponse> CreateProduct(Product request, ServerCallContext context)
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
            if (result.Errors.Count > 0) 
            {
                return Task.FromResult(new ProductResponse
                {
                     Error = String.Join(" || ", result.Errors)
                });
            }

            _productRepository.NewProduct(product);
            return Task.FromResult(new ProductResponse
            {
                Product = new Product
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Stock = product.Stock
                }
            });

        }

        public override Task<ProductResponse> UpdateProduct(Product request, ServerCallContext context)
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
            if (result.Errors.Count > 0)
            {
                return Task.FromResult(new ProductResponse
                {
                    Error = String.Join(" || ", result.Errors)
                });
            }

            _productRepository.UpdateProduct(product);

            return Task.FromResult(new ProductResponse
            {
                Product = new Product
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Stock = product.Stock
                }
            });
        }

        public override Task<Empty> DeleteProduct(ProductRequest request, ServerCallContext context)
        {
            _productRepository.DeleteProduct(request.Id);
            return Task.FromResult(new Empty());
        }



    }



}
