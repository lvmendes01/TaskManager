using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;
using TaskManagerAPI.Controllers.v1;
using TaskManagerAPI.Data;
using Xunit;
using TaskStatus = TaskManager.Models.TaskStatus;

namespace TaskManager.Tests
{
    public class TarefaControllerTests
    {
        private TaskManagerDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<TaskManagerDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new TaskManagerDbContext(options);
        }

        [Fact]
        public async Task GetTasks_ReturnsTasksForProject()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var project = new Projeto
            {
                Id = 1,
                Name = "Projeto 1",
                Tarefas = new List<Tarefa>
                {
                    new Tarefa { Id = 1, Titulo = "Tarefa 1", Status = TaskStatus.Concluida },
                    new Tarefa { Id = 2, Titulo = "Tarefa 2", Status = TaskStatus.Pendente }
                }
            };
            context.Projetos.Add(project);
            await context.SaveChangesAsync();

            var controller = new TarefaController(context);

            // Act
            var result = await controller.GetTasks(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var tasks = Assert.IsType<List<Tarefa>>(okResult.Value);
            Assert.Equal(2, tasks.Count);
        }

        [Fact]
        public async Task GetTasks_ReturnsNotFound_WhenProjectDoesNotExist()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new TarefaController(context);

            // Act
            var result = await controller.GetTasks(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateTask_AddsTaskToProject()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var project = new Projeto
            {
                Id = 1,
                Name = "Projeto 1",
                Tarefas = new List<Tarefa>()
            };
            context.Projetos.Add(project);
            await context.SaveChangesAsync();

            var controller = new TarefaController(context);
            var newTask = new Tarefa
            {
                Titulo = "Nova Tarefa",
                Descricao = "Descrição da tarefa",
                DataVencimento = DateTime.UtcNow.AddDays(5),
                Status = TaskStatus.Pendente
            };

            // Act
            var result = await controller.CreateTask(1, newTask);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var task = Assert.IsType<Tarefa>(createdResult.Value);
            Assert.Equal("Nova Tarefa", task.Titulo);
            Assert.Single(context.Tarefas);
        }

        [Fact]
        public async Task CreateTask_ReturnsBadRequest_WhenTaskLimitExceeded()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var project = new Projeto
            {
                Id = 1,
                Name = "Projeto 1",
                Tarefas = Enumerable.Range(1, 20).Select(i => new Tarefa { Titulo = $"Tarefa {i}" }).ToList()
            };
            context.Projetos.Add(project);
            await context.SaveChangesAsync();

            var controller = new TarefaController(context);
            var newTask = new Tarefa
            {
                Titulo = "Nova Tarefa",
                Descricao = "Descrição da tarefa",
                DataVencimento = DateTime.UtcNow.AddDays(5),
                Status = TaskStatus.Pendente
            };

            // Act
            var result = await controller.CreateTask(1, newTask);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("O projeto já atingiu o limite máximo de 20 tarefas.", badRequestResult.Value);
        }

        [Fact]
        public async Task DeleteTask_RemovesTask()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var task = new Tarefa
            {
                Id = 1,
                Titulo = "Tarefa 1",
                Status = TaskStatus.Pendente
            };
            context.Tarefas.Add(task);
            await context.SaveChangesAsync();

            var controller = new TarefaController(context);

            // Act
            var result = await controller.DeleteTask(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Empty(context.Tarefas);
        }

        [Fact]
        public async Task DeleteTask_ReturnsNotFound_WhenTaskDoesNotExist()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new TarefaController(context);

            // Act
            var result = await controller.DeleteTask(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
