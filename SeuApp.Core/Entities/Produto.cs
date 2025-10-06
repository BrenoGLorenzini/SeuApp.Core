namespace SeuApp.Core.Entities;

public class Produto
{
    public int Id { get; set; }
    public int Quantidade { get; set; }
    public string Descricao { get; set; } = null!;
    public decimal Valor { get; set; }
    public string? Observacao { get; set; }
    public int? FornecedorId { get; set; }
    public Fornecedor? Fornecedor { get; set; }
}
