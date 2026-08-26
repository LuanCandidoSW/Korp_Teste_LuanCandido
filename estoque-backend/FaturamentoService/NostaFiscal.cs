using System.ComponentModel.DataAnnotations;

namespace FaturamentoService;

public class ItemNota
{
    [Key]
    public int Id { get; set; }
    public string CodigoProduto { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public int NotaFiscalId { get; set; }
}

public class NotaFiscal
{
    [Key]
    public int Id { get; set; }
    public string Numero { get; set; } = string.Empty;
    public string Status { get; set; } = "Aberta";
    public List<ItemNota> Itens { get; set; } = new();
}