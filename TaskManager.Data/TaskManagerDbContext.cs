using Microsoft.EntityFrameworkCore;
using TaskManager.Models;

namespace TaskManagerAPI.Data;

public class TaskManagerDbContext : DbContext
{
    public TaskManagerDbContext(DbContextOptions<TaskManagerDbContext> options) : base(options) { }

    public DbSet<Projeto> Projetos { get; set; }
    public DbSet<Tarefa> Tarefas { get; set; }
    public DbSet<HistoricoTarefa> HistoricoTarefas { get; set; }
    public DbSet<Comentario> Comentarios { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Projeto>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<HistoricoTarefa>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<Tarefa>()
            .HasKey(t => t.Id);
        modelBuilder.Entity<Comentario>()
            .HasKey(t => t.Id);

        modelBuilder.Entity<Projeto>()
            .HasMany(p => p.Tarefas)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
