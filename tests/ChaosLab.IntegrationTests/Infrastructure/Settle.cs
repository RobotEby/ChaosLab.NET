namespace ChaosLab.IntegrationTests.Infrastructure;

// A short, fixed wait for assertions about something that should NOT happen,
// where there is nothing to poll for.
public static class Settle
{
    public static Task Briefly() => Task.Delay(TimeSpan.FromSeconds(1));
}
