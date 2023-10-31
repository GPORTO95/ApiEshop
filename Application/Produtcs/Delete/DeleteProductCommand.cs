using Domain.Products;
using MediatR;

namespace Application.Produtcs.Delete;

public record DeleteProductCommand(ProductId Id) : IRequest;
