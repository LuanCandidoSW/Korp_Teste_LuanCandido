using Microsoft.EntityFrameworkCore;

namespace FaturamentoService;

public class FaturamentoContext : DbContext
{
    public FaturamentoContext(DbContextOptions<FaturamentoContext> options) : base(options) { }

    public DbSet<NotaFiscal> Notas { get; set; }
    public DbSet<ItemNota> Itens { get; set; }
}