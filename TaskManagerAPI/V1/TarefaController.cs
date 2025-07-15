using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;
using TaskManagerAPI.Data;

namespace TaskManagerAPI.Controllers.v1;

[ApiController]
[Route("api/[controller]")]
public class TarefaController : ControllerBase
{
    private readonly TaskManagerDbContext _context;

    public TarefaController(TaskManagerDbContext context)
    {
        _context = context;
    }

    
    /// <summary>
    /// Obtém todas as tarefas de um projeto específico.
    /// </summary>
    /// <param name="projectId"></param>
    /// <returns></returns>
    [HttpGet("/TarefasProjeto/{projectId}")]
    public async Task<IActionResult> GetTasks(int projectId)
    {
        var project = await _context.Projetos.Include(p => p.Tarefas).FirstOrDefaultAsync(p => p.Id == projectId);
        if (project == null) return NotFound();

        return Ok(project.Tarefas);
    }
    /// <summary>
    /// Obtem uma tarefa específica de um projeto.
    /// </summary>
    /// <param name="taskId"></param>
    /// <returns></returns>
    [HttpGet("{taskId}")]
    public async Task<IActionResult> GetTasksId(int taskId)
    {
        var tarefa = await _context.Tarefas.FirstOrDefaultAsync(p => p.Id == taskId);
        if (tarefa == null) return NotFound();

        return Ok(tarefa);
    }
    /// <summary>
    /// Cria uma nova tarefa em um projeto específico.
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="tarefa"></param>
    /// <returns></returns>
    [HttpPost("{projectId}")]
    public async Task<IActionResult> CreateTask(int projectId, [FromBody] Tarefa tarefa)
    {
        var project = await _context.Projetos.Include(p => p.Tarefas).FirstOrDefaultAsync(p => p.Id == projectId);
        if (project == null) return NotFound();

        if (project.Tarefas.Count >= 20)
            return BadRequest("O projeto já atingiu o limite máximo de 20 tarefas.");

        project.Tarefas.Add(tarefa);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetTasks), new { projectId = projectId }, tarefa);
    }

    /// <summary>
    /// Atualiza uma tarefa existente em um projeto específico.
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="updatedTask"></param>
    /// <returns></returns>
    [HttpPut("{taskId}")]
    public async Task<IActionResult> UpdateTask(int taskId, [FromBody] Tarefa updatedTask)
    {
        var task = await _context.Tarefas.FirstOrDefaultAsync(t => t.Id == taskId);
        if (task == null) return NotFound();

        // Impedir alteração de prioridade
        if (task.Prioridade != updatedTask.Prioridade)
            return BadRequest("A prioridade de uma tarefa não pode ser alterada.");

        task.Titulo = updatedTask.Titulo;
        task.Descricao = updatedTask.Descricao;
        task.DataVencimento = updatedTask.DataVencimento;
        task.Status = updatedTask.Status;

        await _context.SaveChangesAsync();
        return Ok(task);
    }

    /// <summary>
    /// Exclui uma tarefa específica de um projeto.
    /// </summary>
    /// <param name="taskId"></param>
    /// <returns></returns>
    [HttpDelete("{taskId}")]
    public async Task<IActionResult> DeleteTask(int taskId)
    {
        var task = await _context.Tarefas.FirstOrDefaultAsync(t => t.Id == taskId);
        if (task == null) return NotFound();

        _context.Tarefas.Remove(task);
        await _context.SaveChangesAsync();
        return NoContent();
    }



    private async Task RegistrarHistorico(Tarefa tarefa, string alteracao, string usuario)
    {
        var historico = new HistoricoTarefa
        {
            TarefaId = tarefa.Id,
            Alteracao = alteracao,
            DataAlteracao = DateTime.UtcNow,
            Usuario = usuario
        };

        _context.HistoricoTarefas.Add(historico);
        await _context.SaveChangesAsync();
    }

    [HttpPost("{taskId}/comentarios")]
    public async Task<IActionResult> AddComment(int taskId, [FromBody] Comentario comentario)
    {
        var task = await _context.Tarefas.FirstOrDefaultAsync(t => t.Id == taskId);
        if (task == null) return NotFound();

        comentario.TarefaId = taskId;
        comentario.DataCriacao = DateTime.UtcNow;

        _context.Comentarios.Add(comentario);
        await _context.SaveChangesAsync();

        await RegistrarHistorico(task, $"Comentário adicionado: {comentario.Texto}", "UsuárioAtual");
        return Ok(comentario);
    }
}
