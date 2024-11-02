using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.DTO.Requests.UpdateRequests;

public class CompletionReportOperationUpdateRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "TechnologicalOperationId must be a positive number.")]
    public int? TechnologicalOperationId { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Amount must be a positive number.")]
    public double? Amount { get; set; }
}