using Application.Abstractions.Idempotency;
using Application.Data;
using Application.Orders;
using Application.Orders.Create;
using Application.Orders.GetOrderSummary;
using Domain.Customers;
using Domain.Orders;
using Domain.Products;
using EntityFramework.Exceptions.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Idempotency;
using Persistence.Repositories;
using Persistence.Services;

namespace Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options
                .UseSqlServer(configuration.GetConnectionString("Database"))
                .UseExceptionProcessor());

        services.AddScoped<IApplicationDbContext>(sp =>
            sp.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IUnitOfWork>(sp =>
            sp.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<ICustomerRepository, CustomerRepository>();

        services.AddScoped<IOrderRepository, OrderRepository>();

        services.AddScoped<IOrderSummaryRepository, OrderSummaryRepository>();

        services.AddScoped<IProductRepository, ProductRepository>();

        services.AddScoped<IOrderReadService, OrderReadService>();

        services.AddScoped<IGetOrderSummary, GetOrderSummary>();

        services.AddScoped<ICalculateOrderSummary, CalculateOrderSummary>();

        services.AddScoped<IIdempotencyService, IdempotencyService>();

        return services;
    }
}
