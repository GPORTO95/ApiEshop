using Application.Data;
using Domain.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Orders.GetOrder;

internal sealed class GetOrderQueryHandler : 
    IRequestHandler<GetOrderQuery, OrderResponse>
{
    private readonly IApplicationDbContext _context;

    public async Task<OrderResponse> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var orderResponse = await _context
            .Orders
            .Where(o => o.Id == new OrderId(request.OrderId))
            .Select(order => new OrderResponse(
                order.Id.Value,
                order.CustomerId.Value,
                order.LineItems
                    .Select(li => new LineItemResponse(li.Id.Value, li.Price.Amount))
                    .ToList()))
            .SingleOrDefaultAsync(cancellationToken);

        return orderResponse;
    }
}
