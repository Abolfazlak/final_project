namespace RiskManagement.API.RiskManagement.Models.Projects;

public class CreateProjectDto
{
    public long Id { get; set; }
    public string ProjectName { get; set; }
    public string Methodology { get; set; }
    public string? Description { get; set; }
    public long? AssigneeUserId { get; set; }
}