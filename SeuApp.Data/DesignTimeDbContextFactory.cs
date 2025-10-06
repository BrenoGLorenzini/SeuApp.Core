using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace SeuApp.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            // 1) Primeiro tenta variável de ambiente (recomendado)
            var cs = Environment.GetEnvironmentVariable("SEUAPP_CS");

            // 2) Fallback: use a MESMA string dos appsettings (root)
            cs ??= "server=127.0.0.1;port=3306;database=SeuAppDb;user=root;password=suasenha;TreatTinyAsBoolean=false";

            var serverVersion = ServerVersion.AutoDetect(cs);
            var opt = new DbContextOptionsBuilder<AppDbContext>()
                .UseMySql(cs, serverVersion)
                .Options;

            return new AppDbContext(opt);
        }
    }
}
