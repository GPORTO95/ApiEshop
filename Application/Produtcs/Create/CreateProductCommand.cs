using Application.Abstractions.Idempotency;
using MediatR;

namespace Application.Produtcs.Create;

public record CreateProductCommand(
    Guid RequestId,
    string Name,
    string Sku,
    string Currency,
    decimal Amount) : IdempotencyCommand(RequestId);

public record CreateProductRequest(
    string Name,
    string Sku,
    string Currency,
    decimal Amount);
