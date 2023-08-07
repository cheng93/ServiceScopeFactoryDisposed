namespace ServiceScopeFactoryDisposed;

using System.Threading.Channels;

public class QueueHostedService : BackgroundService
{
    private readonly ChannelReader<WorkItem> queue;

    public QueueHostedService(ChannelReader<WorkItem> queue)
    {
        this.queue = queue;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (await this.queue.WaitToReadAsync(cancellationToken))
        {
            if (this.queue.TryRead(out var workItem))
            {
                await workItem(cancellationToken);
            }
        }
    }
}

public delegate Task WorkItem(CancellationToken cancellationToken);