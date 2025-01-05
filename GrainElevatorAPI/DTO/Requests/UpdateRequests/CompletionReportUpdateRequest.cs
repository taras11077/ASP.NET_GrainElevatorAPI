using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.DTO.Requests.UpdateRequests;

public class CompletionReportUpdateRequest
{
    [MinLength(3, ErrorMessage = "ReportNumber must be at least 3 characters long.")]
    [MaxLength(9, ErrorMessage = "ReportNumber must be at least 9 characters long.")]
    public string? ReportNumber { get; set; }
    
    [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
    [Range(typeof(DateTime), "1900-01-01", "2075-12-31", ErrorMessage = "ReportDate must be between 1900 and 2075.")]
    public DateTime? ReportDate { get; set; }

}