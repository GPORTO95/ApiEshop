using Application.Produtcs.GetById;
using MediatR;

namespace Application.Produtcs.Get;

public record GetProductsQuery(
    string? SearchTerm,
    string? SortColumn,
    string? SortOrder,
    int Page,
    int PageSize): IRequest<PagedList<ProductResponse>>;
