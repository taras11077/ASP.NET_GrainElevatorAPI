using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.DTO.Requests.UpdateRequests;

namespace GrainElevatorAPI.Extensions;

public static class CompletionReportExtensions
{
    public static void UpdateFromRequest(this CompletionReport report, CompletionReportUpdateRequest request)
    {
        report.ReportNumber = request.ReportNumber ?? report.ReportNumber;
        report.ReportDate = request.ReportDate ?? report.ReportDate;
    }
}