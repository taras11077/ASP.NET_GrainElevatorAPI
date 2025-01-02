using System.ComponentModel.DataAnnotations;
using GrainElevatorAPI.DTO.DTOs;

namespace GrainElevatorAPI.DTO.Requests.CreateRequests;

public class WarehouseUnitCreateRequest
{
    [MinLength(2, ErrorMessage = "Title must be at least 2 characters long.")]
    [MaxLength(20, ErrorMessage = "Title must be at least 20 characters long.")]
    public string SupplierTitle { get; set; }
    
    [MinLength(2, ErrorMessage = "Title must be at least 2 characters long.")]
    [MaxLength(20, ErrorMessage = "Title must be at least 20 characters long.")]
    public string ProductTitle { get; set; }
}