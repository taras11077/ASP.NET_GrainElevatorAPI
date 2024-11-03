using System.ComponentModel.DataAnnotations;
using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.DTO.DTOs;

namespace GrainElevatorAPI.DTO.Requests.CreateRequests;

public class CompletionReportCreateRequest
{
    [MinLength(3, ErrorMessage = "ReportNumber must be at least 3 characters long.")]
    [MaxLength(9, ErrorMessage = "ReportNumber must be at least 9 characters long.")]
    public string ReportNumber { get; set; }
    
    public List<int> RegisterIds { get; set; } = new List<int>();
    public List<int> OperationIds { get; set; } = new List<int>();
}