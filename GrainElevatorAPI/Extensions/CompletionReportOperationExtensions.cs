using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.DTO.Requests.UpdateRequests;

namespace GrainElevatorAPI.Extensions;

public static class CompletionReportOperationExtensions
{
    public static void UpdateFromRequest(this CompletionReportOperation operation, CompletionReportOperationUpdateRequest request)
    {
        operation.TechnologicalOperationId = request.TechnologicalOperationId ?? operation.TechnologicalOperationId;
        operation.Amount = request.Amount ?? operation.Amount;
    }
}