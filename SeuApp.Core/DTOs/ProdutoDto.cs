namespace SeuApp.Core.DTOs;

public record ProdutoDto(int Id, int Qtd, string Descricao, decimal Valor, string? Observacao);
