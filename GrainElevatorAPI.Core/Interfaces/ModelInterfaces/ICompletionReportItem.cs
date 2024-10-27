namespace GrainElevatorAPI.Core.Interfaces.ModelInterfaces;

public interface ICompletionReportItem
{
    int Id { get; set; }
    string TechnologicalOperation { get; set; }
    double Amount { get; set; }
    double Price { get; set; }
    double TotalCost { get; set; }
    int? CompletionReportId { get; set; }
    
}