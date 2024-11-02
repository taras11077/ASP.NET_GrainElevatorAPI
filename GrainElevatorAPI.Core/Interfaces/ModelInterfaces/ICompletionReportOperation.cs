namespace GrainElevatorAPI.Core.Interfaces.ModelInterfaces;

public interface ICompletionReportOperation
{
    int Id { get; set; }
    string OperationName { get; set; }
    double Amount { get; set; }
    int? CompletionReportId { get; set; }
    
}