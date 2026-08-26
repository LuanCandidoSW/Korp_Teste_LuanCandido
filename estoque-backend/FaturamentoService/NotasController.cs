using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FaturamentoService;

[ApiController]
[Route("api/[controller]")]
public class NotasController : ControllerBase
{
    private readonly FaturamentoContext _context;
    private readonly HttpClient _httpClient;

    public NotasController(FaturamentoContext context, IHttpClientFactory httpClientFactory)
    {
        _context = context;
        _httpClient = httpClientFactory.CreateClient();
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodas()
    {
        var notas = await _context.Notas.Include(n => n.Itens).ToListAsync();
        return Ok(notas);
    }

    [HttpPost]
    public async Task<IActionResult> CriarNota([FromBody] NotaFiscal novaNota)
    {
        if (novaNota.Itens == null || !novaNota.Itens.Any())
        {
            return BadRequest("A nota fiscal deve conter ao menos um item.");
        }

        novaNota.Status = "Aberta";
        _context.Notas.Add(novaNota);
        await _context.SaveChangesAsync();

        novaNota.Numero = $"NF-{novaNota.Id:D4}";
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(ObterTodas), novaNota);
    }

    [HttpPost("{id}/imprimir")]
    public async Task<IActionResult> Imprimir(int id)
    {
        var nota = await _context.Notas.Include(n => n.Itens).FirstOrDefaultAsync(n => n.Id == id);
        if (nota == null) return NotFound("Nota não encontrada.");

        if (nota.Status != "Aberta")
        {
            return BadRequest("Somente notas com status Aberta podem ser impressas.");
        }

        try
        {
            foreach (var item in nota.Itens)
            {
                var response = await _httpClient.PostAsJsonAsync(
                    $"http://localhost:5293/api/produtos/{item.CodigoProduto}/dar-baixa",
                    new { Quantidade = item.Quantidade }
                );

                if (!response.IsSuccessStatusCode)
                {
                    var erro = await response.Content.ReadAsStringAsync();
                    return BadRequest($"Falha ao dar baixa no produto {item.CodigoProduto}: {erro}");
                }
            }
        }
        catch (HttpRequestException)
        {
            return StatusCode(503, "O serviço de Estoque está indisponível no momento. Tente novamente em instantes.");
        }

        nota.Status = "Fechada";
        await _context.SaveChangesAsync();
        return Ok(nota);
    }
}