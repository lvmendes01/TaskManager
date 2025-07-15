namespace TaskManagerAPI.DTOs
{
    public class ProjectPerformanceDto
    {
        public int ProjetoId { get; set; }
        public string ProjetoName { get; set; }
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public double CompletionPercentage { get; set; }
    }
}