using Domain.Customers;
using Domain.Orders;
using Microsoft.EntityFrameworkCore;

namespace Application.Data;

public interface IApplicationDbContext
{
    DbSet<Customer> Customers { get; set; }

    DbSet<Order> Orders { get; set; }

    Task<int> SaveChancesAsync(CancellationToken cancellationToken = default);
}
