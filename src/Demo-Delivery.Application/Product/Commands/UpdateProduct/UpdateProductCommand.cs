using Demo_Delivery.Application.Common.Exceptions;
using Demo_Delivery.Application.Common.Interfaces;
using Demo_Delivery.Application.CQRS;
using Demo_Delivery.Application.Dtos.Product;
using Demo_Delivery.Domain;
using Demo_Delivery.Domain.SeedWork;
using Microsoft.AspNetCore.Http;

namespace Demo_Delivery.Application.Product.Commands.UpdateProduct;

using Product = Domain.Entities.ProductAggregate.Product;
using Category = Domain.Entities.CategoryAggregate.Category;

public record UpdateProductCommand(
    int Id,
    string Name,
    string Description,
    decimal Price,
    int CategoryId,
    decimal Calories,
    int AmountOnStock,
    int MaxStockThreshold,
    bool IsActive,
    List<IFormFile>? NewImageFiles,
    List<string>? ImageKeysToDelete) : ICommand;

public class UpdateDishHandler : ICommandHandler<UpdateProductCommand>
{
    private readonly IRepository<Product> _productRepository;
    private readonly IReadRepository<Category> _categoryRepository;
    private readonly IFileService _fileService;

    public UpdateDishHandler(IRepository<Product> productRepository, IFileService fileService,
        IReadRepository<Category> categoryRepository)
    {
        _productRepository = productRepository;
        _fileService = fileService;
        _categoryRepository = categoryRepository;
    }

    public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category is null)
        {
            throw new NotFoundException(nameof(Category), request.CategoryId);
        }

        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        if (product is null)
        {
            throw new NotFoundException(nameof(Product), request.Id);
        }

        product.Update(request.Name, request.Description, request.Price, request.Calories, request.AmountOnStock,
            request.MaxStockThreshold, request.IsActive, category);

        if (request.ImageKeysToDelete is { Count: > 0 })
        {
            await _fileService.DeleteFilesAsync(request.ImageKeysToDelete, GlobalConstants.Bucket.BuketName,
                cancellationToken);

            foreach (var key in request.ImageKeysToDelete)
            {
                product.RemoveImageKey(key);
            }
        }

        if (request.NewImageFiles is { Count: > 0 })
        {
            foreach (var newImageFile in request.NewImageFiles)
            {
                var imageKey = await _fileService.UploadFileAsync(newImageFile, GlobalConstants.Bucket.BuketName,
                    GlobalConstants.Bucket.ProductImagesPrefix, cancellationToken);
                product.AddImageKey(imageKey);
            }
        }

        await _productRepository.SaveChangesAsync(cancellationToken);
    }
}