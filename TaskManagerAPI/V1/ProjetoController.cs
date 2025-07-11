using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;
using TaskManagerAPI.Data;

namespace TaskManagerAPI.Controllers;

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


}
