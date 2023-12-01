using Domain.Products;
using MediatR;

namespace Application.Produtcs.Delete;

internal sealed class DeleteProductCommandHandler
    : IRequestHandler<DeleteProductCommand>
{
    private readonly IProductRepository _productRepository;

    public DeleteProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id);

        if (product is null)
        {
            throw new ProductNotFoundException(request.Id);
        }

        _productRepository.Remove(product);
    }
}
