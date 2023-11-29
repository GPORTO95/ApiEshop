using Application.Abstractions;
using Domain.Products;
using MediatR;

namespace Application.Produtcs.GetById;

public record GetProductQuery(ProductId ProductId) : IRequest<ProductResponse>;

public class ProductResponse
{
    public ProductResponse(Guid id, string name, string sku, string currency, decimal amount, List<Link> links)
    {
        Id = id;
        Name = name;
        Sku = sku;
        Currency = currency;
        Amount = amount;
        Links = links;
    }

    public Guid Id { get; init; }

    public string Name { get; init; }

    public string Sku { get; init; }

    public string Currency { get; init; }

    public decimal Amount { get; init; }

    public List<Link> Links { get; init; } = new();
}

//public record ProductResponse(
//    Guid Id,
//    string Name,
//    string Sku,
//    string Currency,
//    decimal Amount)
//{
//    public List<Link> Links { get; set; } = new();
//}
