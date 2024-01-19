using DurableFunction.Infra;
using DurableFunction.Infra.Contratos;
using DurableFunction.Infra.Repositorio;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

[assembly: FunctionsStartup(typeof(DurableFunction.Startup))]

namespace DurableFunction;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var configuration = builder.GetContext().Configuration;

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("SqlServer")));

        builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
    }
}
