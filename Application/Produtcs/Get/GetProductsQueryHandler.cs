using Application.Data;
using Application.Produtcs.GetById;
using Domain.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace Application.Produtcs.Get;

internal sealed class GetProductsQueryHandler
    : IRequestHandler<GetProductsQuery, PagedList<ProductResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetProductsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedList<ProductResponse>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Product> productsQuery = _context.Products;

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            productsQuery = productsQuery.Where(p =>
                p.Name.Contains(request.SearchTerm) ||
                ((string)p.Sku).Contains(request.SearchTerm));
        }

        if(request.SortOrder?.ToLower() == "desc")
        {
            productsQuery = productsQuery.OrderByDescending(GetSortProperty(request));
        }
        else
        {
            productsQuery = productsQuery.OrderBy(GetSortProperty(request));
        }

        var productsResponseQuery = productsQuery
            .Select(p => new ProductResponse(
                p.Id.Value,
                p.Name,
                p.Sku.Value,
                p.Price.Currency,
                p.Price.Amount));

        var produtcs = await PagedList<ProductResponse>.CreateAsync(productsResponseQuery, request.Page, request.PageSize);

        return produtcs;
    }

    private static Expression<Func<Product, object>> GetSortProperty(GetProductsQuery request) =>
        request.SortColumn?.ToLower() switch
        {
            "name" => product => product.Name,
            "sku" => product => product.Sku,
            "amount" => product => product.Price.Amount,
            "currency" => product => product.Price.Currency,
            _ => product => product.Id
        };
}
