using Microsoft.EntityFrameworkCore;
using TaskManager.Models;
using TaskManagerAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer(); // Required for Swagger
builder.Services.AddSwaggerGen(); // Adds Swagger generator
builder.Services.AddDbContext<TaskManagerDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 33))
    ));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Enable Swagger middleware
    app.UseSwaggerUI(); // Enable Swagger UI
}

app.UseHttpsRedirection();

app.MapGet("/api/projects", async (TaskManagerDbContext db) => await db.Projetos.ToListAsync());
app.MapPost("/api/projects", async (TaskManagerDbContext db, Projeto projeto) =>
{
    db.Projetos.Add(projeto);
    await db.SaveChangesAsync();
    return Results.Created($"/api/projects/{projeto.Id}", projeto);
});

app.Run();
