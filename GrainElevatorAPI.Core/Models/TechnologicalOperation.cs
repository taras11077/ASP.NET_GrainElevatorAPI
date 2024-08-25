namespace GrainElevatorAPI.Core.Models;

public class TechnologicalOperation
{
    public int Id { get; set; }
    public string Title { get; set; }
    public double Amount { get; set; }
    public double Price { get; set; }
    public double TotalCost { get; set; }

    public int CompletionReportId { get; set; }
    public virtual CompletionReport CompletionReport { get; set; }

}

