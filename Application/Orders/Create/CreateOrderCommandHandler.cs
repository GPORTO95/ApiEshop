﻿using Application.Data;
using Domain.Customers;
using Domain.Orders;
using MediatR;
using Rebus.Bus;

namespace Application.Orders.Create;

internal sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateOrderCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var customer = await _context.Customers.FindAsync(
            new CustomerId(request.CustomerId));

        if (customer is null)
            return;

        var order = Order.Create(customer.Id);

        _context.Orders.Add(order);

        _context.OrderSummaries.Add(new OrderSummary(order.Id.Value, customer.Id.Value, 0));

        await _context.SaveChangesAsync(cancellationToken);
    }
}
