using System.ComponentModel.DataAnnotations;

namespace RiskManagement.API.RiskManagement.DataProvide;

public class Risk
{
    [Key]
    public long Id { get; set; }
    public string Title { get; set; }
    public int SecondaryRiskCategoryId { get; set; }
    public long ProjectId { get; set; }

    /*
     * STATUS
     * 0- not finished yet
     * 1- finished and happened
     * 2- finished and not happened
     */
    public int Status { get; set; }
    
    public DateTime? FinishedDate { get; set; }
    public long? FinalAmount { get; set; }
    public long? BestSolutionId { get; set; }
    
    public virtual SecondaryRiskCategory SecondaryRiskCategory { get; set; }
    public virtual Project Project { get; set; }

    public virtual ICollection<RiskDetails> RiskDetails { get; set; }
    public virtual ICollection<Solution> Solutions { get; set; }

}