namespace ServiceScopeFactoryDisposed;

using System.Threading.Channels;

public class TestService
{
    private readonly IServiceScopeFactory serviceScopeFactory;
    private readonly ChannelWriter<WorkItem> channelWriter;

    public TestService(
        IServiceScopeFactory serviceScopeFactory,
        ChannelWriter<WorkItem> channelWriter)
    {
        this.serviceScopeFactory = serviceScopeFactory;
        this.channelWriter = channelWriter;
    }

    public async Task Handle()
    {
        async Task DelayedExecute(CancellationToken ct)
        {
            await Task.Delay(1000, ct);
            using var scope = serviceScopeFactory.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Hello World!");
        }

        await channelWriter.WriteAsync(DelayedExecute);
    }
}