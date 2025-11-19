using ApiEmpleadosPorDeptartamentos.Interfaces;
using ApiEmpleadosPorDeptartamentos.Models;
using ApiEmpleadosPorDeptartamentos.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EmpresaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EmpresaDb")));

builder.Services.AddScoped<IEmpresaDbContext>(provider => provider.GetRequiredService<EmpresaDbContext>());
builder.Services.AddScoped<IEmpleadoService, EmpleadoService>();
builder.Services.AddScoped<IDepartamentoService, DepartamentoService>();
builder.Services.AddScoped<IHelperService, HelperService>();


// 🔹 Configurar CORS (para que el frontend Vue pueda consumir la API)
builder.Services.AddCors(options =>
{
    options.AddPolicy("VueCorsPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 🔹 Agregar controladores y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 🔹 Middleware del pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 🔹 Habilitar CORS
app.UseCors("VueCorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
