using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.DTO.Requests.CreateRequests;

public class CompletionReportOperationCreateRequest
{
    [Required(ErrorMessage = "TechnologicalOperationId is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "TechnologicalOperationId must be a positive number.")]
    public int TechnologicalOperationId { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Amount must be a positive number.")]
    public double Amount { get; set; }
}