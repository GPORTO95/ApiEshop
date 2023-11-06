using Domain.Products;
using MediatR;

namespace Application.Produtcs.Create;

internal sealed class CreateProductCommandHandler
    : IRequestHandler<CreateProductCommand>
{
    private readonly IProductRepository _productRepository;

    public CreateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public Task Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product(
            new ProductId(Guid.NewGuid()),
            request.Name,
            new Money(request.Currency, request.Amount),
            Sku.Create(request.Sku)!);

        _productRepository.Add(product);

        return Task.CompletedTask;
    }
}
