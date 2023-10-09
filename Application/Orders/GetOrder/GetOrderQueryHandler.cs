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
        #region :: With ef core simple ::
        //var orderResponse = await _context
        //    .Orders
        //    .Where(o => o.Id == new OrderId(request.OrderId))
        //    .Select(order => new OrderResponse(
        //        order.Id.Value,
        //        order.CustomerId.Value,
        //        order.LineItems
        //            .Select(li => new LineItemResponse(li.Id.Value, li.Price.Amount))
        //            .ToList()))
        //    .SingleOrDefaultAsync(cancellationToken);
        #endregion


        var orderSummaries = await _context
            .Database
            .SqlQuery<OrderSummary>($@"
                SELECT o.Id AS OrderId, o.CustomerId, li.Id AS LineItemId, li.Price_Amount AS LineItemPrice
                FROM Orders AS o
                JOIN LineItems AS li ON li.OrderId = o.Id
                WHERE o.Id = {request.OrderId}")
            .ToListAsync(cancellationToken);

        var orderResponse = orderSummaries
            .GroupBy(o => o.OrderId)
            .Select(grp => new OrderResponse(
                grp.Key,
                grp.First().CustomerId,
                grp.Select(o => new LineItemResponse(o.LineItemId, o.LineItemPrice)).ToList()))
            .Single();

        return orderResponse;
    }

    private sealed record OrderSummary(
        Guid OrderId,
        Guid CustomerId,
        Guid LineItemId,
        decimal LineItemPrice);
}
