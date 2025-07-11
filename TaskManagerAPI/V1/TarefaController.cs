using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;
using TaskManagerAPI.Data;

namespace TaskManagerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TarefaController : ControllerBase
{
    private readonly TaskManagerDbContext _context;

    public TarefaController(TaskManagerDbContext context)
    {
        _context = context;
    }

    // 2. Visualização de Tarefas
    [HttpGet("{projectId}")]
    public async Task<IActionResult> GetTasks(int projectId)
    {
        var project = await _context.Projetos.Include(p => p.Tarefas).FirstOrDefaultAsync(p => p.Id == projectId);
        if (project == null) return NotFound();

        return Ok(project.Tarefas);
    }

    // 4. Criação de Tarefas
    [HttpPost("{projectId}")]
    public async Task<IActionResult> CreateTask(int projectId, [FromBody] Tarefa tarefa)
    {
        var project = await _context.Projetos.Include(p => p.Tarefas).FirstOrDefaultAsync(p => p.Id == projectId);
        if (project == null) return NotFound();

        project.Tarefas.Add(tarefa);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetTasks), new { projectId = projectId }, tarefa);
    }

    // 5. Atualização de Tarefas
    [HttpPut("{taskId}")]
    public async Task<IActionResult> UpdateTask(int taskId, [FromBody] Tarefa updatedTask)
    {
        var task = await _context.Tarefas.FirstOrDefaultAsync(t => t.Id == taskId);
        if (task == null) return NotFound();

        task.Titulo = updatedTask.Titulo;
        task.Descricao = updatedTask.Descricao;
        task.DataVencimento = updatedTask.DataVencimento;
        task.Status = updatedTask.Status;

        await _context.SaveChangesAsync();
        return Ok(task);
    }

    // 6. Remoção de Tarefas
    [HttpDelete("{taskId}")]
    public async Task<IActionResult> DeleteTask(int taskId)
    {
        var task = await _context.Tarefas.FirstOrDefaultAsync(t => t.Id == taskId);
        if (task == null) return NotFound();

        _context.Tarefas.Remove(task);
        await _context.SaveChangesAsync();
        return NoContent();
    }

}
