using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EstoqueService;

[ApiController]
[Route("api/[controller]")]
public class ProdutosController : ControllerBase
{
    private readonly EstoqueContext _context;

    public ProdutosController(EstoqueContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodos()
    {
        return Ok(await _context.Produtos.ToListAsync());
    }

    [HttpPost]
    public async Task<IActionResult> Cadastrar([FromBody] Produto novoProduto)
    {
        if (await _context.Produtos.AnyAsync(p => p.Codigo == novoProduto.Codigo))
        {
            return BadRequest("Produto com este código já existe.");
        }

        _context.Produtos.Add(novoProduto);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(ObterTodos), novoProduto);
    }

    [HttpPost("{codigo}/dar-baixa")]
    public async Task<IActionResult> DarBaixa(string codigo, [FromBody] BaixaEstoqueDto dto)
    {
        var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.Codigo == codigo);
        if (produto == null) return NotFound("Produto não encontrado.");

        if (produto.Saldo < dto.Quantidade)
        {
            return BadRequest("Saldo insuficiente em estoque.");
        }

        produto.Saldo -= dto.Quantidade;
        await _context.SaveChangesAsync();
        return Ok(produto);
    }

    [HttpPost("{codigo}/adicionar-saldo")]
    public async Task<IActionResult> AdicionarSaldo(string codigo, [FromBody] BaixaEstoqueDto dto)
    {
        var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.Codigo == codigo);
        if (produto == null) return NotFound("Produto não encontrado.");

        produto.Saldo += dto.Quantidade;
        await _context.SaveChangesAsync();
        return Ok(produto);
    }
}