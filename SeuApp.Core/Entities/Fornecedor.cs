namespace SeuApp.Core.Entities;

public class Fornecedor
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public string? Cnpj { get; set; }
    public string? Email { get; set; }
    public string? Telefone { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}
