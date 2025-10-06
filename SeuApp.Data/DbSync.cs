using System.Threading;

namespace SeuApp.Data
{

    // Semáforo global para serializar operações EF Core entre módulos
    // (Produtos/Vendas) que compartilham o mesmo DbContext.

    public static class DbSync
    {
        public static readonly SemaphoreSlim Gate = new(1, 1);
    }
}