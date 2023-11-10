using Application.Behaviors;
using FluentValidation;
using MediatR.NotificationPublishers;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DepedencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining<ApplicationAssemblyReference>();

            config.NotificationPublisher = new TaskWhenAllPublisher();

            config.AddOpenBehavior(typeof(UnitOfWorkBehavior<,>));
        });

        services.AddValidatorsFromAssembly(ApplicationAssemblyReference.Assembly);

        return services;
    }
}
