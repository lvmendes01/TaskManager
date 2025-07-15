using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using TaskManager.Models;
using TaskManagerAPI.Controllers.v1;
using TaskManagerAPI.Data;
using Xunit;
using TaskStatus = TaskManager.Models.TaskStatus;

namespace TaskManager.Tests
{
    public class ProjetoControllerTests
    {
        private TaskManagerDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<TaskManagerDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new TaskManagerDbContext(options);
        }

        [Fact]
        public async Task GetProjects_ReturnsAllProjects()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Projetos.Add(new Projeto { Id = 1, Name = "Projeto 1" });
            context.Projetos.Add(new Projeto { Id = 2, Name = "Projeto 2" });
            await context.SaveChangesAsync();

            var controller = new ProjetoController(context);

            // Act
            var result = await controller.GetProjects();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var projects = Assert.IsType<List<Projeto>>(okResult.Value);
            Assert.Equal(2, projects.Count);
        }

        [Fact]
        public async Task CreateProject_AddsNewProject()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new ProjetoController(context);
            var newProject = new Projeto { Name = "Novo Projeto" };

            // Act
            var result = await controller.CreateProject(newProject);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var project = Assert.IsType<Projeto>(createdResult.Value);
            Assert.Equal("Novo Projeto", project.Name);
            Assert.Single(context.Projetos);
        }

        [Fact]
        public async Task DeleteProject_RemovesProject_WhenNoPendingTasks()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var project = new Projeto
            {
                Id = 1,
                Name = "Projeto 1",
                Tarefas = new List<Tarefa>
                {
                    new Tarefa { Id = 1, Status = TaskStatus.Concluida }
                }
            };
            context.Projetos.Add(project);
            await context.SaveChangesAsync();

            var controller = new ProjetoController(context);

            // Act
            var result = await controller.DeleteProject(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Empty(context.Projetos);
        }

        [Fact]
        public async Task DeleteProject_ReturnsBadRequest_WhenPendingTasksExist()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var project = new Projeto
            {
                Id = 1,
                Name = "Projeto 1",
                Tarefas = new List<Tarefa>
                {
                    new Tarefa { Id = 1, Status = TaskStatus.Pendente }
                }
            };
            context.Projetos.Add(project);
            await context.SaveChangesAsync();

            var controller = new ProjetoController(context);

            // Act
            var result = await controller.DeleteProject(1);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("O projeto não pode ser removido enquanto houver tarefas pendentes.", badRequestResult.Value);
        }

        [Fact]
        public async Task GetAverageAndCompletionPercentagePerProject_ReturnsCorrectData()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var project1 = new Projeto
            {
                Id = 1,
                Name = "Projeto 1",
                Tarefas = new List<Tarefa>
        {
            new Tarefa { Id = 1, Status = TaskStatus.Concluida, DataVencimento = DateTime.UtcNow.AddDays(-10) },
            new Tarefa { Id = 2, Status = TaskStatus.Pendente, DataVencimento = DateTime.UtcNow.AddDays(-5) }
        }
            };
            var project2 = new Projeto
            {
                Id = 2,
                Name = "Projeto 2",
                Tarefas = new List<Tarefa>
        {
            new Tarefa { Id = 3, Status = TaskStatus.Concluida, DataVencimento = DateTime.UtcNow.AddDays(-20) },
            new Tarefa { Id = 4, Status = TaskStatus.Concluida, DataVencimento = DateTime.UtcNow.AddDays(-15) }
        }
            };
            context.Projetos.AddRange(project1, project2);
            await context.SaveChangesAsync();

            var controller = new ProjetoController(context);

            // Act
            var result = await controller.GetAverageAndCompletionPercentagePerProject();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = okResult.Value;

            // Acessar propriedades dinamicamente
            Assert.NotNull(data);
            var projects = ((dynamic)data).Projects;
            Assert.Equal(2, projects.Count); // 2 projetos
            Assert.Equal(50.0, projects[0].CompletionPercentage); // Projeto 1: 1/2 concluídas
            Assert.Equal(100.0, projects[1].CompletionPercentage); // Projeto 2: 2/2 concluídas
            Assert.Equal(1.5, ((dynamic)data).AverageTasksPerProject); // Média: (1+2)/2
        }
    }
}

