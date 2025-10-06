using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using SeuApp.Data;
using SeuApp.Data.Repositories;
using SeuApp.WinForms.Forms;

namespace SeuApp.WinForms;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        using var host = CreateHostBuilder().Build();
        var provider = host.Services;

        // Abre a tela inicial (launcher)
        Application.Run(provider.GetRequiredService<MainForm>());
    }

    static IHostBuilder CreateHostBuilder() =>
        Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((ctx, cfg) =>
            {
                // Lê appsettings.json do projeto WinForms
                cfg.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((ctx, services) =>
            {
                // Connection String
                var cs = ctx.Configuration.GetConnectionString("Default")
                         ?? "server=127.0.0.1;port=3306;database=SeuAppDb;user=root;password=1010110101#;TreatTinyAsBoolean=false";

                // DbContext com Pomelo (MariaDB)
                services.AddDbContext<AppDbContext>(opt =>
                    opt.UseMySql(
                        cs,
                        new MariaDbServerVersion(new Version(10, 11, 0)),
                        my => my.SchemaBehavior(MySqlSchemaBehavior.Ignore)
                    )
                );

                // Repositórios genéricos
                services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

                // Forms
                services.AddTransient<MainForm>();
                services.AddTransient<ProdutosForm>();
                services.AddTransient<VendasForm>();
            });
}
