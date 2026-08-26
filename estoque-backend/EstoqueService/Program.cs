using Microsoft.EntityFrameworkCore;
using EstoqueService;

var builder = WebApplication.CreateBuilder(args);

// 1. Configura os Controllers + JSON camelCase
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });

// 2. Configura o banco de dados SQLite
builder.Services.AddDbContext<EstoqueContext>(options =>
    options.UseSqlite("Data Source=estoque.db"));

// 3. Configura a política do CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// 4. Ativa o CORS (deve vir ANTES do MapControllers)
app.UseCors("AllowAngular");

app.MapControllers();

app.Run();