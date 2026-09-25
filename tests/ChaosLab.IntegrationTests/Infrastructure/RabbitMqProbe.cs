using MassTransit;

namespace ChaosLab.IntegrationTests.Infrastructure;

// A minimal, standalone consumer used to observe a message placed on the
// broker by the service under test, without depending on that service's own
// consumers being present.
public sealed class RabbitMqProbe<TMessage> : IAsyncDisposable where TMessage : class
{
    private readonly IBusControl _bus;
    private readonly TaskCompletionSource<TMessage> _received =
        new(TaskCreationOptions.RunContinuationsAsynchronously);

    private RabbitMqProbe(IBusControl bus) => _bus = bus;

    public static async Task<RabbitMqProbe<TMessage>> StartAsync(
        string host, ushort port, string user, string password)
    {
        RabbitMqProbe<TMessage> probe = null!;

        var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
        {
            cfg.Host(host, port, "/", h =>
            {
                h.Username(user);
                h.Password(password);
            });

            cfg.ReceiveEndpoint(Guid.NewGuid().ToString("N"), e =>
                e.Handler<TMessage>(ctx =>
                {
                    probe._received.TrySetResult(ctx.Message);
                    return Task.CompletedTask;
                }));
        });

        probe = new RabbitMqProbe<TMessage>(bus);
        await bus.StartAsync();
        return probe;
    }

    public async Task<TMessage> WaitAsync(TimeSpan timeout)
    {
        var winner = await Task.WhenAny(_received.Task, Task.Delay(timeout));
        if (winner != _received.Task)
            throw new TimeoutException($"No {typeof(TMessage).Name} was observed within {timeout}.");

        return await _received.Task;
    }

    public async ValueTask DisposeAsync() => await _bus.StopAsync();
}
