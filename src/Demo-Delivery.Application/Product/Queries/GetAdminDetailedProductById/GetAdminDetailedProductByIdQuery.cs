using Demo_Delivery.Application.Common.Exceptions;
using Demo_Delivery.Application.Common.Interfaces;
using Demo_Delivery.Application.Dtos.Product;
using Demo_Delivery.Application.Specifications.Product;
using MediatR;

namespace Demo_Delivery.Application.Product.Queries.GetAdminDetailedProductById;

public record GetAdminDetailedProductByIdQuery(int Id) : IRequest<AdminProductDetailsDto>;

public class
    GetAdminDetailedProductByIdQueryHandler : IRequestHandler<GetAdminDetailedProductByIdQuery, AdminProductDetailsDto>
{
    private readonly IReadRepository<Domain.Entities.ProductAggregate.Product> _productReadRepository;

    public GetAdminDetailedProductByIdQueryHandler(
        IReadRepository<Domain.Entities.ProductAggregate.Product> productReadRepository)
    {
        _productReadRepository = productReadRepository;
    }

    public async Task<AdminProductDetailsDto> Handle(GetAdminDetailedProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        var spec = new AdminProductDetailsByIdSpecification(request.Id);
        var product = await _productReadRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (product is null)
        {
            throw new NotFoundException(nameof(Domain.Entities.ProductAggregate.Product), request.Id);
        }

        return product;
    }
}