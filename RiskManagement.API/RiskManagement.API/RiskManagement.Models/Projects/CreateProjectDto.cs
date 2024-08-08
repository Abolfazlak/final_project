namespace RiskManagement.API.RiskManagement.Models.Projects;

public class CreateProjectDto
{ 
    public string ProjectName { get; set; }
    public string Methodology { get; set; }
    public string? Description { get; set; }
    public long? AssigneeUserId { get; set; }
}

public class UpdateProjectDto : CreateProjectDto
{
    public long Id { get; set; }
}
