using Microsoft.EntityFrameworkCore;
using XPeriencia.API.Data;
using XPeriencia.API.Models;
using XPeriencia.API.Services;

var builder = WebApplication.CreateBuilder(args);

// ===== CONFIGURAÇÃO DE SERVIÇOS =====

// Adiciona suporte a controllers (MVC pattern)
builder.Services.AddControllers();

// Configura o Entity Framework Core com SQLite
// Connection string definida no appsettings.json
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registra HttpClient com tempo de vida Scoped e injeta no ExternalApiService
// Permite consumo de APIs externas de forma eficiente e com pool de conexões
builder.Services.AddHttpClient<ExternalApiService>();

// Configura gerador de documentação OpenAPI (Swagger)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Informações da API exibidas no Swagger UI
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "XPeri�ncia API",
        Version = "v1",
        Description = "API para gerenciamento de usu�rios, apostas fict�cias e reflex�es pessoais. Projeto Sprint 4 de C#",

    });

});

// Configura CORS (Cross-Origin Resource Sharing)
// Permite que a API seja consumida por aplicações frontend hospedadas em outros domínios 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()  // Permite requisições de qualquer origem
                   .AllowAnyMethod()  // Permite qualquer método HTTP (GET, POST, PUT, DELETE)
                   .AllowAnyHeader(); // Permite qualquer header nas requisições
        });
});

var app = builder.Build();

// ===== CONFIGURAÇÃO DO PIPELINE HTTP =====

// Habilita Swagger apenas em ambiente de desenvolvimento
// Em produção, considere adicionar autenticação para o Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware para redirecionar requisições HTTP para HTTPS
app.UseHttpsRedirection();

// Aplica a política de CORS configurada
app.UseCors("AllowAll");

// Middleware de autorização (preparado para futuras implementações de segurança)
app.UseAuthorization();

// Mapeia os controllers para as rotas da API
app.MapControllers();

// Inicia a aplicação
app.Run();
