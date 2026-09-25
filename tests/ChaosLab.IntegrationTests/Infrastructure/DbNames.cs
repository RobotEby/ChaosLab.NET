namespace ChaosLab.IntegrationTests.Infrastructure;

public static class DbNames
{
    public static string New(string prefix) => $"{prefix}Tests_{Guid.NewGuid():N}";
}
