using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddQuartz(options =>
        {
            options.UseMicrosoftDependencyInjectionJobFactory();
        });

        services.AddQuartzHostedService(optiopns =>
        {
            optiopns.WaitForJobsToComplete = true;
        });

        services.ConfigureOptions<LoggingBackgroundJobSetup>();
    }

}
