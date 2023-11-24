using Microsoft.Extensions.Logging;
using Quartz;

namespace Infrastructure;


public class LoggindBackgroundJob : IJob
{
    private readonly ILogger<LoggindBackgroundJob> _logger;

    public LoggindBackgroundJob(ILogger<LoggindBackgroundJob> logger)
    {
        _logger = logger;
    }

    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("{UtcNow}", DateTime.UtcNow);

        return Task.CompletedTask;
    }
}
