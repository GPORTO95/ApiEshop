using Application.Abstractions.Data;
using Dapper;
using Domain.Orders;
using MediatR;

namespace Application.Orders.GetOrder;

internal sealed class GetOrderQueryHandler : 
    IRequestHandler<GetOrderQuery, OrderResponse>
{
    private readonly IOrderReadService _orderReadService;
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetOrderQueryHandler(IOrderReadService orderReadService, ISqlConnectionFactory sqlConnectionFactory)
    {
        _orderReadService = orderReadService;
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<OrderResponse> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var connection = _sqlConnectionFactory.Create();

        Dictionary<Guid, OrderResponse> ordersDictionary = new();

        await connection
            .QueryAsync<OrderResponse, LineItemResponse, OrderResponse>(
            """
            SELECT
                o.id AS Id,
                o.customer_id AS CustomerId,
                li.id AS LineItemId,
                li.price_amount as Price
            FROM orders o
            JOIN line_items li ON o.id = li.order_id
            WHERE o.id = @OrderId
            """,
            (order, lineItem) =>
            {
                if (ordersDictionary.TryGetValue(order.Id, out var existingOrder))
                {
                    order = existingOrder;
                }
                else
                {
                    ordersDictionary.Add(order.Id, order);
                }

                order.LineItems.Add(lineItem);

                return order;
            },
            new
            {
                request.OrderId
            },
            splitOn: "LineItemId");

        var orderResponse = ordersDictionary[request.OrderId];

        if (orderResponse is null)
        {
            throw new OrderNotFoundException(new OrderId(request.OrderId));
        }

        return orderResponse;
    }

    public async Task<OrderResponse> HandleEfCore(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var orderId = new OrderId(request.OrderId);

        var orderResponse = await _orderReadService.GetByIdAsync(orderId);

        if (orderResponse is null)
        {
            throw new OrderNotFoundException(orderId);
        }

        return orderResponse;
    }
}
