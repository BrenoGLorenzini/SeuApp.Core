namespace SeuApp.Core.Entities;

public class Venda
{
    public int Id { get; set; }
    public string NomeCliente { get; set; } = null!;
    public string? Cpf { get; set; }
    public decimal Total { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    public ICollection<VendaItem> Itens { get; set; } = new List<VendaItem>();
}
