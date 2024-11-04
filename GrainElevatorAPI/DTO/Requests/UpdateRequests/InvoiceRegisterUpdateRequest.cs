using System.ComponentModel.DataAnnotations;
using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.DTO.Requests.UpdateRequests;

public class InvoiceRegisterUpdateRequest
{
    [MinLength(3, ErrorMessage = "RegisterNumber must be at least 3 characters long.")]
    [MaxLength(9, ErrorMessage = "RegisterNumber must be at least 9 characters long.")]
    public string? RegisterNumber { get; set; }
    
    
    [Range(0.0, 100.0, ErrorMessage = "WeedImpurityBase value must be between 0.0 and 100.0")]
    public double? WeedImpurityBase { get; set; }
    
    
    [Range(0.0, 100.0, ErrorMessage = "MoistureBase value must be between 0.0 and 100.0")]
    public double? MoistureBase { get; set; }
    
    
    public List<int>? LaboratoryCardIds { get; set; }
}