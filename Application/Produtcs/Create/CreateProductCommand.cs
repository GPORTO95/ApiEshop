using MediatR;

namespace Application.Produtcs.Create;

public record CreateProductCommand(
    string Name,
    string Sku,
    string Currency,
    decimal Amount) : IRequest;
