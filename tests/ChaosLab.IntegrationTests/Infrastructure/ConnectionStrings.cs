namespace ChaosLab.IntegrationTests.Infrastructure;

public static class ConnectionStrings
{
    // Swaps the database of a SQL Server connection string, the way each
    // test gets its own database on the shared container.
    public static string ForDatabase(string baseConnectionString, string databaseName)
    {
        var keep = baseConnectionString
            .Split(';', StringSplitOptions.RemoveEmptyEntries)
            .Where(part =>
                !part.TrimStart().StartsWith("Database=", StringComparison.OrdinalIgnoreCase) &&
                !part.TrimStart().StartsWith("Initial Catalog=", StringComparison.OrdinalIgnoreCase) &&
                !part.TrimStart().StartsWith("TrustServerCertificate=", StringComparison.OrdinalIgnoreCase));

        return string.Join(';', keep) + $";Database={databaseName};TrustServerCertificate=True";
    }
}
