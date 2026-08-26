using System.ComponentModel.DataAnnotations;

namespace EstoqueService;

public class Produto
{
    [Key]
    public string Codigo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public int Saldo { get; set; }
}

public class BaixaEstoqueDto
{
    public int Quantidade { get; set; }
}