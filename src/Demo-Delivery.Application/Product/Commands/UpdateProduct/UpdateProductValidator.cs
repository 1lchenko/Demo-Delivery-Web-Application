using Demo_Delivery.Domain;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Demo_Delivery.Application.Product.Commands.UpdateProduct;

using static DomainConstants.ProductValidationConstants;

public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductValidator()
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
            .GreaterThan(d => d.AmountOnStock)
            .LessThanOrEqualTo(StockThresholdUpperLimit);

        RuleFor(d => d.NewImageFiles).Must(x => x is null || x.Count <= 3);

        RuleForEach(d => d.NewImageFiles).SetValidator(new FileValidator());
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