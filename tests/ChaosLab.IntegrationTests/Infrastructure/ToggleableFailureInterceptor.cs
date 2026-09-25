using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ChaosLab.IntegrationTests.Infrastructure;

// Lets a test force the next SaveChanges to fail, to prove the payment and
// its outbox event are committed atomically (PAY-05).
public sealed class ToggleableFailureInterceptor : SaveChangesInterceptor
{
    public bool ShouldFail { get; set; }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (ShouldFail)
            throw new InvalidOperationException("Simulated commit failure");

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
