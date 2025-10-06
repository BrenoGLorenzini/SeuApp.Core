namespace SeuApp.Core.Entities;

public class VendaItem
{
    public int Id { get; set; }
    public int VendaId { get; set; }
    public int ProdutoId { get; set; }

    // Snapshot para relatório e consistência
    public string Descricao { get; set; } = null!;
    public decimal Quantidade { get; set; }
    public decimal ValorUnit { get; set; }

    public Produto? Produto { get; set; }
    public Venda? Venda { get; set; }

    public decimal TotalItem => Quantidade * ValorUnit;
}
