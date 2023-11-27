using Application;
using Application.Abstractions;
using Application.Orders.Create;
using Carter;
using Persistence;
using Infrastructure;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using System.Threading.RateLimiting;
using Web.API.Extensions;
using Web.API.Services;
using Web.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddCarter();

builder.Services.AddScoped<ILinkService, LinkService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddRateLimiter(rateLimiterOptions =>
{
    rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    rateLimiterOptions.AddPolicy("fixed", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.User.Identity?.Name?.ToString(),
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 10,
                Window = TimeSpan.FromSeconds(10)
            }));

    //rateLimiterOptions.AddFixedWindowLimiter("fixed", options =>
    //{
    //    options.Window = TimeSpan.FromSeconds(10);
    //    options.PermitLimit = 3;
    //});

    //rateLimiterOptions.AddSlidingWindowLimiter("sliding", options =>
    //{
    //    options.Window = TimeSpan.FromSeconds(15);
    //    options.SegmentsPerWindow = 3;
    //    options.PermitLimit = 15;
    //});

    //rateLimiterOptions.AddTokenBucketLimiter("token", options =>
    //{
    //    options.TokenLimit = 100;
    //    options.ReplenishmentPeriod = TimeSpan.FromSeconds(5);
    //    options.TokensPerPeriod = 10;
    //});

    //rateLimiterOptions.AddConcurrencyLimiter("concurrency", options =>
    //{
    //    options.PermitLimit = 5;
    //});
});

builder.Services.AddRebus(rebus => rebus
    .Routing(r =>
        r.TypeBased().MapAssemblyOf<ApplicationAssemblyReference>("eshop-queue"))
    .Transport(t =>
        t.UseRabbitMq(
            builder.Configuration.GetConnectionString("MessageBroker"),
            "eshop-queue"))
    .Sagas(s =>
        s.StoreInSqlServer(
            builder.Configuration.GetConnectionString("Database"),
            "sagas",
            "saga_indexes")),
        onCreated: async bus =>
        {
            await bus.Subscribe<OrderConfirmationEmailSent>();
            await bus.Subscribe<OrderPaymentRequestSent>();
        });

builder.Services.AutoRegisterHandlersFromAssemblyOf<ApplicationAssemblyReference>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapCarter();

app.Run();

public partial class Program { }