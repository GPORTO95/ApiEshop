using Domain.Products;
using MediatR;

namespace Application.Produtcs.Get;

public record GetProductQuery(ProductId Id): IRequest<ProductResponse>;

public record ProductResponse(
    Guid Id,
    string Name,
    string Sku,
    string Currency,
    decimal Amount);
