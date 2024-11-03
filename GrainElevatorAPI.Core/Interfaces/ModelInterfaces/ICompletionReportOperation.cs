namespace GrainElevatorAPI.Core.Interfaces.ModelInterfaces;

public interface ICompletionReportOperation
{
    int Id { get; set; }
    int TechnologicalOperationId { get; set; }
    int Amount { get; set; }
    decimal? OperationCost  {get; set; }
    int? CompletionReportId { get; set; }
    
}