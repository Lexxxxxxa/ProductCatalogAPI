using FluentValidation;
using ProductCatalogAPI.Models;

namespace ProductCatalogAPI.Validators;

public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(p => p.Code)
            .NotEmpty().WithMessage("Code is required")
            .Matches(@"^\d{4}-\d{4}$").WithMessage("Code must be in format XXXX-XXXX");

        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Name is required")
            .Length(3, 100).WithMessage("Name must be more than 3 and less than 100 characters long");

        RuleFor(p => p.Description)
            .MaximumLength(500).WithMessage("Description cannot be more than 500 characters");

        RuleFor(p => p.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.0");
    }
}
