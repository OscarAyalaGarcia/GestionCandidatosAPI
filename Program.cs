using GestionCandidatosAPI.Repositorios;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios
builder.Services.AddControllers();

// Inyección de dependencias de los repositorios
builder.Services.AddScoped<IRepositorioVacantes, RepositorioVacantes>();
builder.Services.AddScoped<IRepositorioPostulantes, RepositorioPostulantes>();
builder.Services.AddScoped<IRepositorioEntrevistas, RepositorioEntrevistas>();

// Configuracion de swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "GestionCandidatosAPI", Version = "v1" });
});

var app = builder.Build();

// Configurar el pipeline (ACTIVAR LA PANTALLA VISUAL)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "GestionCandidatosAPI v1");
    c.RoutePrefix = string.Empty; // IMPORTANTE: Esto hace que abra directo en localhost
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();