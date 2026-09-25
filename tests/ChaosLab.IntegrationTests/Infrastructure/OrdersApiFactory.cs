extern alias OrdersApi;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

using OrdersProgram = OrdersApi::Program;

namespace ChaosLab.IntegrationTests.Infrastructure;

public sealed class OrdersApiFactory(
    string connectionString, string rabbitHost, ushort rabbitPort, string rabbitUser, string rabbitPassword)
    : WebApplicationFactory<OrdersProgram>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:Db", connectionString);
        builder.UseSetting("RabbitMq:Host", rabbitHost);
        builder.UseSetting("RabbitMq:Port", rabbitPort.ToString());
        builder.UseSetting("RabbitMq:User", rabbitUser);
        builder.UseSetting("RabbitMq:Password", rabbitPassword);
    }
}
