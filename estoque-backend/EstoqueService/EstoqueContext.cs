using Microsoft.EntityFrameworkCore;

namespace EstoqueService;

public class EstoqueContext : DbContext
{
    public EstoqueContext(DbContextOptions<EstoqueContext> options) : base(options) { }

    public DbSet<Produto> Produtos { get; set; }
}