using Microsoft.EntityFrameworkCore;
using ControlTemperaturas.API.Models; // namespace que generó el Scaffold

var builder = WebApplication.CreateBuilder(args);

// =============================
// servicios al contenedor
// =============================
builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        opt.JsonSerializerOptions.WriteIndented = true; // opcional, JSON más legible


    });


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// =============================
// Configuración de DbContext
// =============================
builder.Services.AddDbContext<ControlTemperaturasContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// =============================
// Configuración de CORS
// =============================
var misReglasCors = "ReglasCors";
builder.Services.AddCors(opt =>
{
    opt.AddPolicy(name: misReglasCors, policyBuilder =>
    {
        policyBuilder
            .AllowAnyOrigin()   //  En producción conviene restringir a tu dominio frontend
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// =============================
// Configuración del pipeline HTTP
// =============================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(misReglasCors);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();



public partial class Program { }



