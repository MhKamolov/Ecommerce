using Ecommerce;
using FluentValidation.Results;
using Google.Protobuf.WellKnownTypes;
using Google.Type;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Logging;
using ProductService;
using ProductService.Repositories;
using ProductService.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Services
{
    public class ProductService : ProductService_proto.ProductService_protoBase
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public override async Task<ProductList> GetProducts(Empty request, ServerCallContext context)
        {
            var products = await _productRepository.GetAllProducts(context.CancellationToken).ConfigureAwait(false);
            var response = new ProductList();
            response.Products.AddRange(products.Select(p => new Product
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock
            }));
            return response;
        }

        public override async Task<ProductResponse> GetProduct(ProductRequest request, ServerCallContext context)
        {
            var product = await _productRepository.GetProductById(request.Id, context.CancellationToken).ConfigureAwait(false);
            if (product == null)
            {
                return new ProductResponse
                {
                    Error = $"Product with ID {request.Id} not found."
                };
            }

            return new ProductResponse
            {
                Product = new Product
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Stock = product.Stock
                }
            };
        }

        public override async Task<ProductResponse> CreateProduct(Product request, ServerCallContext context)
        {
            var product = new Models.Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Stock = request.Stock
            };

            ProductValidator validator = new ProductValidator();
            ValidationResult result = validator.Validate(product);
            if (result.Errors.Count > 0)
            {
                return new ProductResponse
                {
                    Error = String.Join(" || ", result.Errors)
                };
            }

            var success = await _productRepository.NewProduct(product, context.CancellationToken).ConfigureAwait(false);

            if (!success)
            {
                return new ProductResponse
                {
                    Error = $"Product creation failed."
                };
            }

            return new ProductResponse
            {
                Product = new Product
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Stock = product.Stock
                }
            };
        }

        public override async Task<ProductResponse> UpdateProduct(Product request, ServerCallContext context)
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
                return new ProductResponse
                {
                    Error = String.Join(" || ", result.Errors)
                };
            }

            var success = await _productRepository.UpdateProduct(product, context.CancellationToken).ConfigureAwait(false);

            if (!success)
            {
                return new ProductResponse
                {
                    Error = $"Product with ID {request.Id} not found."
                };
            }

            return new ProductResponse
            {
                Product = new Product
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Stock = product.Stock
                }
            };
        }
        
        public override async Task<Empty> DeleteProduct(ProductRequest request, ServerCallContext context)
        {
            var success = await _productRepository.DeleteProduct(request.Id, context.CancellationToken).ConfigureAwait(false);

            if (!success)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Product with ID {request.Id} not found."));
            }

            return new Empty();
        }


    }



}
