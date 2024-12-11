using FluentValidation;
using System;
using Google.Type;
using ProductService.Validators;

namespace ProductService.Validators 
{
    public class ProductValidator : AbstractValidator<Models.Product>
    {
        public ProductValidator() 
        {
            RuleFor(p => p.Name).NotEmpty().Length(1, 100);
            RuleFor(p => p.Price).GreaterThan(new Money { Units = 0, Nanos = 0, CurrencyCode = "USD" });
            RuleFor(p => p.Stock).GreaterThanOrEqualTo(0);
        }
    }
}
