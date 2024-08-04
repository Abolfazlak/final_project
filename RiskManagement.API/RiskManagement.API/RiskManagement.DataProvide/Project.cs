using System.ComponentModel.DataAnnotations;

namespace RiskManagement.API.RiskManagement.DataProvide;

public class Project
{
    [Key]
    public long Id { get; set; }
    public string ProjectName { get; set; }
    public string Methodology { get; set; }
    public string? Description { get; set; }
    public long CompanyId { get; set; }
    public long? AssigneeUserId { get; set; }
    
    public virtual Company Company { get; set; }
}