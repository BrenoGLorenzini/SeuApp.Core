namespace SeuApp.WinForms
{
    internal static class AppEvents
    {
        public static event EventHandler? ProdutosAlterados;
        public static void RaiseProdutosAlterados()
            => ProdutosAlterados?.Invoke(null, EventArgs.Empty);
    }
}