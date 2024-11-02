namespace GrainElevatorAPI.Core.Interfaces.ModelInterfaces;

public interface ICompletionReportOperation
{
    int Id { get; set; }
    int TechnologicalOperationId { get; set; }
    double Amount { get; set; }
    int? CompletionReportId { get; set; }
    
}