namespace RadarWeb.Net.ApuracaoAssistida.Worker;

public sealed class Worker(ILogger<Worker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("RadarWeb.Net.ApuracaoAssistida.Worker iniciado em {Time}", DateTimeOffset.Now);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}
