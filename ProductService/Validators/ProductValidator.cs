using FluentValidation;
using ProductService_gRPC.Models;

namespace ProductService_gRPC.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator() 
        {
            //RuleFor(p => p.Name).NotEmpty().Length(1, 100);
            //RuleFor(p => p.Price).GreaterThan(0);
            //RuleFor(p => p.Stock).GreaterThanOrEqualTo(0);
        }
    }
}
