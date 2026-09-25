namespace ChaosLab.IntegrationTests.Infrastructure;

// Polls an async condition instead of sleeping a fixed amount, so tests are
// fast on the happy path and tolerant of real infrastructure timing.
public static class Eventually
{
    public static async Task Until(Func<Task<bool>> condition, TimeSpan? timeout = null, TimeSpan? interval = null)
    {
        var deadline = DateTime.UtcNow + (timeout ?? TimeSpan.FromSeconds(10));
        var delay = interval ?? TimeSpan.FromMilliseconds(100);

        while (DateTime.UtcNow < deadline)
        {
            if (await condition()) return;
            await Task.Delay(delay);
        }

        throw new TimeoutException("Condition was not met within the timeout.");
    }
}
