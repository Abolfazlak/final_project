using System.ComponentModel.DataAnnotations;

namespace RiskManagement.API.RiskManagement.DataProvide;

public class Solution
{
    [Key]
    public long Id { get; set; }
    public string Description { get; set; }
    public long Amount { get; set; }
    
    public long RiskId { get; set; }
    public virtual Risk Risk { get; set; }
}