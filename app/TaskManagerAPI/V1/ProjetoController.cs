using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;
using TaskManagerAPI.Data;
using TaskManagerAPI.DTOs;
using TaskStatus = TaskManager.Models.TaskStatus;

namespace TaskManagerAPI.Controllers.v1;

[ApiController]
[Route("api/[controller]")]
public class ProjetoController : ControllerBase
{
    private readonly TaskManagerDbContext _context;

    public ProjetoController(TaskManagerDbContext context)
    {
        _context = context;
    }


    // 1. Listagem de Projetos
    [HttpGet]
    public async Task<IActionResult> GetProjects()
    {
        var projects = await _context.Projetos.Include(p => p.Tarefas).ToListAsync();
        return Ok(projects);
    }

    // 3. Criação de Projetos
    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] Projeto projeto)
    {
        _context.Projetos.Add(projeto);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProjects), new { id = projeto.Id }, projeto);
    }

    [HttpDelete("{projectId}")]
    public async Task<IActionResult> DeleteProject(int projectId)
    {
        var project = await _context.Projetos.Include(p => p.Tarefas).FirstOrDefaultAsync(p => p.Id == projectId);
        if (project == null) return NotFound();

        if (project.Tarefas.Any(t => t.Status != TaskStatus.Concluida))
            return BadRequest("O projeto não pode ser removido enquanto houver tarefas pendentes.");

        _context.Projetos.Remove(project);
        await _context.SaveChangesAsync();
        return NoContent();
    }


    [HttpGet("relatorios/desempenho")]
    //[Authorize(Roles = "Gerente")] // Restringir acesso
    public async Task<IActionResult> GetAverageAndCompletionPercentagePerProject()
    {
        var last30Days = DateTime.UtcNow.AddDays(-30);

        var completedTasksByProject = await _context.Projetos
            .Include(p => p.Tarefas)
            .Select(p => new ProjectPerformanceDto
            {
                ProjetoId = p.Id,
                ProjetoName = p.Name,
                TotalTasks = p.Tarefas.Count,
                CompletedTasks = p.Tarefas
                    .Where(t => t.Status == TaskStatus.Concluida && t.DataVencimento >= last30Days)
                    .Count(),
                CompletionPercentage = p.Tarefas.Count > 0
                    ? (double)p.Tarefas.Count(t => t.Status == TaskStatus.Concluida && t.DataVencimento >= last30Days) / p.Tarefas.Count * 100
                    : 0
            })
            .ToListAsync();

        var totalProjects = completedTasksByProject.Count;
        var totalTasks = completedTasksByProject.Sum(x => x.CompletedTasks);
        var averageTasksPerProject = totalProjects > 0 ? (double)totalTasks / totalProjects : 0;

        var response = new PerformanceReportDto
        {
            AverageTasksPerProject = averageTasksPerProject,
            Projects = completedTasksByProject
        };

        return Ok(response);
    }


}
