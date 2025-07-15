using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models;

public class Projeto
{
    
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Tarefa> Tarefas { get; set; } = new();
}
