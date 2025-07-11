using Microsoft.EntityFrameworkCore;
using TaskManager.Models;

namespace TaskManagerAPI.Data;

public class TaskManagerDbContext : DbContext
{
    public TaskManagerDbContext(DbContextOptions<TaskManagerDbContext> options) : base(options) { }

    public DbSet<Projeto> Projetos { get; set; }
    public DbSet<Tarefa> Tarefas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuração de chave primária para Projeto
        modelBuilder.Entity<Projeto>()
            .HasKey(p => p.Id);

        // Configuração de chave primária para Tarefa
        modelBuilder.Entity<Tarefa>()
            .HasKey(t => t.Id);

        // Relacionamento entre Projeto e Tarefa
        modelBuilder.Entity<Projeto>()
            .HasMany(p => p.Tarefas)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
