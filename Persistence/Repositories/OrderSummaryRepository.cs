using Domain.Orders;

namespace Persistence.Repositories;

internal sealed class OrderSummaryRepository : IOrderSummaryRepository
{
    private readonly ApplicationDbContext _context;

    public OrderSummaryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Add(OrderSummary orderSummary)
    {
        _context.OrderSummaries.Add(orderSummary);
    }
}
