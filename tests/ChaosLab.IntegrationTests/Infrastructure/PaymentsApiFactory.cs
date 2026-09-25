extern alias PaymentsApi;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using PaymentsProgram = PaymentsApi::Program;
using PaymentsApi::Payments.Api.Gateways;

namespace ChaosLab.IntegrationTests.Infrastructure;

public sealed class PaymentsApiFactory(
    string connectionString, string rabbitHost, ushort rabbitPort, string rabbitUser, string rabbitPassword,
    IPaymentGateway gateway)
    : WebApplicationFactory<PaymentsProgram>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:Db", connectionString);
        builder.UseSetting("RabbitMq:Host", rabbitHost);
        builder.UseSetting("RabbitMq:Port", rabbitPort.ToString());
        builder.UseSetting("RabbitMq:User", rabbitUser);
        builder.UseSetting("RabbitMq:Password", rabbitPassword);

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IPaymentGateway>();
            services.AddSingleton(gateway);
        });
    }
}
