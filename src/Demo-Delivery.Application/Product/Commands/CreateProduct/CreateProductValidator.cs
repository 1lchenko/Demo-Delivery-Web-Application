using Demo_Delivery.Domain;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Demo_Delivery.Application.Product.Commands.CreateProduct;

using static DomainConstants.ProductValidationConstants;

public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(d => d.Name).NotEmpty().MinimumLength(MinNameLength).MaximumLength(MaxNameLength);

        RuleFor(d => d.Description).NotEmpty().MinimumLength(MinDescriptionLength).MaximumLength(MaxDescriptionLength);

        RuleFor(d => d.Price).GreaterThan(MinPriceValue).LessThanOrEqualTo(MaxPriceValue);

        RuleFor(d => d.Calories).GreaterThan(MinCaloriesValue).LessThanOrEqualTo(MaxCaloriesValue);

        RuleFor(d => d.AmountOnStock)
            .GreaterThanOrEqualTo(MinAmountOnStockValue)
            .LessThanOrEqualTo(MaxAmountOnStockValue);

        RuleFor(d => d.MaxStockThreshold) 
            .GreaterThan(StockThresholdLowerLimit)
            .GreaterThanOrEqualTo(d => d.AmountOnStock)
            .LessThanOrEqualTo(StockThresholdUpperLimit);

        RuleForEach(d => d.ImageFiles).SetValidator(new FileValidator());
    }

    private class FileValidator : AbstractValidator<IFormFile>
    {
        public FileValidator()
        {
            RuleFor(x => x.Length)
                .NotNull()
                .LessThanOrEqualTo(MaxFileLength)
                .WithMessage("Image must be less or equals 100MB");

            RuleFor(x => x.ContentType)
                .NotNull()
                .Must(x => x.Equals("image/jpeg") || x.Equals("image/png") || x.Equals("image/jpg"))
                .WithMessage("Incorrect image's format. Allowed formats: .png, .jpg, .jpeg");
        }
    }
}