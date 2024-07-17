using Demo_Delivery.Application.Common.Exceptions;
using Demo_Delivery.Application.Common.Interfaces;
using Demo_Delivery.Application.CQRS;
using Demo_Delivery.Domain;
using Microsoft.AspNetCore.Http;

namespace Demo_Delivery.Application.Product.Commands.CreateProduct;

using Product = Domain.Entities.ProductAggregate.Product;
using Category = Domain.Entities.CategoryAggregate.Category;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    int CategoryId,
    decimal Calories,
    int AmountOnStock,
    int MaxStockThreshold,
    bool IsActive,
    List<IFormFile>? ImageFiles) : ICommand;

public class CreateProductHandler : ICommandHandler<CreateProductCommand>
{
    private readonly IRepository<Product> _productRepository;
    private readonly IReadRepository<Category> _categoryRepository;
    private readonly IFileService _fileService;
   

    public CreateProductHandler(IRepository<Product> productRepository, IFileService fileService,
        IReadRepository<Category> categoryRepository )
    {
        _productRepository = productRepository;
        _fileService = fileService;
        _categoryRepository = categoryRepository;
         
    }

    public async Task Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var category = await GetCategoryAsync(request.CategoryId, cancellationToken);

        var product = new Product(request.Name, request.Description, request.Price, request.Calories,
            request.AmountOnStock, request.MaxStockThreshold, request.IsActive, category);

        if (request.ImageFiles is not null && request.ImageFiles.Any())
        {
            await AddImageFilesToProductAsync(product, request.ImageFiles, cancellationToken);
        }

        await _productRepository.AddAsync(product, cancellationToken);
    }

    private async Task<Category> GetCategoryAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId, cancellationToken);
        if (category is null)
        {
            throw new NotFoundException(nameof(Category), categoryId);
        }

        return category;
    }

    private async Task AddImageFilesToProductAsync(Product product, List<IFormFile> files,
        CancellationToken cancellationToken = default)
    {
        foreach (var imageFile in files)
        {
            var imageKey = await _fileService.UploadFileAsync(imageFile, GlobalConstants.Bucket.BuketName,
                GlobalConstants.Bucket.ProductImagesPrefix, cancellationToken);
            product.AddImageKey(imageKey);
        }
    }
}