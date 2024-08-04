
using RiskManagement.API.RiskManagement.Models.Users;

namespace RiskManagement.API.RiskManagement.Models.Projects;

public class ProjectDto
{
    public long Id { get; set; }
    public string ProjectName { get; set; }
    public string Methodology { get; set; }
    public string? Description { get; set; }
}

public class ProjectWithAssigneeDto : ProjectDto
{
    public UserDto? UserDto { get; set; }
}