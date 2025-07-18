using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models;

public class Tarefa
{
    
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public DateTime DataVencimento { get; set; }
    public TaskStatus Status { get; set; } = TaskStatus.Pendente;
    public Prioridade Prioridade { get; set; }

}
public enum Prioridade
{
    Baixa,
    Media,
    Alta
}
public enum TaskStatus
{
    Pendente,
    Andamento,
    Concluida
}

