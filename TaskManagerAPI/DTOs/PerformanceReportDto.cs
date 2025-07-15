namespace TaskManagerAPI.DTOs
{
    public class PerformanceReportDto
    {
        public double AverageTasksPerProject { get; set; }
        public List<ProjectPerformanceDto> Projects { get; set; }
    }
}
