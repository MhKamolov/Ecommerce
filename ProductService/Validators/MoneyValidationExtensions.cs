using FluentValidation;
using System;
using Google.Type;

namespace ProductService.Validators
{
    public static class MoneyValidationExtensions
    {
        public static IRuleBuilderOptions<T, Money> GreaterThan<T>(
            this IRuleBuilder<T, Money> ruleBuilder, Money boundary)
        {
            return ruleBuilder.Must(actual =>
                actual != null &&
                string.Equals(actual.CurrencyCode, boundary.CurrencyCode, StringComparison.OrdinalIgnoreCase) &&
                (actual.Units > boundary.Units ||
                (actual.Units == boundary.Units && actual.Nanos > boundary.Nanos))
                )
                .WithMessage($"Значение должно быть больше чем {boundary.Units}.{boundary.Nanos} {boundary.CurrencyCode}");


        }
    }
}
