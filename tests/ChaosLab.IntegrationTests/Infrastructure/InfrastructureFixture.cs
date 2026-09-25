using Testcontainers.MsSql;
using Testcontainers.RabbitMq;
using Xunit;

namespace ChaosLab.IntegrationTests.Infrastructure;

// Starts the one SQL Server and one RabbitMQ container shared by every test
// in the "Infrastructure" collection, mirroring the single-instance-many-
// databases setup described in docs/05-docker-environment.
public sealed class InfrastructureFixture : IAsyncLifetime
{
    public MsSqlContainer Sql { get; } = new MsSqlBuilder()
        .WithPassword("Str0ng!Passw0rd")
        .Build();

    public RabbitMqContainer Rabbit { get; } = new RabbitMqBuilder()
        .WithUsername("chaos")
        .WithPassword("chaos")
        .Build();

    public Task InitializeAsync() =>
        Task.WhenAll(Sql.StartAsync(), Rabbit.StartAsync());

    public Task DisposeAsync() =>
        Task.WhenAll(Sql.DisposeAsync().AsTask(), Rabbit.DisposeAsync().AsTask());
}

[CollectionDefinition(Name)]
public sealed class InfrastructureCollection : ICollectionFixture<InfrastructureFixture>
{
    public const string Name = "Infrastructure";
}
